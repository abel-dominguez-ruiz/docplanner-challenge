namespace DocPlanner.Client
{
    public static class DocPlannerAPIExtensions
    {
        public static IServiceCollection AddDocPlannerAPIClient(
            this IServiceCollection services,
            Action<DocPlannerConfig> docplannerconfig)
        {
            var builder = new DocPlannerConfig();
            docplannerconfig?.Invoke(builder);

            services.AddSingleton(builder);
            services.AddSingleton<IBaseAuthentication, BaseAuthenticationService>(opts => new BaseAuthenticationService(builder.UserName, builder.Password));
            services.AddTransient<AuthorizationHandler>();
            services.AddHttpClient<IDocPlannerClient, DocPlannerClient>(opts =>
            {
                opts.BaseAddress = new Uri(builder.BaseUrl);
                opts.Timeout = TimeSpan.FromSeconds(10);
            }).AddHttpMessageHandler<AuthorizationHandler>()
                .AddPolicyHandler(DocPlannerAPIClientRetryPolicy());

            return services;
        }


        private static IAsyncPolicy<HttpResponseMessage> DocPlannerAPIClientRetryPolicy()
        {
            const int maxRetryAttempts = 3;
            TimeSpan retryInterval = TimeSpan.FromSeconds(1);

            return Policy.HandleResult<HttpResponseMessage>(r => (int)r.StatusCode >= 400 && (int)r.StatusCode < 500)
             .WaitAndRetryAsync(maxRetryAttempts, _ => retryInterval);
        }
    }
}

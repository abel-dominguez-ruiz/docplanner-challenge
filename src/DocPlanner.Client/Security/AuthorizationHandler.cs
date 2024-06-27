
namespace DocPlanner.Client.Security
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly IBaseAuthentication _baseAuthentication;

        public AuthorizationHandler(
            IBaseAuthentication baseAuthentication)
        {
            _baseAuthentication = baseAuthentication;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.Headers.Add("Authorization", _baseAuthentication.GetAuthentication());
            return await base.SendAsync(request, cancellationToken);
        }
    }
}

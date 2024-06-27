
namespace VerticalSlice
{
    public static class VerticalSliceCollectionExtensions
    {

        public static IServiceCollection AddMediatRPipelines(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(VerticalSliceCollectionExtensions).Assembly))
                .AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }

        public static IServiceCollection AddFeaturesAssembly(this IServiceCollection services, Assembly assembly)
        {
            assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));

            services.AddValidatorsFromAssembly(assembly);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
            services.AddMvcCore().AddApplicationPart(assembly);

            return services;
        }

    }
}
namespace doctor_slot.Configuration
{
    public static class ConfigProvider
    {
        public static IConfiguration Configuration;

        public static IConfiguration GetConfig()
        {
            Configuration ??= new ConfigurationBuilder()
                        .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                        .AddEnvironmentVariables()
                        .Build();

            return Configuration;
        }
    }

    public class AppSettingsProvider
    {
        private static IConfiguration Config => ConfigProvider.GetConfig();
        public static DocPlannerOptions DocPlannerOptions => GetDocPlannerOptions();


        private static DocPlannerOptions GetDocPlannerOptions()
        {
            var result = new DocPlannerOptions();
            Config.GetSection("DocPlannerOptions").Bind(result);
            return result;
        }
    }
}

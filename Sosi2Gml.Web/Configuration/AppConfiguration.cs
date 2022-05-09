using Sosi2Gml.Application.Models.Config;

namespace Sosi2Gml.Web.Configuration
{
    public static class AppConfiguration
    {
        public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var datasets = new Dictionary<string, DatasetSettings>();
            configuration.GetSection(Datasets.SectionName).Bind(datasets);
            var datasetConfiguration = new Datasets(datasets);

            services.AddSingleton(datasetConfiguration);

            var coordinateReferenceSystems= new Dictionary<string, string>();
            configuration.GetSection(CrsSettings.SectionName).Bind(coordinateReferenceSystems);
            var crsConfiguration = new CrsSettings(coordinateReferenceSystems);

            services.AddSingleton(crsConfiguration);
        }
    }
}

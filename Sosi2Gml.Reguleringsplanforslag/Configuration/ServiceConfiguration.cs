using Microsoft.Extensions.DependencyInjection;
using Sosi2Gml.Reguleringsplanforslag.Mappers;
using Sosi2Gml.Reguleringsplanforslag.Services;

namespace Sosi2Gml.Reguleringsplanforslag.Configuration
{
    public static class ServiceConfiguration
    {
        public static void AddApplicationServicesForReguleringsplanforslag(this IServiceCollection services)
        {
            services.AddTransient<IRpfSosi2GmlService, RpfSosi2GmlService>();
            services.AddTransient<IArealplanMapper, ArealplanMapper>();
            services.AddTransient<IRpBestemmelseMidlByggAnleggMapper, RpBestemmelseMidlByggAnleggMapper>();
        }
    }
}

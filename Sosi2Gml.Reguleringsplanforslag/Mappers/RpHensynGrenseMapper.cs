using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Models;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public interface IRpHensynGrenseMapper
    {
        T Map<T>(SosiObject sosiObject, string srsName, int decimalPlaces) where T : RpHensynGrense;
    }

    public class RpHensynGrenseMapper : IRpHensynGrenseMapper
    {
        public RpHensynGrenseMapper()
        {
        }

        public T Map<T>(SosiObject sosiObject, string srsName, int decimalPlaces) where T : RpHensynGrense
        {
            return Activator.CreateInstance(typeof(T), new object[] { sosiObject, srsName, decimalPlaces }) as T;
        }
    }
}

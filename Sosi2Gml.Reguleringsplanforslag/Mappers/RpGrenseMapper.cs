using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Models;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public class RpGrenseMapper : IGmlCurveFeatureMapper<RpGrense>
    {
        public RpGrenseMapper()
        {
        }

        public RpGrense Map(SosiObject sosiObject, string srsName, int decimalPlaces)
        {
            return new RpGrense(sosiObject, srsName, decimalPlaces);
        }
    }
}

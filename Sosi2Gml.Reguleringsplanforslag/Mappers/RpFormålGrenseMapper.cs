using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Models;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public class RpFormålGrenseMapper : IGmlCurveFeatureMapper<RpFormålGrense>
    {
        public RpFormålGrenseMapper()
        {
        }

        public RpFormålGrense Map(SosiObject sosiObject, string srsName, int decimalPlaces)
        {
            return new RpFormålGrense(sosiObject, srsName, decimalPlaces);
        }
    }
}

using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Models;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public class RpOmrådeMapper : IGmlSurfaceFeatureMapper<RpOmråde, RpGrense>
    {
        public RpOmrådeMapper()
        {
        }

        public RpOmråde Map(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpGrense> curveFeatures)
        {
            return new RpOmråde(sosiObject, srsName, decimalPlaces, curveFeatures);
        }
    }
}

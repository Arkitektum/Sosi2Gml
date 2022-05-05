using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Models;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public class RpArealformålOmrådeMapper : IGmlSurfaceFeatureMapper<RpArealformålOmråde, RpFormålGrense>
    {
        public RpArealformålOmrådeMapper()
        {
        }

        public RpArealformålOmråde Map(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpFormålGrense> curveFeatures)
        {
            return new RpArealformålOmråde(sosiObject, srsName, decimalPlaces, curveFeatures);
        }
    }
}

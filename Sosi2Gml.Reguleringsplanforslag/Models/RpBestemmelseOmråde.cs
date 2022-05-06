using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpBestemmelseOmråde")]
    public class RpBestemmelseOmråde : SurfaceFeature
    {
        public RpBestemmelseOmråde(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<CurveFeature> curveFeatures) : 
            base(sosiObject, srsName, decimalPlaces, curveFeatures)
        {
        }

        public override string FeatureName => "RpBestemmelseOmråde";
        public List<RpPåskrift> Påskrifter { get; set; }

        public override XElement ToGml(XNamespace appNs)
        {
            throw new NotImplementedException();
        }
    }
}

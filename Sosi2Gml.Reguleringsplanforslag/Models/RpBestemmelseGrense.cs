using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpBestemmelseGrense")]
    public class RpBestemmelseGrense : CurveFeature
    {
        public RpBestemmelseGrense(SosiObject sosiObject, string srsName, int decimalPlaces) : 
            base(sosiObject, srsName, decimalPlaces)
        {
        }

        public override string FeatureName => "RpBestemmelseGrense";
        public override XElement ToGml(XNamespace appNs) => null;
    }
}

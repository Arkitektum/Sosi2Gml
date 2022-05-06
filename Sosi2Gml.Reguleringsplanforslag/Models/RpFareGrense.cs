using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpFareGrense")]
    public class RpFareGrense : CurveFeature
    {
        public RpFareGrense(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
        }

        public override string FeatureName => "RpFareGrense";

        public override XElement ToGml(XNamespace appNs) => null;
    }
}

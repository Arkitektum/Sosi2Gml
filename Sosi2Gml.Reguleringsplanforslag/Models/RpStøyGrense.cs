using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpStøyGrense")]
    public class RpStøyGrense : CurveFeature
    {
        public RpStøyGrense(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
        }

        public override string FeatureName => "RpStøyGrense";
        public override XElement ToGml(XNamespace appNs) => null;
    }
}

using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpBåndleggingGrense")]
    public class RpBåndleggingGrense : CurveFeature
    {
        public RpBåndleggingGrense(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
        }

        public override string FeatureName => "RpBåndleggingGrense";

        public override XElement ToGml(XNamespace appNs) => null;
    }
}

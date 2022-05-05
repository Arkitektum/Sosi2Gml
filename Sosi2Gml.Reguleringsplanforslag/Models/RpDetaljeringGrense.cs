using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpDetaljeringGrense")]
    public class RpDetaljeringGrense : CurveFeature
    {
        public RpDetaljeringGrense(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
        }

        public override string FeatureName => "RpDetaljeringGrense";

        public override XElement ToGml() => null;
    }
}

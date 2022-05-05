using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpGjennomføringGrense")]
    public class RpGjennomføringGrense : CurveFeature
    {
        public RpGjennomføringGrense(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
        }

        public override string FeatureName => "RpGjennomføringGrense";

        public override XElement ToGml() => null;
    }
}

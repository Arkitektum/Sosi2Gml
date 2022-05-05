using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpAngittHensynGrense")]
    public class RpAngittHensynGrense : CurveFeature
    {
        public RpAngittHensynGrense(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
        }

        public override string FeatureName => "RpAngittHensynGrense";

        public override XElement ToGml() => null;
    }
}

using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpInfrastrukturGrense")]
    public class RpInfrastrukturGrense : CurveFeature
    {
        public RpInfrastrukturGrense(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
        }

        public override string FeatureName => "RpInfrastrukturGrense";
        public override XElement ToGml(XNamespace appNs) => null;
    }
}

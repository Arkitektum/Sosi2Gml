using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpInfrastrukturSone")]
    public class RpInfrastrukturSone : RpHensynSone
    {
        public RpInfrastrukturSone(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpInfrastrukturGrense> curveFeature) : 
            base(sosiObject, srsName, decimalPlaces, curveFeature)
        {
            Infrastruktur = sosiObject.GetValue("..RPINFRASTRUKTUR");
        }

        public override string FeatureName => "RpInfrastrukturSone";
        public string Infrastruktur { get; set; }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "infrastruktur", Infrastruktur));

            return featureMember;
        }
    }
}

using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

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

        public string Infrastruktur { get; set; }
        public override string FeatureName => "RpInfrastrukturSone";

        public override XElement ToGml()
        {
            var featureMember = base.ToGml();

            featureMember.Add(new XElement(AppNs + "infrastruktur", Infrastruktur));

            return featureMember;
        }
    }
}

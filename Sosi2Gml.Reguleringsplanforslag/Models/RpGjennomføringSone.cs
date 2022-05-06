using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpGjennomføringSone")]
    public class RpGjennomføringSone : RpHensynSone
    {
        public RpGjennomføringSone(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpGjennomføringGrense> curveFeatures) : 
            base(sosiObject, srsName, decimalPlaces, curveFeatures)
        {
            Gjennomføring = sosiObject.GetValue("..RPGJENNOMFØRING");
        }

        public string Gjennomføring { get; set; }
        public override string FeatureName => "RpGjennomføringSone";

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "gjennomføring", Gjennomføring));

            return featureMember;
        }
    }
}

using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpSikringSone")]
    public class RpSikringSone : RpHensynSone
    {
        public RpSikringSone(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpSikringGrense> curveFeatures) : 
            base(sosiObject, srsName, decimalPlaces, curveFeatures)
        {
            Sikring = sosiObject.GetValue("..RPSIKRING");
        }

        public override string FeatureName => "RpSikringSone";
        public string Sikring { get; set; }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "sikring", Sikring));

            return featureMember;
        }
    }
}

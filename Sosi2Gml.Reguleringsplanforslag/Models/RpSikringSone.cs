using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

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

        public string Sikring { get; set; }
        public override string FeatureName => "RpSikringSone";

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "sikring", Sikring));

            return featureMember;
        }
    }
}

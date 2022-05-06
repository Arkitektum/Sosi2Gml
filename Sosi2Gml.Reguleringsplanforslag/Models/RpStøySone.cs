using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpStøySone")]
    public class RpStøySone : RpHensynSone
    {
        public RpStøySone(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpStøyGrense> curveFeatures) : 
            base(sosiObject, srsName, decimalPlaces, curveFeatures)
        {
            Støy = sosiObject.GetValue("..RPSTØY");
        }

        public string Støy { get; set; }
        public override string FeatureName => "RpStøySone";

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "støy", Støy));

            return featureMember;
        }
    }
}

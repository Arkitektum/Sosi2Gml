using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

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

        public override XElement ToGml()
        {
            var featureMember = base.ToGml();

            featureMember.Add(new XElement(AppNs + "støy", Støy));

            return featureMember;
        }
    }
}

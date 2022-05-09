using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpAngittHensynSone")]
    public class RpAngittHensynSone : RpHensynSone
    {
        public RpAngittHensynSone(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpAngittHensynGrense> curveFeatures) : 
            base(sosiObject, srsName, decimalPlaces, curveFeatures)
        {
            AngittHensyn = sosiObject.GetValue("..RPANGITTHENSYN");
        }

        public override string FeatureName => "RpAngittHensynSone";
        public string AngittHensyn { get; set; }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "angittHensyn", AngittHensyn));

            return featureMember;
        }
    }
}

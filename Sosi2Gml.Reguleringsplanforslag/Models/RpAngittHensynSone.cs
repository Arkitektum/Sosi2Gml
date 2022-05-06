using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

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

        public string AngittHensyn { get; set; }
        public override string FeatureName => "RpAngittHensynSone";

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "angittHensyn", AngittHensyn));

            return featureMember;
        }
    }
}

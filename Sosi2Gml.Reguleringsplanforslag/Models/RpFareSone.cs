using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpFareSone")]
    public class RpFareSone : RpHensynSone
    {
        public RpFareSone(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpFareGrense> curveFeatures) : 
            base(sosiObject, srsName, decimalPlaces, curveFeatures)
        {
            Fare = sosiObject.GetValue("..RPFARE");
        }

        public override string FeatureName => "RpFareSone";
        public string Fare { get; set; }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "fare", Fare));

            return featureMember;
        }
    }
}

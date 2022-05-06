using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

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

        public string Fare { get; set; }
        public override string FeatureName => "RpFareSone";

        public override XElement ToGml()
        {
            var featureMember = base.ToGml();

            featureMember.Add(new XElement(AppNs + "fare", Fare));

            return featureMember;
        }
    }
}

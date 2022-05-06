using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpBåndleggingSone")]
    public class RpBåndleggingSone : RpHensynSone
    {
        public RpBåndleggingSone(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpBåndleggingGrense> curveFeatures) : 
            base(sosiObject, srsName, decimalPlaces, curveFeatures)
        {
            Båndlegging = sosiObject.GetValue("..RPBÅNDLEGGING");
        }

        public string Båndlegging { get; set; }
        public override string FeatureName => "RpBåndleggingSone";

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "båndlegging", Båndlegging));

            return featureMember;
        }
    }
}

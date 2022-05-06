using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpDetaljeringSone")]
    public class RpDetaljeringSone : RpHensynSone
    {
        public RpDetaljeringSone(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpDetaljeringGrense> curveFeatures) : 
            base(sosiObject, srsName, decimalPlaces, curveFeatures)
        {
            Detaljering = sosiObject.GetValue("..RPDETALJERING");
        }

        public string Detaljering { get; set; }
        public override string FeatureName => "RpDetaljeringSone";

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "detaljering", Detaljering));

            return featureMember;
        }
    }
}

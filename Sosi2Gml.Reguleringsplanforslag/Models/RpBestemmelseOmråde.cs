using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.GeometryHelper;
using Sosi2Gml.Application.Models.Features;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpBestemmelseOmråde")]
    public class RpBestemmelseOmråde : SurfaceFeature
    {
        public RpBestemmelseOmråde(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<CurveFeature> curveFeatures) : 
            base(sosiObject, srsName, decimalPlaces, curveFeatures)
        {
            BestemmelseOmrådeNavn = sosiObject.GetValue("..BESTEMMELSEOMRNAVN");
            BetemmelseHjemmel = sosiObject.GetValue("..RPBESTEMMELSEHJEMMEL");
        }

        public override string FeatureName => "RpBestemmelseOmråde";
        public string BestemmelseOmrådeNavn { get; set; }
        public string BetemmelseHjemmel { get; set; }
        public RpOmråde Planområde { get; set; }
        public List<RpPåskrift> Påskrifter { get; set; } = new();

        public override void AddAssociations(List<Feature> features)
        {
            Planområde = GetClosestFeature<RpOmråde>(features, Geometry);

            if (Planområde != null)
                Planområde.BestemmelseOmråder.Add(this);
        }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "område", GeomElement));
            featureMember.Add(new XElement(appNs + "bestemmelseHjemmel", BetemmelseHjemmel));
            featureMember.Add(new XElement(appNs + "bestemmelseOmrådeNavn", BestemmelseOmrådeNavn));

            if (Planområde != null)
                featureMember.Add(CreateXLink(appNs + "planområde", Planområde.GmlId));

            foreach (var påskrift in Påskrifter)
                featureMember.Add(CreateXLink(appNs + "påskrift", påskrift.GmlId));

            return featureMember;
        }
    }
}

using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.GeometryHelper;
using Sosi2Gml.Application.Models.Features;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    public abstract class RpHensynSone : SurfaceFeature
    {
        protected RpHensynSone(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<CurveFeature> curveFeatures) : 
            base(sosiObject, srsName, decimalPlaces, curveFeatures)
        {
            HensynSonenavn = sosiObject.GetValue("..HENSYNSONENAVN");
        }

        public string HensynSonenavn { get; set; }
        public RpOmråde Planområde { get; set; }
        public List<RpPåskrift> Påskrifter { get; set; } = new();

        public override void AddAssociations(List<Feature> features)
        {
            Planområde = GetClosestFeature<RpOmråde>(features, Geometry);

            if (Planområde != null)
                Planområde.Hensyn.Add(this);
        }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "område", GeomElement));
            featureMember.Add(new XElement(appNs + "hensynSonenavn", HensynSonenavn));

            if (Planområde != null)
                featureMember.Add(CreateXLink(appNs + "planområde", Planområde.GmlId));

            foreach (var påskrift in Påskrifter)
                featureMember.Add(CreateXLink(appNs + "påskrift", påskrift.GmlId));

            return featureMember;
        }
    }
}

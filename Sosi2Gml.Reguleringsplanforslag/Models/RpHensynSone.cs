using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

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
            var planområder = features.OfType<RpOmråde>().ToList();

            if (planområder.Count == 1)
                Planområde = planområder.First();
            else
                Planområde = planområder.FirstOrDefault(planområde => planområde.Geometry?.Intersects(Geometry) ?? false);

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

            if (Påskrifter.Any())
                featureMember.Add(Påskrifter.Select(påskrift => CreateXLink(appNs + "påskrift", påskrift.GmlId)));

            return featureMember;
        }
    }
}

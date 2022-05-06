using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpJuridiskLinje")]
    public class RpJuridiskLinje : CurveFeature
    {
        public RpJuridiskLinje(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
            JuridiskLinje = sosiObject.GetValue("..RPJURLINJE");
        }

        public override string FeatureName => "RpJuridiskLinje";
        public string JuridiskLinje { get; set; }
        public RpOmråde Planområde { get; set; }

        public override void AddAssociations(List<Feature> features)
        {
            var planområder = features.OfType<RpOmråde>().ToList();

            if (planområder.Count == 1)
                Planområde = planområder.First();
            else
                Planområde = planområder.FirstOrDefault(planområde => planområde.Geometry?.Intersects(Geometry) ?? false);

            if (Planområde != null)
                Planområde.JuridiskLinje.Add(this);
        }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "senterlinje", GeomElement));

            featureMember.Add(new XElement(appNs + "juridisklinje", JuridiskLinje));

            if (Planområde != null)
                featureMember.Add(CreateXLink(appNs + "planområde", Planområde.GmlId));

            return featureMember;
        }
    }
}

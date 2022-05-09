using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.GeometryHelper;
using Sosi2Gml.Application.Models.Features;

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
        public RpPåskrift Påskrift { get; set; }

        public override void AddAssociations(List<Feature> features)
        {
            Planområde = GetClosestFeature<RpOmråde>(features, Geometry);

            if (Planområde != null)
                Planområde.JuridiskeLinjer.Add(this);
        }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "senterlinje", GeomElement));

            featureMember.Add(new XElement(appNs + "juridisklinje", JuridiskLinje));

            if (Planområde != null)
                featureMember.Add(CreateXLink(appNs + "planområde", Planområde.GmlId));

            if (Påskrift != null)
                featureMember.Add(CreateXLink(appNs + "påskrift", Påskrift.GmlId));

            return featureMember;
        }
    }
}

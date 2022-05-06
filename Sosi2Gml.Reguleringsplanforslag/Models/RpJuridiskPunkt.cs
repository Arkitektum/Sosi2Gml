using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Geometries;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.GmlHelper;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpJuridiskPunkt")]
    public class RpJuridiskPunkt : PointFeature
    {
        public RpJuridiskPunkt(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
            Symbolretning = GetSymbolretning(Points);
            JuridiskPunkt = sosiObject.GetValue("..RPJURPUNKT");
        }

        public override string FeatureName => "RpJuridiskPunkt";        
        public double[] Symbolretning { get; set; }
        public string JuridiskPunkt { get; set; }
        public RpOmråde Planområde { get; set; }

        public override void AddAssociations(List<Feature> features)
        {
            var planområder = features.OfType<RpOmråde>().ToList();

            if (planområder.Count == 1)
                Planområde = planområder.First();
            else
                Planområde = planområder.FirstOrDefault(planområde => planområde.Geometry?.Intersects(Geometry) ?? false);

            if (Planområde != null)
                Planområde.JuridiskPunkt.Add(this);
        }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "posisjon", CreatePoint(Points.First(), $"{GmlId}-0", SrsName)));

            featureMember.Add(new XElement(appNs + "symbolretning",
                new XAttribute("srsDimension", 2),
                new XAttribute("srsName", SrsName),
                FormattableString.Invariant($"{Symbolretning[0]} {Symbolretning[1]}")
            ));

            featureMember.Add(new XElement(appNs + "juridiskpunkt", JuridiskPunkt));

            if (Planområde != null)
                featureMember.Add(CreateXLink(appNs + "planområde", Planområde.GmlId));

            return featureMember;
        }

        private static double[] GetSymbolretning(List<Point> points)
        {
            var firstPoint = points.First();
            var lastPoint = points.Last();

            return new[] { lastPoint.X - firstPoint.X, lastPoint.Y - firstPoint.Y };
        }
    }
}

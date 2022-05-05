using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Geometries;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

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

        public double[] Symbolretning { get; set; }
        public string JuridiskPunkt { get; set; }
        public RpOmråde Planområde { get; set; }

        public override string FeatureName => "RpJuridiskPunkt";

        public override XElement ToGml()
        {
            var featureMember = new XElement(AppNs + FeatureName, new XAttribute(GmlNs + "id", GmlId));

            featureMember.Add(Identifikasjon.ToGml(AppNs));

            if (FørsteDigitaliseringsdato.HasValue)
                featureMember.Add(new XElement(AppNs + "førsteDigitaliseringsdato", FormatDateTime(FørsteDigitaliseringsdato.Value)));

            featureMember.Add(new XElement(AppNs + "oppdateringsdato", FormatDateTime(Oppdateringsdato)));

            if (Kvalitet != null)
                featureMember.Add(Kvalitet.ToGml(AppNs));

            featureMember.Add(new XElement(AppNs + "posisjon", CreatePoint(Points.First(), GmlId, SrsName)));

            featureMember.Add(new XElement(AppNs + "symbolretning",
                new XAttribute("srsDimension", 2),
                new XAttribute("srsName", SrsName),
                FormattableString.Invariant($"{Symbolretning[0]} {Symbolretning[1]}")
            ));

            featureMember.Add(new XElement(AppNs + "juridiskpunkt", JuridiskPunkt));

            if (Planområde != null)
                featureMember.Add(CreateXLink(AppNs + "planområde", Planområde.GmlId));

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

using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    public class RpJuridiskPunkt : PointFeature
    {
        public RpJuridiskPunkt(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
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

            featureMember.Add(new XElement(AppNs + "symboldimensjon",
                new XElement(AppNs + "SymbolDimensjon",
                    new XElement(AppNs + "symbolhøyde", "0.00"),
                    new XElement(AppNs + "symbolbredde", "0.00")
                )
            ));

            featureMember.Add(new XElement(AppNs + "symbolretning",
                new XAttribute("srsDimension", 2),
                new XAttribute("srsName", SrsName),
                FormattableString.Invariant($"{Symbolretning[0]} {Symbolretning[1]}")
            ));

            featureMember.Add(new XElement(AppNs + "juridiskpunkt", JuridiskPunkt));

            featureMember.Add(CreateXLink(AppNs + "planområde", Planområde.GmlId));

            return featureMember;
        }
    }
}

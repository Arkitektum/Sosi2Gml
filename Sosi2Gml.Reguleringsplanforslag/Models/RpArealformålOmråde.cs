using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpArealformålOmråde")]
    public class RpArealformålOmråde : SurfaceFeature
    {
        public RpArealformålOmråde(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpFormålGrense> rpFormålGrenser) : 
            base(sosiObject, srsName, decimalPlaces, rpFormålGrenser)
        {
            Arealformål = sosiObject.GetValue("..RPAREALFORMÅL");
            Feltnavn = sosiObject.GetValue("..FELTNAVN");
            Beskrivelse = sosiObject.GetValue("..BESKRIVELSE");
            Eierform = sosiObject.GetValue("..EIERFORM");
            Uteoppholdsareal = sosiObject.GetValue("..UTEAREAL");
            Avkjørselsbestemmelse = sosiObject.GetValue("..AVKJ");
            Byggverkbestemmelse = sosiObject.GetValue("..BYGGVERK");

            if (sosiObject.HasValue("..UTNYTT"))
            {
                Utnyttinger.Add(new Utnytting
                {
                    Utnyttingstype = sosiObject.GetValue("...UTNTYP"),
                    Utnyttingstall = sosiObject.GetValue("...UTNTALL"),
                    UtnyttingstallMinimum = sosiObject.GetValue("...UTNTALL_MIN"),
                });
            }

            AvgrensesAv.AddRange(Surface.GetFeatures<RpFormålGrense>());
        }

        public string Arealformål { get; set; }
        public string Feltnavn { get; set; }
        public string Beskrivelse { get; set; }
        public string Eierform { get; set; }
        public List<Utnytting> Utnyttinger { get; set; } = new();
        public string Uteoppholdsareal { get; set; }
        public string Avkjørselsbestemmelse { get; set; }
        public string Byggverkbestemmelse { get; set; }
        public List<RpFormålGrense> AvgrensesAv { get; set; } = new();
        public RpOmråde Planområde { get; set; }
        public List<RpPåskrift> Påskrifter { get; set; } = new();

        public override string FeatureName => "RpArealformålOmråde";

        public override XElement ToGml()
        {
            var featureMember = new XElement(AppNs + FeatureName, new XAttribute(GmlNs + "id", GmlId));

            featureMember.Add(Identifikasjon.ToGml(AppNs));

            if (FørsteDigitaliseringsdato.HasValue)
                featureMember.Add(new XElement(AppNs + "førsteDigitaliseringsdato", FormatDateTime(FørsteDigitaliseringsdato.Value)));

            featureMember.Add(new XElement(AppNs + "oppdateringsdato", FormatDateTime(Oppdateringsdato)));

            if (Kvalitet != null)
                featureMember.Add(Kvalitet.ToGml(AppNs));

            featureMember.Add(new XElement(AppNs + "område", GeomElement));
            featureMember.Add(new XElement(AppNs + "arealformål", Arealformål));
            featureMember.Add(new XElement(AppNs + "feltnavn", Feltnavn));
            featureMember.Add(new XElement(AppNs + "eierform", Eierform));

            if (Utnyttinger.Any())
                featureMember.Add(new XElement(AppNs + "utnytting", Utnyttinger.Select(utnytting => utnytting.ToGml(AppNs))));

            if (!string.IsNullOrWhiteSpace(Uteoppholdsareal))
                featureMember.Add(new XElement(AppNs + "uteoppholdsareal", Uteoppholdsareal));

            if (!string.IsNullOrWhiteSpace(Avkjørselsbestemmelse))
                featureMember.Add(new XElement(AppNs + "avkjørselsbestemmelse", Avkjørselsbestemmelse));

            if (!string.IsNullOrWhiteSpace(Byggverkbestemmelse))
                featureMember.Add(new XElement(AppNs + "byggverkbestemmelse", Byggverkbestemmelse));

            if (!string.IsNullOrWhiteSpace(Uteoppholdsareal))
                featureMember.Add(new XElement(AppNs + "Uteoppholdsareal", Uteoppholdsareal));

            if (AvgrensesAv.Any())
                featureMember.Add(AvgrensesAv.Select(formålGrense => CreateXLink(AppNs + "avgrensesAv", formålGrense.GmlId)));

            //featureMember.Add(CreateXLink(AppNs + "planområde", Planområde.GmlId));

            /*if (AvgrensesAv.Any())
                featureMember.Add(Påskrifter.Select(påskrift => CreateXLink(AppNs + "påskrift", påskrift.)));*/

            return featureMember;
        }
    }
}

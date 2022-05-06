using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.GmlHelper;

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
                    UtnyttingstallMinimum = sosiObject.GetValue("...UTNTALL_MIN")
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

        public override void AddAssociations(List<Feature> features)
        {
            var planområder = features.OfType<RpOmråde>().ToList();

            if (planområder.Count == 1)
                Planområde = planområder.First();
            else
                Planområde = planområder.FirstOrDefault(planområde => planområde.Geometry?.Intersects(Geometry) ?? false);

            if (Planområde != null)
                Planområde.Formål.Add(this);
        }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "område", GeomElement));
            featureMember.Add(new XElement(appNs + "arealformål", Arealformål));
            featureMember.Add(new XElement(appNs + "feltnavn", Feltnavn));
            featureMember.Add(new XElement(appNs + "eierform", Eierform));

            if (Utnyttinger.Any())
                featureMember.Add(new XElement(appNs + "utnytting", Utnyttinger.Select(utnytting => utnytting.ToGml(appNs))));

            if (!string.IsNullOrWhiteSpace(Uteoppholdsareal))
                featureMember.Add(new XElement(appNs + "uteoppholdsareal", Uteoppholdsareal));

            if (!string.IsNullOrWhiteSpace(Avkjørselsbestemmelse))
                featureMember.Add(new XElement(appNs + "avkjørselsbestemmelse", Avkjørselsbestemmelse));

            if (!string.IsNullOrWhiteSpace(Byggverkbestemmelse))
                featureMember.Add(new XElement(appNs + "byggverkbestemmelse", Byggverkbestemmelse));

            if (!string.IsNullOrWhiteSpace(Uteoppholdsareal))
                featureMember.Add(new XElement(appNs + "Uteoppholdsareal", Uteoppholdsareal));

            if (AvgrensesAv.Any())
                featureMember.Add(AvgrensesAv.Select(formålGrense => CreateXLink(appNs + "avgrensesAv", formålGrense.GmlId)));
            
            if (Planområde != null)
                featureMember.Add(CreateXLink(appNs + "planområde", Planområde.GmlId));

            if (Påskrifter.Any())
                featureMember.Add(Påskrifter.Select(påskrift => CreateXLink(appNs + "påskrift", påskrift.GmlId)));

            return featureMember;
        }
    }
}

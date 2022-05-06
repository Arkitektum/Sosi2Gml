using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    public class Arealplan : Feature
    {
        public Arealplan(SosiObject sosiObject) : base(sosiObject)
        {
            if (sosiObject.HasValue("..IDENT"))
                Identifikasjon = new Identifikasjon(sosiObject);

            Oppdateringsdato = SosiDateToDateTime(sosiObject.GetValue("..OPPDATERINGSDATO")).Value;
            Kommunenummer = sosiObject.GetValue("...KOMM");
            Planidentifikasjon = sosiObject.GetValue("...PLANID");
            Plantype = sosiObject.GetValue("..PLANTYPE");
            Plannavn = sosiObject.GetValue("..PLANNAVN");
            Planstatus = sosiObject.GetValue("..PLANSTAT");
            OmPlanbestemmelser = sosiObject.GetValue("..PLANBEST");
            Lovreferanse = sosiObject.GetValue("..LOVREFERANSE");
        }

        public Identifikasjon Identifikasjon { get; set; }
        public DateTime Oppdateringsdato { get; set; }
        public string Kommunenummer { get; set; }
        public string Planidentifikasjon { get; set; }
        public string Plantype { get; set; }
        public string Plannavn { get; set; }
        public string Planstatus { get; set; }
        public string OmPlanbestemmelser { get; set; }
        public string Lovreferanse { get; set; }
        public List<RpOmråde> RpOmråder { get; set; } = new();

        public override string FeatureName => "Arealplan";

        public override void AddAssociations(List<Feature> features)
        {
        }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = new XElement(appNs + FeatureName, new XAttribute(GmlNs + "id", GmlId));

            featureMember.Add(Identifikasjon.ToGml(appNs));
            featureMember.Add(new XElement(appNs + "oppdateringsdato", FormatDateTime(Oppdateringsdato)));

            featureMember.Add(new XElement(appNs + "nasjonalArealplanId",
                new XElement(appNs + "NasjonalArealplanId",
                    new XElement(appNs + "administrativEnhet",
                        new XElement(appNs + "AdministrativEnhetskode",
                            new XElement(appNs + "kommunenummer", Kommunenummer)
                        )
                    ),
                    new XElement(appNs + "planidentifikasjon", Planidentifikasjon)
                )
            ));

            featureMember.Add(new XElement(appNs + "plantype", Plantype));
            featureMember.Add(new XElement(appNs + "plannavn", Plannavn));
            featureMember.Add(new XElement(appNs + "planstatus", Planstatus));
            featureMember.Add(new XElement(appNs + "omPlanbestemmelser", OmPlanbestemmelser));
            featureMember.Add(new XElement(appNs + "lovreferanse", Lovreferanse));

            foreach (var rpOmråde in RpOmråder)
                featureMember.Add(CreateXLink(appNs + "rpOmråde", rpOmråde.GmlId));

            return featureMember;
        }
    }
}

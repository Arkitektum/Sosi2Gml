using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

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

        public override XElement ToGml()
        {
            var featureMember = new XElement(AppNs + FeatureName, new XAttribute(GmlNs + "id", GmlId));

            featureMember.Add(Identifikasjon.ToGml(AppNs));
            featureMember.Add(new XElement(AppNs + "oppdateringsdato", FormatDateTime(Oppdateringsdato)));

            featureMember.Add(new XElement(AppNs + "nasjonalArealplanId",
                new XElement(AppNs + "NasjonalArealplanId",
                    new XElement(AppNs + "administrativEnhet",
                        new XElement(AppNs + "AdministrativEnhetskode",
                            new XElement(AppNs + "kommunenummer", Kommunenummer)
                        )
                    ),
                    new XElement(AppNs + "planidentifikasjon", Planidentifikasjon)
                )
            ));

            featureMember.Add(new XElement(AppNs + "plantype", Plantype));
            featureMember.Add(new XElement(AppNs + "plannavn", Plannavn));
            featureMember.Add(new XElement(AppNs + "planstatus", Planstatus));
            featureMember.Add(new XElement(AppNs + "omPlanbestemmelser", OmPlanbestemmelser));
            featureMember.Add(new XElement(AppNs + "lovreferanse", Lovreferanse));

            foreach (var rpOmråde in RpOmråder)
                featureMember.Add(CreateXLink(AppNs + "rpOmråde", rpOmråde.GmlId));

            return featureMember;
        }
    }
}

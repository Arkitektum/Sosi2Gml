using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpRegulertHøyde")]
    public class RpRegulertHøyde : CurveFeature
    {
        public RpRegulertHøyde(SosiObject sosiObject, string srsName, int decimalPlaces) : 
            base(sosiObject, srsName, decimalPlaces)
        {
            RegulertHøyde = sosiObject.GetValue("...REGULERTHØYDE");
            Høydereferansesystem = sosiObject.GetValue("...HØYDE-REF") ?? "NN2000";
        }

        public string RegulertHøyde { get; set; }
        public string Høydereferansesystem { get; set; }
        public string TypeHøyde { get; } = "GH";
        public RpOmråde Planområde { get; set; }

        public override string FeatureName => "RpRegulertHøyde";

        public override XElement ToGml()
        {
            var featureMember = new XElement(AppNs + FeatureName, new XAttribute(GmlNs + "id", GmlId));

            featureMember.Add(Identifikasjon.ToGml(AppNs));

            if (FørsteDigitaliseringsdato.HasValue)
                featureMember.Add(new XElement(AppNs + "førsteDigitaliseringsdato", FormatDateTime(FørsteDigitaliseringsdato.Value)));

            featureMember.Add(new XElement(AppNs + "oppdateringsdato", FormatDateTime(Oppdateringsdato)));

            if (Kvalitet != null)
                featureMember.Add(Kvalitet.ToGml(AppNs));
            
            featureMember.Add(new XElement(AppNs + "senterlinje", GeomElement));

            featureMember.Add(new XElement(AppNs + "høydeFraPlanbestemmelse",
                new XElement(AppNs + "HøydeFraPlanbestemmelse",
                    new XElement(AppNs + "regulerthøyde", RegulertHøyde),
                    new XElement(AppNs + "vertikalReferanse",
                        new XElement(AppNs + "VertikalReferanse",
                            new XElement(AppNs + "høydereferansesystem", Høydereferansesystem)
                        )
                    )
                )
            ));

            featureMember.Add(new XElement(AppNs + "typeHøyde", TypeHøyde));

            featureMember.Add(new XComment("Pga. manglende kompabilitet mellom SOSI 4.5 og SOSI 5.0, settes \"typeHøyde\" til \"GH\" (gesimshøyde). Tilgjengelige valg er: GH, MH, TH, BH og PH."));

            if (Planområde != null)
                featureMember.Add(CreateXLink(AppNs + "planområde", Planområde.GmlId));

            return featureMember;
        }
    }
}

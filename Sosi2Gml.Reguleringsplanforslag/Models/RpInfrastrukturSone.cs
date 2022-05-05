using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpInfrastrukturSone")]
    public class RpInfrastrukturSone : RpHensynSone
    {
        public RpInfrastrukturSone(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpInfrastrukturGrense> curveFeature) : 
            base(sosiObject, srsName, decimalPlaces, curveFeature)
        {
            Infrastruktur = sosiObject.GetValue("..RPINFRASTRUKTUR");
        }

        public string Infrastruktur { get; set; }
        public override string FeatureName => "RpInfrastrukturSone";

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
            featureMember.Add(new XElement(AppNs + "hensynSonenavn", HensynSonenavn));

            //featureMember.Add(CreateXLink(AppNs + "planområde", Planområde.GmlId));

            /*if (Påskrifter.Any())
                featureMember.Add(Påskrifter.Select(påskrift => CreateXLink(AppNs + "påskrift", påskrift.GmlId)));*/

            featureMember.Add(new XElement(AppNs + "infrastruktur", Infrastruktur));

            return featureMember;
        }
    }
}

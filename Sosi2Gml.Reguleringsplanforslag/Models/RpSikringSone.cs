using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    public class RpSikringSone : RpHensynSone
    {
        public RpSikringSone(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpSikringGrense> rpSikringGrenser) : 
            base(sosiObject, srsName, decimalPlaces, rpSikringGrenser)
        {
        }

        public string Sikring { get; set; }
        public override string FeatureName => "RpSikringSone";

        public override XElement ToGml()
        {
            var featureMember = new XElement(AppNs + FeatureName, new XAttribute(GmlNs + "id", GmlId));

            featureMember.Add(Identifikasjon.ToGml(AppNs));

            if (FørsteDigitaliseringsdato.HasValue)
                featureMember.Add(new XElement(AppNs + "førsteDigitaliseringsdato", FormatDateTime(FørsteDigitaliseringsdato.Value)));

            featureMember.Add(new XElement(AppNs + "oppdateringsdato", FormatDateTime(Oppdateringsdato)));

            if (Kvalitet != null)
                featureMember.Add(Kvalitet.ToGml(AppNs));

            featureMember.Add(new XElement(AppNs + "område", GetGeomElement()));
            featureMember.Add(new XElement(AppNs + "hensynSonenavn", HensynSonenavn));

            //featureMember.Add(CreateXLink(AppNs + "planområde", Planområde.GmlId));

            /*if (Påskrifter.Any())
                featureMember.Add(Påskrifter.Select(påskrift => CreateXLink(AppNs + "påskrift", påskrift.GmlId)));*/

            featureMember.Add(new XElement(AppNs + "sikring", Sikring));

            return featureMember;
        }
    }
}

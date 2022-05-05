using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpStøySone")]
    public class RpStøySone : RpHensynSone
    {
        public RpStøySone(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpStøyGrense> curveFeatures) : 
            base(sosiObject, srsName, decimalPlaces, curveFeatures)
        {
            Støy = sosiObject.GetValue("..RPSTØY");
        }

        public string Støy { get; set; }
        public override string FeatureName => "RpStøySone";

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

            featureMember.Add(new XElement(AppNs + "støy", Støy));

            return featureMember;
        }
    }
}

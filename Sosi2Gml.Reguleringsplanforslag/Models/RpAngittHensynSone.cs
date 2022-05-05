using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpAngittHensynSone")]
    public class RpAngittHensynSone : RpHensynSone
    {
        public RpAngittHensynSone(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpAngittHensynGrense> curveFeatures) : 
            base(sosiObject, srsName, decimalPlaces, curveFeatures)
        {
            AngittHensyn = sosiObject.GetValue("..RPANGITTHENSYN");
        }

        public string AngittHensyn { get; set; }
        public override string FeatureName => "RpAngittHensynSone";

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

            featureMember.Add(new XElement(AppNs + "angittHensyn", AngittHensyn));

            return featureMember;
        }
    }
}

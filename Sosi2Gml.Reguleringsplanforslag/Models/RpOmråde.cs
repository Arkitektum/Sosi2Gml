using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    public class RpOmråde : SurfaceFeature
    {
        public RpOmråde(
            SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpGrense> rpGrenser) : base(sosiObject, srsName, decimalPlaces, rpGrenser)
        {
            Vertikalnivå = sosiObject.GetValue("..VERTNIV");
        }

        public string Vertikalnivå { get; set; }
        public Arealplan Arealplan { get; set; }
        public List<RpPåskrift> Påskrifter { get; set; } = new();

        public override string FeatureName => "RpOmråde";

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

            featureMember.Add(new XElement(AppNs + "vertikalnivå", Vertikalnivå));

            return featureMember;
        }
    }
}

using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using Sosi2Gml.Application.Helpers;
using Sosi2Gml.Application.Attributes;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpGrense")]
    public class RpGrense : CurveFeature
    {
        public RpGrense(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
        }

        public override string FeatureName => "RpGrense";

        public override XElement ToGml()
        {
            var featureMember = new XElement(AppNs + FeatureName, new XAttribute(GmlNs + "id", GmlId));

            featureMember.Add(Identifikasjon.ToGml(AppNs));

            if (FørsteDigitaliseringsdato.HasValue)
                featureMember.Add(new XElement(AppNs + "førsteDigitaliseringsdato", FormatDateTime(FørsteDigitaliseringsdato.Value)));

            featureMember.Add(new XElement(AppNs + "oppdateringsdato", FormatDateTime(Oppdateringsdato)));

            if (Kvalitet != null)
                featureMember.Add(Kvalitet.ToGml(AppNs));

            var curveSegment = CartographicElementType == CartographicElementType.Kurve ? GmlHelper.CreateLineStringSegment(Points) : GmlHelper.CreateArc(Points);
            var curve = GmlHelper.CreateCurve(new[] { curveSegment }, $"{GmlId}-0", SrsName);

            featureMember.Add(new XElement(AppNs + "grense", curve));

            return featureMember;
        }
    }
}

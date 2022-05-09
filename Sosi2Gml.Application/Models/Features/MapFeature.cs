using OSGeo.OGR;
using Sosi2Gml.Application.Helpers;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.SosiHelper;

namespace Sosi2Gml.Application.Models.Features
{
    public abstract class MapFeature : Feature
    {
        protected MapFeature(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject)
        {
            if (sosiObject.HasValue("..IDENT"))
                Identifikasjon = new Identifikasjon(sosiObject);

            if (sosiObject.HasValue("..KVALITET"))
                Kvalitet = new Kvalitet(sosiObject);

            FørsteDigitaliseringsdato = SosiDateToDateTime(sosiObject.GetValue("..FØRSTEDIGITALISERINGSDATO"));
            Oppdateringsdato = SosiDateToDateTime(sosiObject.GetValue("..OPPDATERINGSDATO")).GetValueOrDefault();
            CartographicElementType = ParseCartographicElementType(sosiObject.ElementType);
            SequenceNumber = sosiObject.SequenceNumber;
            SrsName = srsName;
            DecimalPlaces = decimalPlaces;
        }

        public Identifikasjon Identifikasjon { get; set; }
        public Kvalitet Kvalitet { get; set; }
        public DateTime? FørsteDigitaliseringsdato { get; set; }
        public DateTime Oppdateringsdato { get; set; }
        public int SequenceNumber { get; private set; }
        public string SrsName { get; private set; }
        public int DecimalPlaces { get; private set; }
        public CartographicElementType CartographicElementType { get; private set; }
        public Geometry Geometry => GeometryHelper.GeometryFromGml(GeomElement);
        public virtual XElement GeomElement => null;

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = new XElement(appNs + FeatureName, new XAttribute(GmlNs + "id", GmlId));

            featureMember.Add(Identifikasjon.ToGml(appNs));

            if (FørsteDigitaliseringsdato.HasValue)
                featureMember.Add(new XElement(appNs + "førsteDigitaliseringsdato", FormatDateTime(FørsteDigitaliseringsdato.Value)));

            featureMember.Add(new XElement(appNs + "oppdateringsdato", FormatDateTime(Oppdateringsdato)));

            if (Kvalitet != null)
                featureMember.Add(Kvalitet.ToGml(appNs));

            return featureMember;
        }
    }
}

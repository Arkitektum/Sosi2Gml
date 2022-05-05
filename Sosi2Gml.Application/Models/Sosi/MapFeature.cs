using OSGeo.OGR;
using Sosi2Gml.Application.Helpers;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Application.Models.Sosi
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
            Oppdateringsdato = SosiDateToDateTime(sosiObject.GetValue("..OPPDATERINGSDATO")).Value;
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
    }
}

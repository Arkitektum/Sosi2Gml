using OSGeo.OGR;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Application.Models.Sosi
{
    public abstract class GeometryFeature : Kartobjekt
    {
        protected GeometryFeature(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject)
        {
            SequenceNumber = sosiObject.SequenceNumber;
            SrsName = srsName;
            DecimalPlaces = decimalPlaces;
            CartographicElementType = ParseCartographicElementType(sosiObject.ElementType);
        }

        public int SequenceNumber { get; private set; }
        public Geometry Geometry { get; set; }
        public string SrsName { get; private set; }
        public int DecimalPlaces { get; private set; }
        public CartographicElementType CartographicElementType { get; private set; }
    }
}

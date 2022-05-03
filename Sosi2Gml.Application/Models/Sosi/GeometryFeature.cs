using OSGeo.OGR;
using Sosi2Gml.Application.Helpers;
using System.Xml.Linq;
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
        public string SrsName { get; private set; }
        public int DecimalPlaces { get; private set; }
        public CartographicElementType CartographicElementType { get; private set; }

        public Geometry GetGeometry()
        {
            var geomElement = GetGeomElement();

            return GeometryHelper.GeometryFromGml(geomElement);
        }

        public virtual XElement GetGeomElement()
        {
            throw new NotImplementedException();
        }
    }
}

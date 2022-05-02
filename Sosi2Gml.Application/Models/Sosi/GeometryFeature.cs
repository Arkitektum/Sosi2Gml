using OSGeo.OGR;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Application.Models.Sosi
{
    public abstract class GeometryFeature : Kartobjekt, IDisposable
    {
        private bool _disposed;

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

        public virtual XElement GetGeomElement()
        {
            throw new NotImplementedException();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing && Geometry != null)
                {
                    Geometry.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

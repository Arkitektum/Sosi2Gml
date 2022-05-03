using OSGeo.OGR;
using System.Xml.Linq;

namespace Sosi2Gml.Application.Helpers
{
    public class GeometryHelper
    {
        public static Geometry GeometryFromGml(XElement geomElement)
        {
            try
            {
                return Geometry.CreateFromGML(geomElement.ToString());
            }
            catch
            {
                return null;
            }
        }
    }
}

using Sosi2Gml.Application.Constants;
using Sosi2Gml.Application.Models.Geometries;
using System.Xml.Linq;

namespace Sosi2Gml.Helpers
{
    public class GmlHelper
    {
        public static XElement CreatePoint(Point point, string gmlId, string srsName)
        {
            return new XElement(Namespace.GmlNs + "Point",
                new XAttribute(Namespace.GmlNs + "id", gmlId),
                new XAttribute("srsDimension", 2),
                new XAttribute("srsName", srsName),
                new XElement(Namespace.GmlNs + "pos",
                    point.ToString()
                )
            );
        }

        public static XElement CreateLineString(IEnumerable<Point> points, string gmlId, string srsName)
        {
            return new XElement(Namespace.GmlNs + "LineString",
                new XAttribute(Namespace.GmlNs + "id", gmlId),
                new XAttribute("srsDimension", 2),
                new XAttribute("srsName", srsName),
                new XElement(Namespace.GmlNs + "posList",
                    string.Join(" ", points.Select(point => point.ToString()))
                )
            );
        }

        public static XElement CreateArc(IEnumerable<Point> points)
        {
            return new XElement(Namespace.GmlNs + "Arc",
                new XElement(Namespace.GmlNs + "posList",
                    new XAttribute("srsDimension", 2),
                    string.Join(" ", points.Select(point => point.ToString()))
                )
            );
        }

        public static XElement CreateLineStringSegment(IEnumerable<Point> points)
        {
            return new XElement(Namespace.GmlNs + "LineStringSegment",
                new XElement(Namespace.GmlNs + "posList",
                    new XAttribute("srsDimension", 2),
                    string.Join(" ", points.Select(point => point.ToString()))
                )
            );
        }
    }
}

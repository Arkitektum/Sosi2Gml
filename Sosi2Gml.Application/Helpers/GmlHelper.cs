using Sosi2Gml.Application.Constants;
using Sosi2Gml.Application.Models.Geometries;
using System.Xml.Linq;

namespace Sosi2Gml.Application.Helpers
{
    public class GmlHelper
    {
        public static XElement CreateXLink(XName name, string gmlId)
        {
            return new XElement(name,
                new XAttribute(Namespace.XLinkNs + "type", "simple"),
                new XAttribute(Namespace.XLinkNs + "href", $"#{gmlId}")
            );
        }

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

        public static XElement CreateCurve(IEnumerable<XElement> segments, string gmlId, string srsName)
        {
            return new XElement(Namespace.GmlNs + "Curve",
                new XAttribute(Namespace.GmlNs + "id", gmlId),
                new XAttribute("srsDimension", 2),
                new XAttribute("srsName", srsName),
                new XElement(Namespace.GmlNs + "segments",
                    segments
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

        public static XElement CreateMultiSurface(IEnumerable<XElement> exterior, IEnumerable<IEnumerable<XElement>> interiors, string gmlId, string srsName)
        {
            var multiSurface = new XElement(Namespace.GmlNs + "MultiSurface",
                new XAttribute(Namespace.GmlNs + "id", $"{gmlId}-0"),
                new XAttribute("srsName", srsName),
                new XAttribute("srsDimension", 2)
            );

            var polygon = new XElement(Namespace.GmlNs + "Polygon",
                new XAttribute(Namespace.GmlNs + "id", $"{gmlId}-1"),
                new XElement(Namespace.GmlNs + "exterior",
                    new XElement(Namespace.GmlNs + "Ring",
                        new XElement(Namespace.GmlNs + "curveMember",
                            CreateCurve(exterior, $"{gmlId}-2", srsName)
                        )
                    )
                )
            );

            if (interiors.Any())
            {
                polygon.Add(interiors.Select((interior, index) =>
                {
                    return new XElement(Namespace.GmlNs + "interior",
                        new XElement(Namespace.GmlNs + "Ring",
                            new XElement(Namespace.GmlNs + "curveMember",
                                CreateCurve(interior, $"{gmlId}-{index + 3}", srsName)
                            )
                        )
                    );
                }));
            }

            multiSurface.Add(new XElement(Namespace.GmlNs + "surfaceMember", polygon));

            return multiSurface;
        }
    }
}

using Sosi2Gml.Application.Models.Geometries;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Application.Models.Sosi
{
    public abstract class PointFeature : GeometryFeature
    {
        protected PointFeature(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
            Points = GetPoints(SosiValues, decimalPlaces);
        }

        public override XElement GetGeomElement()
        {
            /*GmlHelper.CreatePoint()
            var curveSegment = CartographicElementType == CartographicElementType.Kurve ? GmlHelper.CreateLineStringSegment(Points) : GmlHelper.CreateArc(Points);
            var curve = GmlHelper.CreateCurve(new[] { curveSegment }, $"{GmlId}-0", SrsName);

            return curve;*/
            return null;
        }

        public List<Point> Points { get; private set; }
    }
}

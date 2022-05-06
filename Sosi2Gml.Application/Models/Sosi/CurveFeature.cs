using Sosi2Gml.Application.Helpers;
using Sosi2Gml.Application.Models.Geometries;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Application.Models.Sosi
{
    public abstract class CurveFeature : MapFeature
    {
        protected CurveFeature(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
            Points = GetPoints(SosiValues, decimalPlaces);
        }

        public List<Point> Points { get; private set; }

        public override XElement GeomElement
        {
            get
            {
                var curveSegment = CartographicElementType == CartographicElementType.Kurve ? GmlHelper.CreateLineStringSegment(Points) : GmlHelper.CreateArc(Points);
                var curve = GmlHelper.CreateCurve(new[] { curveSegment }, $"{GmlId}-0", SrsName);

                return curve;
            }
        }

        public override XElement ToGml(XNamespace appNs) => base.ToGml(appNs);
    }
}

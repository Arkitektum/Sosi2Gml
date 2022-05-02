using Sosi2Gml.Application.Constants;
using Sosi2Gml.Application.Helpers;
using Sosi2Gml.Application.Models.Geometries;
using System.Xml.Linq;

namespace Sosi2Gml.Application.Models.Sosi
{
    public abstract class CurveFeature : GeometryFeature
    {
        protected CurveFeature(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
            SetPoints();
        }

        public override XElement GetGeomElement()
        {
            var curveSegment = CartographicElementType == CartographicElementType.Kurve ? GmlHelper.CreateLineStringSegment(Points) : GmlHelper.CreateArc(Points);
            var curve = GmlHelper.CreateCurve(new[] { curveSegment }, $"{GmlId}-0", SrsName);

            return curve;
        }

        public List<Point> Points { get; private set; }

        private void SetPoints()
        {
            for (int i = 0; i < SosiValues.Lines.Count; i++)
            {
                var value = SosiValues.Lines[i];

                if (Regexes.PointsStartRegex.IsMatch(value))
                {
                    var pointValues = SosiValues.Lines.Skip(i);
                    var points = new List<Point>();

                    foreach (var pointValue in pointValues)
                    {
                        var pointMatch = Regexes.PointRegex.Match(pointValue);

                        if (pointMatch.Success)
                            points.Add(Point.Create(pointMatch.Groups["x"].Value, pointMatch.Groups["y"].Value, DecimalPlaces));
                    }

                    Points = points;
                    break;
                }
            }
        }
    }
}

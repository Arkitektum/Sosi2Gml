using Sosi2Gml.Application.Constants;
using Sosi2Gml.Application.Models.Geometries;

namespace Sosi2Gml.Application.Models.Sosi
{
    public abstract class CurveFeature : GeometryFeature
    {
        protected CurveFeature(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
            SetPoints();
        }

        public List<Point> Points { get; private set; }

        private void SetPoints()
        {
            for (int i = 0; i < SosiValues.Values.Count; i++)
            {
                var value = SosiValues.Values[i];

                if (Regexes.PointsStartRegex.IsMatch(value))
                {
                    var pointValues = SosiValues.Values.Skip(i);
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

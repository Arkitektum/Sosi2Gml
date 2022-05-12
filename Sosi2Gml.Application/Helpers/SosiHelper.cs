using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Features;
using Sosi2Gml.Application.Models.Geometries;
using Sosi2Gml.Application.Models.Sosi;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Sosi2Gml.Application.Helpers
{
    public class SosiHelper
    {
        private static readonly Regex _curveReferencesRegex = new(@"\.\.REF(?<refs>(.*))\.\.NØ", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex _surfaceRingRegex = new(@"(?<exterior>.*?)\((?<interiors>.*?)\)", RegexOptions.Compiled);
        private static readonly Regex _pointsStartRegex = new(@"^\.\.NØ", RegexOptions.Compiled);
        private static readonly Regex _pointRegex = new(@"^(?<y>\d+) (?<x>\d+)", RegexOptions.Compiled);

        public static string GetSosiObjectName<T>() where T : Feature
        {
            var type = typeof(T);
            var sosiObjectNameAttribute = type.GetCustomAttribute<SosiObjectNameAttribute>();

            if (sosiObjectNameAttribute != null)
                return sosiObjectNameAttribute.SosiObjectName;

            return type.Name;
        }

        public static DateTime? SosiDateToDateTime(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            return DateTime.TryParseExact(dateString, "yyyyMMdd", null, DateTimeStyles.None, out var dateTime) ? dateTime : null;
        }

        public static string FormatDateTime(DateTime dateTime, string format = "yyyy-MM-ddTHH:mm:ss.fff")
        {
            return dateTime.ToString(format);
        }

        public static CartographicElementType ParseCartographicElementType(string elementType)
        {
            return elementType switch
            {
                "PUNKT" => CartographicElementType.Punkt,
                "KURVE" => CartographicElementType.Kurve,
                "BUEP" => CartographicElementType.BueP,
                "FLATE" => CartographicElementType.Flate,
                "TEKST" => CartographicElementType.Tekst,
                "SYMBOL" => CartographicElementType.Symbol,
                _ => CartographicElementType.Unknown,
            };
        }

        public static List<Point> GetPoints(SosiValues sosiValues, int decimalPlaces)
        {
            var points = new List<Point>();

            for (int i = 0; i < sosiValues.Lines.Count; i++)
            {
                var value = sosiValues.Lines[i];

                if (_pointsStartRegex.IsMatch(value))
                {
                    var pointValues = sosiValues.Lines.Skip(i);

                    foreach (var pointValue in pointValues)
                    {
                        var pointMatch = _pointRegex.Match(pointValue);

                        if (pointMatch.Success)
                            points.Add(Point.Create(pointMatch.Groups["x"].Value, pointMatch.Groups["y"].Value, decimalPlaces));
                    }

                    break;
                }
            }

            return points;
        }

        public static ReferenceSurface CreateReferenceSurface(IEnumerable<CurveFeature> curveFeatures, List<string> sosiValueLines)
        {
            var joined = string.Join(Environment.NewLine, sosiValueLines);
            var match = _curveReferencesRegex.Match(joined);

            if (!match.Success)
                return null;

            var refLine = match.Groups["refs"].Value;
            var ringMatches = _surfaceRingRegex.Matches(refLine);
            var surface = new ReferenceSurface();

            if (!ringMatches.Any())
            {
                surface.Exterior.AddRange(CreateReferenceSurfaceRing(curveFeatures, refLine));
                return surface;
            }

            var exterior = ringMatches
                .Select(match => match.Groups["exterior"].Value)
                .FirstOrDefault(refLine => !string.IsNullOrWhiteSpace(refLine));

            surface.Exterior.AddRange(CreateReferenceSurfaceRing(curveFeatures, exterior));

            var interiors = ringMatches
                .Select(match => match.Groups["interiors"].Value)
                .Where(refLine => !string.IsNullOrWhiteSpace(refLine));

            foreach (var interiorRefLine in interiors)
                surface.Interior.Add(CreateReferenceSurfaceRing(curveFeatures, interiorRefLine));

            return surface;
        }

        private static List<CurveReference> CreateReferenceSurfaceRing(IEnumerable<CurveFeature> curveFeatures, string refLine)
        {
            var refs = refLine.Split(":");
            var curveReferences = new List<CurveReference>();

            foreach (var reference in refs)
            {
                if (!int.TryParse(reference.Trim(), out var sequenceNumber))
                    continue;

                var reversed = false;

                if (sequenceNumber < 0)
                {
                    sequenceNumber *= -1;
                    reversed = true;
                }

                var curveFeature = curveFeatures.SingleOrDefault(feature => feature.SequenceNumber == sequenceNumber);

                if (curveFeature != null)
                    curveReferences.Add(new CurveReference(curveFeature, reversed));
            }

            return curveReferences;
        }
    }
}

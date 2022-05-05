using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Constants;
using Sosi2Gml.Application.Models.Geometries;
using Sosi2Gml.Application.Models.Sosi;
using System.Globalization;
using System.Reflection;

namespace Sosi2Gml.Application.Helpers
{
    public class MapperHelper
    {
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

        public static string GenerateGmlId()
        {
            return $"_{Guid.NewGuid()}";
        }

        public static List<Point> GetPoints(SosiValues sosiValues, int decimalPlaces)
        {
            var points = new List<Point>();

            for (int i = 0; i < sosiValues.Lines.Count; i++)
            {
                var value = sosiValues.Lines[i];

                if (Regexes.PointsStartRegex.IsMatch(value))
                {
                    var pointValues = sosiValues.Lines.Skip(i);

                    foreach (var pointValue in pointValues)
                    {
                        var pointMatch = Regexes.PointRegex.Match(pointValue);

                        if (pointMatch.Success)
                            points.Add(Point.Create(pointMatch.Groups["x"].Value, pointMatch.Groups["y"].Value, decimalPlaces));
                    }

                    break;
                }
            }

            return points;
        }
    }
}

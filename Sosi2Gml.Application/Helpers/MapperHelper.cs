using Sosi2Gml.Application.Models.Sosi;
using System.Globalization;

namespace Sosi2Gml.Application.Helpers
{
    public class MapperHelper
    {
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
    }
}

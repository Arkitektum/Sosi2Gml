using Sosi2Gml.Application.Models.Geometries;
using System.Text.RegularExpressions;

namespace Sosi2Gml.Application.Models.Sosi
{
    public class Envelope
    {
        private static readonly Regex _pointRegex = new(@"^(?<y>\d+) (?<x>\d+)", RegexOptions.Compiled);

        private Envelope(Point lowerCorner, Point upperCorner)
        {
            LowerCorner = lowerCorner;
            UpperCorner = upperCorner;
        }

        public Point LowerCorner { get; set; }
        public Point UpperCorner { get; set; }

        public static Envelope Create(string minNorthEast, string maxNorthEast)
        {
            var pointMatchMinNe = _pointRegex.Match(minNorthEast);
            var pointMatchMaxNe = _pointRegex.Match(maxNorthEast);
            var lowerCorner = Point.Create(pointMatchMinNe.Groups["x"].Value, pointMatchMinNe.Groups["y"].Value, 0);
            var upperCorner = Point.Create(pointMatchMaxNe.Groups["x"].Value, pointMatchMaxNe.Groups["y"].Value, 0);

            return new Envelope(lowerCorner, upperCorner);
        }
    }
}

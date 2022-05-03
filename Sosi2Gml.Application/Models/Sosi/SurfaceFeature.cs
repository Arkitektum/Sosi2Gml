using System.Text.RegularExpressions;

namespace Sosi2Gml.Application.Models.Sosi
{
    public abstract class SurfaceFeature : GeometryFeature
    {
        private static readonly Regex _curveReferencesRegex = new(@"\.\.REF(?<refs>(.*))\.\.NØ", RegexOptions.Compiled | RegexOptions.Singleline);
        private static readonly Regex _surfaceRingRegex = new(@"(?<exterior>.*?)(?<interiors>\(.*?\))", RegexOptions.Compiled);

        public SurfaceFeature(
            SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<CurveFeature> curveFeatures) : base(sosiObject, srsName, decimalPlaces)
        {
            SetCurveReferences(curveFeatures);
        }

        public Surface Surface { get; set; } = new();

        private void SetCurveReferences(IEnumerable<CurveFeature> curveFeatures)
        {
            var joined = string.Join(Environment.NewLine, SosiValues.Lines);
            var match = _curveReferencesRegex.Match(joined);

            if (!match.Success)
                return;

            var refLine = match.Groups["refs"].Value;
            var ringMatches = _surfaceRingRegex.Matches(refLine);

            if (!ringMatches.Any())
            {
                Surface.Exterior.AddRange(CreateSurfaceRing(curveFeatures, refLine));
                return;
            }

            var exterior = ringMatches
                .Select(match => match.Groups["exterior"].Value)
                .FirstOrDefault(refLine => !string.IsNullOrWhiteSpace(refLine));

            Surface.Exterior.AddRange(CreateSurfaceRing(curveFeatures, exterior));

            var interiors = ringMatches
                .Select(match => match.Groups["interiors"].Value)
                .Where(refLine => !string.IsNullOrWhiteSpace(refLine));

            foreach (var interiorRefLine in interiors)
                Surface.Interior.Add(CreateSurfaceRing(curveFeatures, interiorRefLine));
        }

        private static List<CurveReference> CreateSurfaceRing(IEnumerable<CurveFeature> curveFeatures, string refLine)
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

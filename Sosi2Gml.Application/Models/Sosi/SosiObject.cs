using System.Text.RegularExpressions;

namespace Sosi2Gml.Application.Models.Sosi
{
    public class SosiObject
    {
        private static readonly Regex _elementTypeRegex = new(@"^\.(?<elementType>([A-ZÆØÅ]+))( (?<sn>\d+))?", RegexOptions.Compiled);

        public string ElementType { get; private set; }
        public int SequenceNumber { get; private set; }
        public SosiValues SosiValues { get; private set; }

        public string GetValue(string key)
        {
            if (SosiValues == null)
                return null;

            if (SosiValues.ObjectProperties.TryGetValue(key, out var value))
                return value.Trim('"', '\'');

            return null;
        }

        public bool HasValue(string key)
        {
            if (SosiValues == null)
                return false;

            return SosiValues.ObjectProperties.ContainsKey(key);
        }

        public static SosiObject Create(string elementName, List<string> values)
        {
            var elementTypeMatch = _elementTypeRegex.Match(elementName);
            var elementType = elementTypeMatch.Groups["elementType"].Value;

            var sosiObject = new SosiObject
            {
                ElementType = elementType,
                SosiValues = SosiValues.Create(values)
            };

            if (int.TryParse(elementTypeMatch.Groups["sn"].Value, out var sequenceNumber))
                sosiObject.SequenceNumber = sequenceNumber;

            return sosiObject;
        }
    }
}

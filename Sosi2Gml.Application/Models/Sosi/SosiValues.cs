using System.Text.RegularExpressions;

namespace Sosi2Gml.Application.Models.Sosi
{
    public class SosiValues
    {
        private static readonly Regex _propertyRegex = new(@"^\.+((?!(NØ|REF)).)*$", RegexOptions.Compiled);
        private static readonly Regex _nameAndValueRegex = new(@"^(?<name>(\.+[A-ZÆØÅ-]+))( (?<value>.*))?", RegexOptions.Compiled);

        private SosiValues(List<string> values, Dictionary<string, string> objectProperties)
        {
            Values = values;
            ObjectProperties = objectProperties;
        }

        public Dictionary<string, string> ObjectProperties { get; private set; } = new();
        public List<string> Values { get; private set; } = new();

        public string Get(string key)
        {
            return ObjectProperties.TryGetValue(key, out var value) ? value : null;
        }

        public bool Has(string key)
        {
            return ObjectProperties.ContainsKey(key);
        }

        public static SosiValues Create(List<string> values)
        {
            var propertyLines = values.Where(value => _propertyRegex.IsMatch(value));
            var objectProperties = new Dictionary<string, string>();

            foreach (var line in propertyLines)
            {
                var match = _nameAndValueRegex.Match(line);

                if (match.Success)
                {
                    var value = match.Groups["value"].Value;
                    objectProperties.Add(match.Groups["name"].Value, !string.IsNullOrWhiteSpace(value) ? value : null);
                }
            }

            return new SosiValues(values, objectProperties);
        }
    }
}

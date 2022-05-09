namespace Sosi2Gml.Application.Models.Config
{
    public class CrsSettings
    {
        public static readonly string SectionName = "Crs";
        private readonly Dictionary<string, string> _crss;

        public CrsSettings(Dictionary<string, string> crss)
        {
            _crss = crss;
        }

        public string GetSrsName(string code)
        {
            return _crss.TryGetValue(code, out var srsName) ? srsName : null;
        }
    }
}

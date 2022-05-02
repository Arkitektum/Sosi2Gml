using System.Text.RegularExpressions;

namespace Sosi2Gml.Application.Constants
{
    public class Regexes
    {
        public static readonly Regex SosiObjectRegex = new(@"^\.[A-ZÆØÅ]+", RegexOptions.Compiled);
        public static readonly Regex ObjTypeRegex = new(@"^\.\.OBJTYPE (?<objType>(\w+))", RegexOptions.Compiled);
        public static readonly Regex CartographicElementTypeRegex = new(@"^\.(?<cartographicElementType>([A-ZÆØÅ]+)) (?<sn>\d+)\:", RegexOptions.Compiled);
        public static readonly Regex PointsStartRegex = new(@"^\.\.NØ", RegexOptions.Compiled);
        public static readonly Regex PointRegex = new(@"^(?<y>\d+) (?<x>\d+)", RegexOptions.Compiled);
    }
}

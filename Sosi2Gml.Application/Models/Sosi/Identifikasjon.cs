using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Sosi2Gml.Application.Models.Sosi
{
    public class Identifikasjon : GmlElement
    {
        private static readonly Regex _uuidRegex =
            new("^[a-f0-9]{8}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{4}-[a-f0-9]{12}$", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public Identifikasjon(SosiObject sosiObject)
        {
            LokalId = GetLokalId(sosiObject.GetValue("...LOKALID"));
            Navnerom = sosiObject.GetValue("...NAVNEROM");
            VersjonId = sosiObject.GetValue("...VERSJONID");
        }

        public string LokalId { get; set; }
        public string Navnerom { get; set; }
        public string VersjonId { get; set; }

        public override XElement ToGml(XNamespace ns)
        {
            return new XElement(ns + "identifikasjon",
                new XElement(ns + "Identifikasjon",
                    new XElement(ns + "lokalId", LokalId),
                    new XElement(ns + "navnerom", Navnerom),
                    new XElement(ns + "versjonId", VersjonId)
                )
            );
        }

        private static string GetLokalId(string lokalId)
        {
            if (_uuidRegex.IsMatch(lokalId))
                return lokalId;

            return Guid.NewGuid().ToString();
        }
    }
}

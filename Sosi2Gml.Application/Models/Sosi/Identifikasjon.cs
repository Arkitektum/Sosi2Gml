using System.Xml.Linq;

namespace Sosi2Gml.Application.Models.Sosi
{
    public class Identifikasjon : GmlElement
    {
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
    }
}

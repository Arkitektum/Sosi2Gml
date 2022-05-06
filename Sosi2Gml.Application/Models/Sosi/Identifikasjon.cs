using System.Xml.Linq;

namespace Sosi2Gml.Application.Models.Sosi
{
    public class Identifikasjon : GmlElement
    {
        public Identifikasjon(SosiObject sosiObject)
        {
            LokalId = Guid.NewGuid().ToString();
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
    }
}

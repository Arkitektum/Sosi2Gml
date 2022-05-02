using System.Xml.Linq;

namespace Sosi2Gml.Application.Models.Sosi
{
    public class Kvalitet : GmlElement
    {
        public string Målemetode { get; set; }
        public string Nøyaktighet { get; set; }

        public override XElement ToGml(XNamespace ns)
        {
            return new XElement(ns + "kvalitet",
                new XElement(ns + "Posisjonskvalitet",
                    new XElement(ns + "målemetode", Målemetode),
                    new XElement(ns + "nøyaktighet", Nøyaktighet ?? "0")
                )
            );
        }
    }
}

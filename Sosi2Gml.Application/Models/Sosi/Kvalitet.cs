using System.Xml.Linq;

namespace Sosi2Gml.Application.Models.Sosi
{
    public class Kvalitet : GmlElement
    {
        public Kvalitet(SosiObject sosiObject)
        {
            Målemetode = sosiObject.GetValue("..KVALITET");
        }

        public string Målemetode { get; set; }

        public override XElement ToGml(XNamespace ns)
        {
            return new XElement(ns + "kvalitet",
                new XElement(ns + "Posisjonskvalitet",
                    new XElement(ns + "målemetode", Målemetode),
                    new XElement(ns + "nøyaktighet", 0)
                )
            );
        }
    }
}

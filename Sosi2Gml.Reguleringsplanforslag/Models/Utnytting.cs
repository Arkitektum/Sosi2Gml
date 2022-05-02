using Sosi2Gml.Application.Models;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    public class Utnytting : GmlElement
    {
        public string Utnyttingstype { get; set; }
        public string Utnyttingstall { get; set; }
        public string UtnyttingstallMinimum { get; set; }

        public override XElement ToGml(XNamespace ns)
        {
            return new XElement(ns + "utnytting",
                new XElement(ns + "Utnytting",
                    new XElement(ns + "utnyttingstype", Utnyttingstype),
                    new XElement(ns + "utnyttingstall", Utnyttingstall ?? "0"),
                    new XElement(ns + "utnyttingstall_minimum", UtnyttingstallMinimum ?? "0")
                )
            );
        }
    }
}

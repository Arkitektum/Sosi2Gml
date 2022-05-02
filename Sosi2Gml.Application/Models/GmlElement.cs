using System.Xml.Linq;

namespace Sosi2Gml.Application.Models
{
    public abstract class GmlElement
    {
        public abstract XElement ToGml(XNamespace ns);
    }
}

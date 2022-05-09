using System.Xml.Linq;

namespace Sosi2Gml.Application.Models.Features
{
    public abstract class GmlElement
    {
        public abstract XElement ToGml(XNamespace appNs);
    }
}

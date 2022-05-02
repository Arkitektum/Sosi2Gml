using Sosi2Gml.Application.Models;
using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Application.Mappers.Interfaces
{
    public interface IGmlElementMapper<TGmlElement> where TGmlElement : GmlElement
    {
        TGmlElement Map(SosiObject sosiObject);
    }
}

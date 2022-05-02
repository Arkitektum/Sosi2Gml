using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Application.Mappers.Interfaces
{
    public interface IHodeMapper
    {
        Hode Map(IEnumerable<string> values);
    }
}

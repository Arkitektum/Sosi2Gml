using Sosi2Gml.Application.Models.Features;
using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public interface IRpBestemmelseMidlByggAnleggMapper
    {
        List<Feature> Map(SosiDocument document);
    }
}

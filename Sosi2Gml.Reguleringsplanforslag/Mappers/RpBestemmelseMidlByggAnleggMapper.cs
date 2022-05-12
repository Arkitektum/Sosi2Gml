using Sosi2Gml.Application.Mappers;
using Sosi2Gml.Application.Models.Features;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Models;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public class RpBestemmelseMidlByggAnleggMapper : IRpBestemmelseMidlByggAnleggMapper
    {
        private readonly IGmlMapper _gmlMapper;

        public RpBestemmelseMidlByggAnleggMapper(
            IGmlMapper gmlMapper)
        {
            _gmlMapper = gmlMapper;
        }

        public List<Feature> Map(SosiDocument document)
        {
            var features = _gmlMapper.MapCurveAndSurfaceFeatures<PblMidlByggAnleggGrense, RpBestemmelseMidlByggAnlegg>(document);
            var midlByggAnlegg = features.OfType<RpBestemmelseMidlByggAnlegg>().ToList();

            for (var i = 0; i < midlByggAnlegg.Count; i++)
                midlByggAnlegg[i].BestemmelseOmrådeNavn = $"#{i + 1}_MBA";

            return features;
        }
    }
}

using Sosi2Gml.Application.Models.Features;
using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Application.Mappers
{
    public interface IGmlMapper
    {
        List<Feature> MapPointFeatures<TPointFeature>(SosiDocument document)
            where TPointFeature : PointFeature;

        List<Feature> MapCurveFeatures<TCurveFeature>(SosiDocument document)
            where TCurveFeature : CurveFeature;

        List<Feature> MapCurveAndSurfaceFeatures<TCurveFeature, TSurfaceFeature>(SosiDocument document)
            where TCurveFeature : CurveFeature
            where TSurfaceFeature : SurfaceFeature;
    }
}

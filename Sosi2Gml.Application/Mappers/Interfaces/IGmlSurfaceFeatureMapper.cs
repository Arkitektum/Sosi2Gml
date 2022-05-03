using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Application.Mappers.Interfaces
{
    public interface IGmlSurfaceFeatureMapper<TSurfaceFeatureModel, TCurveFeatureModel> 
        where TSurfaceFeatureModel : SurfaceFeature
        where TCurveFeatureModel : CurveFeature
    {
        TSurfaceFeatureModel Map(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<TCurveFeatureModel> curveFeatures);
    }
}

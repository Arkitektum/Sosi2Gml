using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Application.Mappers.Interfaces
{
    public interface IGmlCurveFeatureMapper<TCurveFeature> 
        where TCurveFeature : CurveFeature
    {
        TCurveFeature Map(SosiObject sosiObject, string srsName, int decimalPlaces);
    }
}

using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Application.Mappers.Interfaces
{
    public interface IGmlFeatureMapper<TFeatureModel> where TFeatureModel : Feature
    {
        TFeatureModel Map(SosiObject sosiObject, string srsName, int decimalPlaces);
    }
}

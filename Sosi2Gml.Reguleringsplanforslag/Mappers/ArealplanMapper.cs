using Sosi2Gml.Application.Models.Features;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Models;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public class ArealplanMapper : IArealplanMapper
    {
        public List<Feature> Map(SosiDocument document)
        {
            var sosiObject = document.GetSosiObjects<RpOmråde>().First();

            return new List<Feature> { new Arealplan(sosiObject) };
        }
    }
}

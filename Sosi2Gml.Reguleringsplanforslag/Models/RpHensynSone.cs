using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    public abstract class RpHensynSone : SurfaceFeature
    {
        protected RpHensynSone(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<CurveFeature> curveFeatures) : 
            base(sosiObject, srsName, decimalPlaces, curveFeatures)
        {
            
        
        }

        public string HensynSonenavn { get; set; }
        public RpOmråde Planområde { get; set; }
        public List<RpPåskrift> Påskrifter { get; set; }
    }
}

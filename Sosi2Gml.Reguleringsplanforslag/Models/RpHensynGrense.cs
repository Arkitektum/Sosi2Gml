using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    public abstract class RpHensynGrense : CurveFeature
    {
        public RpHensynGrense(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
        }
    }
}

using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    public class RpOmråde : SurfaceFeature
    {
        public RpOmråde(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
        }

        public override string FeatureName => "RpOmråde";

        public override XElement ToGml()
        {
            throw new NotImplementedException();
        }
    }
}

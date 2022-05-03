using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    public class RpSikringGrense : RpHensynGrense
    {
        public RpSikringGrense(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
        }

        public override string FeatureName => "RpSikringGrense";

        public override XElement ToGml()
        {
            throw new NotImplementedException();
        }
    }
}

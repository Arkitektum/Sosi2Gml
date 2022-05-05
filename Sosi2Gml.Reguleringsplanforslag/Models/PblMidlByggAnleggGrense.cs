using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("PblMidlByggAnleggGrense")]
    public class PblMidlByggAnleggGrense : CurveFeature
    {
        public PblMidlByggAnleggGrense(SosiObject sosiObject, string srsName, int decimalPlaces) : 
            base(sosiObject, srsName, decimalPlaces)
        {
        }

        public override string FeatureName => "PblMidlByggAnleggGrense";

        public override XElement ToGml() => null;
    }
}

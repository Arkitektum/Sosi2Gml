using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("PblMidlByggAnleggOmråde")]
    public class RpBestemmelseMidlByggAnlegg : SurfaceFeature
    {
        public RpBestemmelseMidlByggAnlegg(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<CurveFeature> curveFeatures) : 
            base(sosiObject, srsName, decimalPlaces, curveFeatures)
        {
        }

        public override string FeatureName => "RpBestemmelseMidlByggAnlegg";

        public override XElement ToGml()
        {
            throw new NotImplementedException();
        }
    }
}

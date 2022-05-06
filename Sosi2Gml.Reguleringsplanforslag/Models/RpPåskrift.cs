using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpPåskrift")]
    public class RpPåskrift : PointFeature
    {
        public RpPåskrift(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
            Tekststreng = sosiObject.GetValue("..STRENG");
            PåskriftType = sosiObject.GetValue("..RPPÅSKRIFTTYPE");
        }

        public string Tekststreng { get; set; }
        public string PåskriftType { get; set; }
        public RpOmråde Planområde { get; set; }
        public RpArealformålOmråde Formål { get; set; }
        public RpHensynSone Hensyn { get; set; }
        public RpBestemmelseOmråde BestemmelseOmråde { get; set; }
        public RpBestemmelseMidlByggAnlegg MidlByggAnlegg { get; set; }
        public RpRegulertHøyde RegulertHøyde { get; set; }
        public RpJuridiskLinje JuridiskLinje { get; set; }

        public override string FeatureName => throw new NotImplementedException();

        public override XElement ToGml()
        {
            return null;
        }
    }
}

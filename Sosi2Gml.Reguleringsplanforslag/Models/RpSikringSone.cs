using Sosi2Gml.Application.Models.Sosi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    public class RpSikringSone : RpHensynSone
    {
        public RpSikringSone(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
        }

        public string Sikring { get; set; }
        public override string FeatureName => "RpSikringSone";

        public override XElement ToGml()
        {
            throw new NotImplementedException();
        }
    }
}

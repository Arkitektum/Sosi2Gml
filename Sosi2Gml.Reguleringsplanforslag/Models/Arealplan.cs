using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    public class Arealplan : Feature
    {
        public Arealplan(SosiObject sosiObject) : base(sosiObject)
        {
        }

        public override string FeatureName => "Arealplan";

        public override XElement ToGml()
        {
            throw new NotImplementedException();
        }
    }
}

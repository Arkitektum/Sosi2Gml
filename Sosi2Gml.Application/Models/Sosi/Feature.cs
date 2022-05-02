using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Application.Models.Sosi
{
    public abstract class Feature
    {
        protected Feature(SosiObject sosiObject)
        {
            SosiValues = sosiObject.SosiValues;
        }

        public abstract string FeatureName { get; }
        public string GmlId { get; set; } = GenerateGmlId();
        public List<Feature> References { get; private set; } = new();
        public SosiValues SosiValues { get; private set; }
        public abstract XElement ToGml();
    }
}

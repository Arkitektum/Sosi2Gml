using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.GmlHelper;

namespace Sosi2Gml.Application.Models.Features
{
    public abstract class Feature
    {
        protected Feature(SosiObject sosiObject)
        {
            SosiValues = sosiObject.SosiValues;
        }

        public abstract string FeatureName { get; }
        public string GmlId { get; set; } = CreateGmlId();
        public SosiValues SosiValues { get; private set; }
        public virtual XElement ToGml(XNamespace appNs) => null;

        public virtual void AddAssociations(List<Feature> features)
        {
        }
    }
}

using OSGeo.OGR;
using Sosi2Gml.Application.Models.Features;
using System.Xml.Linq;
using Feature = Sosi2Gml.Application.Models.Features.Feature;

namespace Sosi2Gml.Application.Helpers
{
    public class GeometryHelper
    {
        public static Geometry GeometryFromGml(XElement geomElement)
        {
            try
            {
                return Geometry.CreateFromGML(geomElement.ToString());
            }
            catch
            {
                return null;
            }
        }

        public static T GetClosestFeature<T>(List<Feature> features, Geometry geometry) where T : MapFeature
        {
            if (geometry == null || !geometry.IsValid())
                return null;

            var featuresOfType = features.OfType<T>().ToList();

            if (featuresOfType.Count == 1)
                return featuresOfType.Single();

            return featuresOfType.MinBy(feature => feature.Geometry?.Distance(geometry) ?? double.PositiveInfinity);
        }
    }
}

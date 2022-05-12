using Sosi2Gml.Application.Models.Features;
using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Application.Mappers
{
    public class GmlMapper : IGmlMapper
    {
        public List<Feature> MapPointFeatures<TPointFeature>(SosiDocument document)
            where TPointFeature : PointFeature
        {
            var features = new List<Feature>();
            var pointObjects = document.GetSosiObjects<TPointFeature>();

            var pointFeatures = pointObjects
                .ConvertAll(sosiObject => Activator.CreateInstance(typeof(TPointFeature), new object[] { sosiObject, document.SrsName, document.DecimalCount }) as TPointFeature);

            features.AddRange(pointFeatures);

            return features;
        }

        public List<Feature> MapCurveFeatures<TCurveFeature>(SosiDocument document)
            where TCurveFeature : CurveFeature
        {
            var features = new List<Feature>();
            var curveObjects = document.GetSosiObjects<TCurveFeature>();

            var curveFeatures = curveObjects
                .ConvertAll(sosiObject => Activator.CreateInstance(typeof(TCurveFeature), new object[] { sosiObject, document.SrsName, document.DecimalCount }) as TCurveFeature);

            features.AddRange(curveFeatures);

            return features;
        }

        public List<Feature> MapCurveAndSurfaceFeatures<TCurveFeature, TSurfaceFeature>(SosiDocument document)
            where TCurveFeature : CurveFeature
            where TSurfaceFeature : SurfaceFeature
        {
            var features = new List<Feature>();
            var curveObjects = document.GetSosiObjects<TCurveFeature>();
            var surfaceObjects = document.GetSosiObjects<TSurfaceFeature>();

            var curveFeatures = curveObjects
                .ConvertAll(sosiObject => Activator.CreateInstance(typeof(TCurveFeature), new object[] { sosiObject, document.SrsName, document.DecimalCount }) as TCurveFeature);

            var surfaceFeatures = surfaceObjects
                .ConvertAll(sosiObject => Activator.CreateInstance(typeof(TSurfaceFeature), new object[] { sosiObject, document.SrsName, document.DecimalCount, curveFeatures }) as TSurfaceFeature);

            features.AddRange(curveFeatures);
            features.AddRange(surfaceFeatures);

            return features;
        }
    }
}

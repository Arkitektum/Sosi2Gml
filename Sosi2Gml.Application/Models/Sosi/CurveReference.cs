namespace Sosi2Gml.Application.Models.Sosi
{
    public class CurveReference
    {
        public CurveReference(CurveFeature feature, bool reversed)
        {
            Feature = feature;
            Reversed = reversed;
        }

        public CurveFeature Feature { get; private set; }
        public bool Reversed { get; private set; }
    }

    public class Surface
    {
        public List<CurveReference> Exterior { get; set; } = new();
        public List<List<CurveReference>> Interior { get; set; } = new();

        public List<TCurveFeatureModel> GetFeatures<TCurveFeatureModel>() where TCurveFeatureModel : CurveFeature
        {
            var features = new List<TCurveFeatureModel>();

            features.AddRange(Exterior.Select(curveReference => (TCurveFeatureModel)curveReference.Feature));
            features.AddRange(Interior.SelectMany(interior => interior.Select(curveReference => (TCurveFeatureModel)curveReference.Feature)));

            return features;
        }
    }
}

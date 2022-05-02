namespace Sosi2Gml.Application.Models.Sosi
{
    public abstract class SurfaceFeature : GeometryFeature
    {
        public SurfaceFeature(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
        }

        public List<CurveReference> CurveReferences { get; set; } = new();
    }
}

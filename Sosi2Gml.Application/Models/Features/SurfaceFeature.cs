using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.SosiHelper;

namespace Sosi2Gml.Application.Models.Features
{
    public abstract class SurfaceFeature : MapFeature
    {
        public SurfaceFeature(
            SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<CurveFeature> curveFeatures) : base(sosiObject, srsName, decimalPlaces)
        {
            Surface = CreateReferenceSurface(curveFeatures, SosiValues.Lines);
        }

        public ReferenceSurface Surface { get; set; } = new();
        public override XElement ToGml(XNamespace appNs) => base.ToGml(appNs);

        public override XElement GeomElement
        {
            get
            {
                var exterior = CreateSurfaceRing(Surface.Exterior);
                var interiors = Surface.Interior.Select(curveReferences => CreateSurfaceRing(curveReferences));

                return CreateMultiSurface(exterior, interiors, GmlId, SrsName);
            }
        }
    }
}

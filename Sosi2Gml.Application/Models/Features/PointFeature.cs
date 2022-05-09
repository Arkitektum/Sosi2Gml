using Sosi2Gml.Application.Models.Geometries;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.SosiHelper;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Application.Models.Features
{
    public abstract class PointFeature : MapFeature
    {
        protected PointFeature(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
            Points = GetPoints(SosiValues, decimalPlaces);
        }

        public override XElement GeomElement => CreatePoint(Points.First(), $"{GmlId}-0", SrsName);
        public override XElement ToGml(XNamespace appNs) => base.ToGml(appNs);
        public List<Point> Points { get; private set; }
    }
}

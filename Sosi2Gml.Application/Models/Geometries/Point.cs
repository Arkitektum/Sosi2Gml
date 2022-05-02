namespace Sosi2Gml.Application.Models.Geometries
{
    public class Point
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public override string ToString()
        {
            return FormattableString.Invariant($"{X} {Y}");
        }

        public static Point Create(string x, string y, int numDecimals)
        {
            return new Point(double.Parse(x.Insert(x.Length - numDecimals, ".")), double.Parse(y.Insert(y.Length - numDecimals, ".")));
        }
    }
}

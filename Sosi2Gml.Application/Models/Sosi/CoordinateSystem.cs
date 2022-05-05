namespace Sosi2Gml.Application.Models.Sosi
{
    public class CoordinateSystem
    {
        public CoordinateSystem(string code, string uri)
        {
            Code = code;
            Uri = uri;
        }

        public string Code { get; set; }
        public string Uri { get; set; }
    }
}

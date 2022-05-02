namespace Sosi2Gml.Application.Models.Sosi
{
    public class Koordinatsystem
    {
        public Koordinatsystem(int kode, string uri)
        {
            Kode = kode;
            Uri = uri;
        }

        public int Kode { get; set; }
        public string Uri { get; set; }
    }
}

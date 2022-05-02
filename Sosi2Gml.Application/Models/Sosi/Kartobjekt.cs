namespace Sosi2Gml.Application.Models.Sosi
{
    public abstract class Kartobjekt : Feature
    {
        protected Kartobjekt(SosiObject sosiObject) : base(sosiObject)
        {
        }

        public Identifikasjon Identifikasjon { get; set; }
        public DateTime? FørsteDigitaliseringsdato { get; set; }
        public DateTime Oppdateringsdato { get; set; }
        public Kvalitet Kvalitet { get; set; }
    }
}

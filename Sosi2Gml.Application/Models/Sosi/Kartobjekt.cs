using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Application.Models.Sosi
{
    public abstract class Kartobjekt : Feature
    {
        protected Kartobjekt(SosiObject sosiObject) : base(sosiObject)
        {
            if (sosiObject.HasValue("..IDENT"))
                Identifikasjon = new Identifikasjon(sosiObject);

            if (sosiObject.HasValue("..KVALITET"))
                Kvalitet = new Kvalitet(sosiObject);

            FørsteDigitaliseringsdato = SosiDateToDateTime(sosiObject.GetValue("..FØRSTEDIGITALISERINGSDATO"));
            Oppdateringsdato = SosiDateToDateTime(sosiObject.GetValue("..OPPDATERINGSDATO")).Value;
        }

        public Identifikasjon Identifikasjon { get; set; }
        public Kvalitet Kvalitet { get; set; }
        public DateTime? FørsteDigitaliseringsdato { get; set; }
        public DateTime Oppdateringsdato { get; set; }
    }
}

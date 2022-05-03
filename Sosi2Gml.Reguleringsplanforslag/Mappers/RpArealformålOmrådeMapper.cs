using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Models;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public class RpArealformålOmrådeMapper : IGmlSurfaceFeatureMapper<RpArealformålOmråde, RpFormålGrense>
    {
        private readonly IGmlElementMapper<Identifikasjon> _identifikasjonMapper;
        private readonly IGmlElementMapper<Kvalitet> _kvalitetMapper;

        public RpArealformålOmrådeMapper(
            IGmlElementMapper<Identifikasjon> identifikasjonMapper,
            IGmlElementMapper<Kvalitet> kvalitetMapper)
        {
            _identifikasjonMapper = identifikasjonMapper;
            _kvalitetMapper = kvalitetMapper;
        }

        public RpArealformålOmråde Map(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpFormålGrense> curveFeatures)
        {
            var rpArealformålOmråde = new RpArealformålOmråde(sosiObject, srsName, decimalPlaces, curveFeatures)
            {
                Identifikasjon = _identifikasjonMapper.Map(sosiObject),
                Kvalitet = _kvalitetMapper.Map(sosiObject),
                FørsteDigitaliseringsdato = SosiDateToDateTime(sosiObject.GetValue("..FØRSTEDIGITALISERINGSDATO")),
                Oppdateringsdato = SosiDateToDateTime(sosiObject.GetValue("..OPPDATERINGSDATO")).Value,
                Arealformål = sosiObject.GetValue("..RPAREALFORMÅL"),
                Feltnavn = sosiObject.GetValue("..FELTNAVN"),
                Beskrivelse = sosiObject.GetValue("..BESKRIVELSE"),
                Eierform = sosiObject.GetValue("..EIERFORM"),
                Uteoppholdsareal = sosiObject.GetValue("..UTEAREAL"),
                Avkjørselsbestemmelse = sosiObject.GetValue("..AVKJ"),
                Byggverkbestemmelse = sosiObject.GetValue("..BYGGVERK"),
            };

            if (sosiObject.HasValue("..UTNYTT"))
            {
                rpArealformålOmråde.Utnyttinger.Add(new Utnytting
                {
                    Utnyttingstype = sosiObject.GetValue("...UTNTYP"),
                    Utnyttingstall = sosiObject.GetValue("...UTNTALL"),
                    UtnyttingstallMinimum = sosiObject.GetValue("...UTNTALL_MIN"),
                });
            }

            rpArealformålOmråde.AvgrensesAv.AddRange(rpArealformålOmråde.Surface.GetFeatures<RpFormålGrense>());

            return rpArealformålOmråde;
        }
    }
}

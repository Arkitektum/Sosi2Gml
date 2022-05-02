using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Mappers.Interfaces;
using Sosi2Gml.Reguleringsplanforslag.Models;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public class RpGrenseMapper : IRpGrenseMapper
    {
        private readonly IGmlElementMapper<Identifikasjon> _identifikasjonMapper;
        private readonly IGmlElementMapper<Kvalitet> _kvalitetMapper;

        public RpGrenseMapper(
            IGmlElementMapper<Identifikasjon> identifikasjonMapper,
            IGmlElementMapper<Kvalitet> kvalitetMapper)
        {
            _identifikasjonMapper = identifikasjonMapper;
            _kvalitetMapper = kvalitetMapper;
        }

        public RpGrense Map(SosiObject sosiObject)
        {
            var rpGrense = new RpGrense(sosiObject, "http://www.opengis.net/def/crs/EPSG/0/25832", 2)
            {
                Identifikasjon = _identifikasjonMapper.Map(sosiObject),
                Kvalitet = _kvalitetMapper.Map(sosiObject)
            };

            var førsteDigitaliseringsdato = sosiObject.SosiValues.Get("..FØRSTEDIGITALISERINGSDATO");

            if (førsteDigitaliseringsdato != null)
                rpGrense.FørsteDigitaliseringsdato = SosiDateToDateTime(førsteDigitaliseringsdato);

            rpGrense.Oppdateringsdato = SosiDateToDateTime(sosiObject.SosiValues.Get("..OPPDATERINGSDATO"));

            return rpGrense;
        }
    }
}

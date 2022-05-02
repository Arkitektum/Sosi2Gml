using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Mappers.Interfaces;
using Sosi2Gml.Reguleringsplanforslag.Models;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public class RpFormålGrenseMapper : IRpFormålGrenseMapper
    {
        private readonly IGmlElementMapper<Identifikasjon> _identifikasjonMapper;
        private readonly IGmlElementMapper<Kvalitet> _kvalitetMapper;

        public RpFormålGrenseMapper(
            IGmlElementMapper<Identifikasjon> identifikasjonMapper,
            IGmlElementMapper<Kvalitet> kvalitetMapper)
        {
            _identifikasjonMapper = identifikasjonMapper;
            _kvalitetMapper = kvalitetMapper;
        }

        public RpFormålGrense Map(SosiObject sosiObject)
        {
            var rpFormålGrense = new RpFormålGrense(sosiObject, "http://www.opengis.net/def/crs/EPSG/0/25832", 2)
            {
                Identifikasjon = _identifikasjonMapper.Map(sosiObject),
                Kvalitet = _kvalitetMapper.Map(sosiObject)
            };

            var førsteDigitaliseringsdato = sosiObject.GetValue("..FØRSTEDIGITALISERINGSDATO");

            if (førsteDigitaliseringsdato != null)
                rpFormålGrense.FørsteDigitaliseringsdato = SosiDateToDateTime(førsteDigitaliseringsdato);

            rpFormålGrense.Oppdateringsdato = SosiDateToDateTime(sosiObject.GetValue("..OPPDATERINGSDATO"));

            return rpFormålGrense;
        }
    }
}

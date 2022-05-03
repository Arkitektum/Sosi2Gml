using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Models;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public class RpFormålGrenseMapper : IGmlFeatureMapper<RpFormålGrense>
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

        public RpFormålGrense Map(SosiObject sosiObject, string srsName, int decimalPlaces)
        {
            var rpFormålGrense = new RpFormålGrense(sosiObject, srsName, decimalPlaces)
            {
                Identifikasjon = _identifikasjonMapper.Map(sosiObject),
                FørsteDigitaliseringsdato = SosiDateToDateTime(sosiObject.GetValue("..FØRSTEDIGITALISERINGSDATO")),
                Oppdateringsdato = SosiDateToDateTime(sosiObject.GetValue("..OPPDATERINGSDATO")).Value,
                Kvalitet = _kvalitetMapper.Map(sosiObject)
            };

            return rpFormålGrense;
        }
    }
}

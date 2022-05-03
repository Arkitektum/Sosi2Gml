using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Models;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public class RpOmrådeMapper : IGmlSurfaceFeatureMapper<RpOmråde, RpGrense>
    {
        private readonly IGmlElementMapper<Identifikasjon> _identifikasjonMapper;
        private readonly IGmlElementMapper<Kvalitet> _kvalitetMapper;

        public RpOmrådeMapper(
            IGmlElementMapper<Identifikasjon> identifikasjonMapper,
            IGmlElementMapper<Kvalitet> kvalitetMapper)
        {
            _identifikasjonMapper = identifikasjonMapper;
            _kvalitetMapper = kvalitetMapper;
        }

        public RpOmråde Map(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpGrense> curveFeatures)
        {
            var rpOmråde = new RpOmråde(sosiObject, srsName, decimalPlaces, curveFeatures)
            {
                Identifikasjon = _identifikasjonMapper.Map(sosiObject),
                Kvalitet = _kvalitetMapper.Map(sosiObject),
                FørsteDigitaliseringsdato = SosiDateToDateTime(sosiObject.GetValue("..FØRSTEDIGITALISERINGSDATO")),
                Oppdateringsdato = SosiDateToDateTime(sosiObject.GetValue("..OPPDATERINGSDATO")).Value,
                Vertikalnivå = sosiObject.GetValue("..VERTNIV")
            };

            return rpOmråde;
        }
    }
}

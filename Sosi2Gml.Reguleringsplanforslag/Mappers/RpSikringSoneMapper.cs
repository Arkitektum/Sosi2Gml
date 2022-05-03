using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Models;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public abstract class RpHensynSoneMapper
    {
        private readonly IGmlElementMapper<Identifikasjon> _identifikasjonMapper;
        private readonly IGmlElementMapper<Kvalitet> _kvalitetMapper;

        public RpHensynSoneMapper(
            IGmlElementMapper<Identifikasjon> identifikasjonMapper,
            IGmlElementMapper<Kvalitet> kvalitetMapper)
        {
            _identifikasjonMapper = identifikasjonMapper;
            _kvalitetMapper = kvalitetMapper;
        }

        public TRpHensynSoneModel Map<TRpHensynSoneModel>(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpSikringGrense> curveFeatures)
            where TRpHensynSoneModel : RpHensynSone
        {
            var rpHensynSone = Activator.CreateInstance(typeof(TRpHensynSoneModel), new object[] { sosiObject, srsName, decimalPlaces, curveFeatures }) as TRpHensynSoneModel;

            rpHensynSone.Identifikasjon = _identifikasjonMapper.Map(sosiObject);
            rpHensynSone.Kvalitet = _kvalitetMapper.Map(sosiObject);
            rpHensynSone.FørsteDigitaliseringsdato = SosiDateToDateTime(sosiObject.GetValue("..FØRSTEDIGITALISERINGSDATO"));
            rpHensynSone.Oppdateringsdato = SosiDateToDateTime(sosiObject.GetValue("..OPPDATERINGSDATO")).Value;
            rpHensynSone.HensynSonenavn = sosiObject.GetValue("..HENSYNSONENAVN");

            return rpHensynSone;
        }
    }

    public class RpSikringSoneMapper : RpHensynSoneMapper, IGmlSurfaceFeatureMapper<RpSikringSone, RpSikringGrense>
    {
        public RpSikringSoneMapper(
            IGmlElementMapper<Identifikasjon> identifikasjonMapper,
            IGmlElementMapper<Kvalitet> kvalitetMapper) : base(identifikasjonMapper, kvalitetMapper)
        {
        }

        public RpSikringSone Map(SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpSikringGrense> curveFeatures)
        {
            var rpSikringSone = Map<RpSikringSone>(sosiObject, srsName, decimalPlaces, curveFeatures);

            rpSikringSone.Sikring = sosiObject.GetValue("..RPSIKRING");

            return rpSikringSone;
        }
    }
}

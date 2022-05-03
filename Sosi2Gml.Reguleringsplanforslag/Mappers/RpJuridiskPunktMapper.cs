using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Geometries;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Models;
using static Sosi2Gml.Application.Helpers.MapperHelper;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers
{
    public class RpJuridiskPunktMapper : IGmlFeatureMapper<RpJuridiskPunkt>
    {
        private readonly IGmlElementMapper<Identifikasjon> _identifikasjonMapper;
        private readonly IGmlElementMapper<Kvalitet> _kvalitetMapper;

        public RpJuridiskPunktMapper(
            IGmlElementMapper<Identifikasjon> identifikasjonMapper,
            IGmlElementMapper<Kvalitet> kvalitetMapper)
        {
            _identifikasjonMapper = identifikasjonMapper;
            _kvalitetMapper = kvalitetMapper;
        }

        public RpJuridiskPunkt Map(SosiObject sosiObject, string srsName, int decimalPlaces)
        {
            var rpJuridiskPunkt = new RpJuridiskPunkt(sosiObject, srsName, decimalPlaces)
            {
                Identifikasjon = _identifikasjonMapper.Map(sosiObject),
                Kvalitet = _kvalitetMapper.Map(sosiObject),
                FørsteDigitaliseringsdato = SosiDateToDateTime(sosiObject.GetValue("..FØRSTEDIGITALISERINGSDATO")),
                Oppdateringsdato = SosiDateToDateTime(sosiObject.GetValue("..OPPDATERINGSDATO")).Value,
                JuridiskPunkt = sosiObject.GetValue("..RPJURPUNKT"),
            };

            rpJuridiskPunkt.Symbolretning = GetSymbolretning(rpJuridiskPunkt.Points);

            return rpJuridiskPunkt;
        }

        private static double[] GetSymbolretning(List<Point> points)
        {
            var firstPoint = points.First();
            var lastPoint = points.Last();

            return new[] { lastPoint.X - firstPoint.X, lastPoint.Y - firstPoint.Y };
        }
    }
}

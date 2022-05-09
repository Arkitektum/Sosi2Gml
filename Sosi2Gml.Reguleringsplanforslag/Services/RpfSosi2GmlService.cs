using Microsoft.AspNetCore.Http;
using Sosi2Gml.Application.Models.Config;
using Sosi2Gml.Application.Services.Sosi2Gml;
using Sosi2Gml.Reguleringsplanforslag.Constants;
using Sosi2Gml.Reguleringsplanforslag.Models;

namespace Sosi2Gml.Reguleringsplanforslag.Services
{
    public class RpfSosi2GmlService : Sosi2GmlService, IRpfSosi2GmlService
    {
        private readonly DatasetSettings _settings;

        public RpfSosi2GmlService(
            Datasets datasets,
            CrsSettings crsSettings) : base(crsSettings)
        {
            _settings = datasets.GetSettings(Dataset.Reguleringsplanforslag);
        }

        public async Task<MemoryStream> Sosi2GmlAsync(IFormFile sosiFile)
        {
            var document = await ReadSosiFileAsync(sosiFile);

            document.GetSosiObjects<RpOmråde>().First().SosiValues.ObjectProperties

            return await CreateGmlDocumentAsync(document, _settings, new[]
            {
                () => MapFeature<Arealplan>(document, document => new(document.GetSosiObjects<RpOmråde>().First())),
                () => MapCurveAndSurfaceFeatures<RpGrense, RpOmråde>(document),
                () => MapCurveAndSurfaceFeatures<RpFormålGrense, RpArealformålOmråde>(document),
                () => MapCurveAndSurfaceFeatures<RpBestemmelseGrense, RpBestemmelseOmråde>(document),
                () => MapCurveAndSurfaceFeatures<PblMidlByggAnleggGrense, RpBestemmelseMidlByggAnlegg>(document),
                () => MapCurveAndSurfaceFeatures<RpAngittHensynGrense, RpAngittHensynSone>(document),
                () => MapCurveAndSurfaceFeatures<RpBåndleggingGrense, RpBåndleggingSone>(document),
                () => MapCurveAndSurfaceFeatures<RpDetaljeringGrense, RpDetaljeringSone>(document),
                () => MapCurveAndSurfaceFeatures<RpFareGrense, RpFareSone>(document),
                () => MapCurveAndSurfaceFeatures<RpGjennomføringGrense, RpGjennomføringSone>(document),
                () => MapCurveAndSurfaceFeatures<RpInfrastrukturGrense, RpInfrastrukturSone>(document),
                () => MapCurveAndSurfaceFeatures<RpSikringGrense, RpSikringSone>(document),
                () => MapCurveAndSurfaceFeatures<RpStøyGrense, RpStøySone>(document),
                () => MapCurveFeatures<RpRegulertHøyde>(document),
                () => MapCurveFeatures<RpJuridiskLinje>(document),
                () => MapPointFeatures<RpJuridiskPunkt>(document),
                () => MapPointFeatures<RpPåskrift>(document)
            });
        }
    }
}

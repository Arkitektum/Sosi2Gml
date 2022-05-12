using Microsoft.AspNetCore.Http;
using Sosi2Gml.Application.Mappers;
using Sosi2Gml.Application.Models.Config;
using Sosi2Gml.Application.Services.Sosi2Gml;
using Sosi2Gml.Reguleringsplanforslag.Constants;
using Sosi2Gml.Reguleringsplanforslag.Mappers;
using Sosi2Gml.Reguleringsplanforslag.Models;

namespace Sosi2Gml.Reguleringsplanforslag.Services
{
    public class RpfSosi2GmlService : Sosi2GmlService, IRpfSosi2GmlService
    {
        private readonly IGmlMapper _gmlMapper;
        private readonly IArealplanMapper _arealplanMapper;
        private readonly IRpBestemmelseMidlByggAnleggMapper _midlByggAnleggMapper;
        private readonly DatasetSettings _settings;

        public RpfSosi2GmlService(
            IGmlMapper gmlMapper,
            IArealplanMapper arealplanMapper,
            IRpBestemmelseMidlByggAnleggMapper midlByggAnleggMapper,
            Datasets datasets,
            CrsSettings crsSettings) : base(crsSettings)
        {
            _gmlMapper = gmlMapper;
            _arealplanMapper = arealplanMapper;
            _midlByggAnleggMapper = midlByggAnleggMapper;
            _settings = datasets.GetSettings(Dataset.Reguleringsplanforslag);
        }

        public async Task<MemoryStream> Sosi2GmlAsync(IFormFile sosiFile)
        {
            var document = await ReadSosiFileAsync(sosiFile);

            return await CreateGmlDocumentAsync(document, _settings, new[]
            {
                () => _arealplanMapper.Map(document),
                () => _gmlMapper.MapCurveAndSurfaceFeatures<RpGrense, RpOmråde>(document),
                () => _gmlMapper.MapCurveAndSurfaceFeatures<RpFormålGrense, RpArealformålOmråde>(document),
                () => _gmlMapper.MapCurveAndSurfaceFeatures<RpBestemmelseGrense, RpBestemmelseOmråde>(document),
                () => _gmlMapper.MapCurveAndSurfaceFeatures<RpAngittHensynGrense, RpAngittHensynSone>(document),
                () => _gmlMapper.MapCurveAndSurfaceFeatures<RpBåndleggingGrense, RpBåndleggingSone>(document),
                () => _gmlMapper.MapCurveAndSurfaceFeatures<RpDetaljeringGrense, RpDetaljeringSone>(document),
                () => _gmlMapper.MapCurveAndSurfaceFeatures<RpFareGrense, RpFareSone>(document),
                () => _gmlMapper.MapCurveAndSurfaceFeatures<RpGjennomføringGrense, RpGjennomføringSone>(document),
                () => _gmlMapper.MapCurveAndSurfaceFeatures<RpInfrastrukturGrense, RpInfrastrukturSone>(document),
                () => _gmlMapper.MapCurveAndSurfaceFeatures<RpSikringGrense, RpSikringSone>(document),
                () => _gmlMapper.MapCurveAndSurfaceFeatures<RpStøyGrense, RpStøySone>(document),
                () => _gmlMapper.MapCurveFeatures<RpRegulertHøyde>(document),
                () => _gmlMapper.MapCurveFeatures<RpJuridiskLinje>(document),
                () => _gmlMapper.MapPointFeatures<RpJuridiskPunkt>(document),
                () => _gmlMapper.MapPointFeatures<RpPåskrift>(document),
                () => _midlByggAnleggMapper.Map(document)
            });
        }
    }
}

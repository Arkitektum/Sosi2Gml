using Microsoft.AspNetCore.Mvc;
using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Constants;
using Sosi2Gml.Reguleringsplanforslag.Mappers;
using Sosi2Gml.Reguleringsplanforslag.Mappers.Interfaces;
using Sosi2Gml.Reguleringsplanforslag.Models;
using System.Text;
using System.Text.RegularExpressions;
using Sosi2Gml.Application.Constants;
using Sosi2Gml.Application.Helpers;

namespace Sosi2Gml.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static Regex _sosiObjectRegex = new(@"^\.[A-ZÆØÅ]+", RegexOptions.Compiled);

        private readonly IGmlCurveFeatureMapper<RpGrense> _rpGrenseMapper;
        private readonly IGmlCurveFeatureMapper<RpFormålGrense> _rpFormålGrenseMapper;
        private readonly IGmlFeatureMapper<RpSikringGrense> _rpSikringGrenseMapper;
        private readonly IGmlSurfaceFeatureMapper<RpOmråde, RpGrense> _rpOmrådeMapper;
        private readonly IGmlSurfaceFeatureMapper<RpArealformålOmråde, RpFormålGrense> _rpArealformålOmrådeMapper;
        private readonly IGmlSurfaceFeatureMapper<RpSikringSone, RpSikringGrense> _rpSikringSoneMapper;
        private readonly IGmlFeatureMapper<RpJuridiskPunkt> _rpJuridiskPunktMapper;
        private readonly IServiceProvider _serviceProvider;

        public TestController(
             IGmlCurveFeatureMapper<RpGrense> rpGrenseMapper,
             IGmlCurveFeatureMapper<RpFormålGrense> rpFormålGrenseMapper,
             //IGmlFeatureMapper<RpSikringGrense> rpSikringGrenseMapper,
             IGmlSurfaceFeatureMapper<RpOmråde, RpGrense> rpOmrådeMapper,
             IGmlSurfaceFeatureMapper<RpArealformålOmråde, RpFormålGrense> rpArealformålOmrådeMapper,
             IGmlSurfaceFeatureMapper<RpSikringSone, RpSikringGrense> rpSikringSoneMapper,
             IGmlFeatureMapper<RpJuridiskPunkt> rpJuridiskPunktMapper,
             IServiceProvider serviceProvider)
        {
            _rpGrenseMapper = rpGrenseMapper;
            _rpFormålGrenseMapper = rpFormålGrenseMapper;
           // _rpSikringGrenseMapper = rpSikringGrenseMapper;
            _rpOmrådeMapper = rpOmrådeMapper;
            _rpArealformålOmrådeMapper = rpArealformålOmrådeMapper;
            _rpJuridiskPunktMapper = rpJuridiskPunktMapper;
            _rpSikringSoneMapper = rpSikringSoneMapper;
            _serviceProvider = serviceProvider;
        }

        private T Map<T>(SosiObject sosiObject) where T : Feature, new()
        {
            return new T();
        }

        public List<TOutput> ConvertAll<TOutput>(Dictionary<string, List<SosiObject>> sosiObjects, Converter<SosiObject, TOutput> converter) 
        {
            return null;
        }

        public void MapCurveFeatures<TCurveFeature>(SosiDocument document, List<Feature> features)
            where TCurveFeature : CurveFeature
        {
            var curveObjects = document.GetSosiObjects<TCurveFeature>();

            var curveFeatures = curveObjects
                .ConvertAll(sosiObject => Activator.CreateInstance(typeof(TCurveFeature), new object[] { sosiObject, document.SrsName, document.DecimalCount }) as TCurveFeature);

            features.AddRange(curveFeatures);
        }

        public void MapPointFeatures<TPointFeature>(SosiDocument document, List<Feature> features)
            where TPointFeature : PointFeature
        {
            var pointObjects = document.GetSosiObjects<TPointFeature>();

            var pointFeatures = pointObjects
                .ConvertAll(sosiObject => Activator.CreateInstance(typeof(TPointFeature), new object[] { sosiObject, document.SrsName, document.DecimalCount }) as TPointFeature);

            features.AddRange(pointFeatures);
        }

        public void MapCurveAndSurfaceFeatures<TCurveFeature, TSurfaceFeature>(SosiDocument document, List<Feature> features)
            where TCurveFeature : CurveFeature
            where TSurfaceFeature : SurfaceFeature
        {
            var curveObjects = document.GetSosiObjects<TCurveFeature>();
            var surfaceObjects = document.GetSosiObjects<TSurfaceFeature>();

            var curveFeatures = curveObjects
                .ConvertAll(sosiObject => Activator.CreateInstance(typeof(TCurveFeature), new object[] { sosiObject, document.SrsName, document.DecimalCount }) as TCurveFeature);

            var surfaceFeatures = surfaceObjects
                .ConvertAll(sosiObject => Activator.CreateInstance(typeof(TSurfaceFeature), new object[] { sosiObject, document.SrsName, document.DecimalCount, curveFeatures }) as TSurfaceFeature);

            features.AddRange(curveFeatures);
            features.AddRange(surfaceFeatures);
        }

        [HttpPost]
        public async Task<IActionResult> Sosi2Gml(IFormFile sosiFile)
        {
            const string SrsName = "http://www.opengis.net/def/crs/EPSG/0/25832";
            const int DecimalPlaces = 2;

            var document = await ReadSosiFileAsync(sosiFile);

            /*MapCurveAndSurfaceFeatures(
                document, 
                (sosiObject, srsName, decimalPlaces) => new RpGrense(sosiObject, srsName, decimalPlaces),
                (sosiObject, srsName, decimalPlaces, curveFeatures) => new RpOmråde(sosiObject, srsName, decimalPlaces, curveFeatures)
            );*/

            var s = DateTime.Now;

            var arealplan = new Arealplan(document.GetSosiObjects<RpOmråde>().First());
            var features = new List<Feature> { arealplan };

            MapCurveAndSurfaceFeatures<RpGrense, RpOmråde>(document, features);
            MapCurveAndSurfaceFeatures<RpFormålGrense, RpArealformålOmråde>(document, features);
            MapCurveAndSurfaceFeatures<RpAngittHensynGrense, RpAngittHensynSone>(document, features);
            MapCurveAndSurfaceFeatures<RpBåndleggingGrense, RpBåndleggingSone>(document, features);
            MapCurveAndSurfaceFeatures<RpDetaljeringGrense, RpDetaljeringSone>(document, features);
            MapCurveAndSurfaceFeatures<RpFareGrense, RpFareSone>(document, features);
            MapCurveAndSurfaceFeatures<RpGjennomføringGrense, RpGjennomføringSone>(document, features);
            MapCurveAndSurfaceFeatures<RpInfrastrukturGrense, RpInfrastrukturSone>(document, features);
            MapCurveAndSurfaceFeatures<RpSikringGrense, RpSikringSone>(document, features);
            MapCurveAndSurfaceFeatures<RpStøyGrense, RpStøySone>(document, features);
            MapCurveFeatures<RpJuridiskLinje>(document, features);
            MapCurveFeatures<RpRegulertHøyde>(document, features);
            MapPointFeatures<RpJuridiskPunkt>(document, features);

            var e = s;

            //var hode = sosiObjects.First();

            /*var b = ConvertAll<RpGrense>(sosiDocument, sosiObject => new(sosiObject, SrsName, DecimalPlaces));

            

            //ConvertAll<RpGrense>(sosiObjects, () => new(sosiObject)

            var rpGrenseSosiObjects = sosiDocument[FeatureMemberName.RpGrense];
            var rpGrenser = rpGrenseSosiObjects.ConvertAll(rpGrense => _rpGrenseMapper.Map(rpGrense, SrsName, DecimalPlaces));

            var rpOmrådeSosiObjects = sosiDocument[FeatureMemberName.RpOmråde];
            var rpOmråder = rpOmrådeSosiObjects.ConvertAll<RpOmråde>(sosiObject => new(sosiObject, SrsName, DecimalPlaces, rpGrenser));

            var rpSikringGrenseObjects = sosiDocument["RpSikringGrense"];
            var rpSikringGrenser = rpSikringGrenseObjects.ConvertAll(sosiObject => _rpHensynGrenseMapper.Map<RpSikringGrense>(sosiObject, SrsName, DecimalPlaces));

            var rpSikringSoneObjects = sosiDocument["RpSikringSone"];
            var rpSikringSoner = rpSikringSoneObjects.ConvertAll(sosiObject => _rpSikringSoneMapper.Map(sosiObject, SrsName, DecimalPlaces, rpSikringGrenser));

            var g = rpSikringSoner;

            /*var rpJuridiskPunktObjects = sosiObjects[FeatureMemberName.RpJuridiskPunkt];
            var rpJuridiskePunkt = rpJuridiskPunktObjects.ConvertAll(sosiObject => _rpJuridiskPunktMapper.Map(sosiObject, SrsName, DecimalPlaces));

            var rpGrenseSosiObjects = sosiObjects[FeatureMemberName.RpGrense];
            var rpGrenser = rpGrenseSosiObjects.ConvertAll(rpGrense => _rpGrenseMapper.Map(rpGrense, SrsName, DecimalPlaces));

            var rpOmrådeSosiObjects = sosiObjects[FeatureMemberName.RpOmråde];
            var rpOmråder = rpOmrådeSosiObjects.ConvertAll(rpOmråde => _rpOmrådeMapper.Map(rpOmråde, SrsName, DecimalPlaces, rpGrenser));

            var rpFormålGrenseSosiObjects = sosiObjects[FeatureMemberName.RpFormålGrense];
            var rpFormålGrenser = rpFormålGrenseSosiObjects.ConvertAll(rpFormålGrense => _rpFormålGrenseMapper.Map(rpFormålGrense, SrsName, DecimalPlaces));

            var rpArealformålOmrådeSosiObjects = sosiObjects[FeatureMemberName.RpArealformålOmråde];
            var rpArealformålOmråder = rpArealformålOmrådeSosiObjects.ConvertAll(rpArealformålOmråde => _rpArealformålOmrådeMapper.Map(rpArealformålOmråde, SrsName, DecimalPlaces, rpFormålGrenser));

            var rpSikringGrenseObjects = sosiObjects["RpSikringGrense"];
            var rpSikringGrenser = rpSikringGrenseObjects.ConvertAll(sosiObject => _rpSikringGrenseMapper.Map(sosiObject, SrsName, DecimalPlaces));

            var rpSikringSoneObjects = sosiObjects["RpSikringSone"];
            var rpSikringSoner = rpSikringGrenseObjects.ConvertAll(sosiObject => _rpSikringSoneMapper.Map(sosiObject, SrsName, DecimalPlaces, rpSikringGrenser));

            var b = rpArealformålOmråder.First();*/

            return Ok();
        }
      
        public class SosiDocument
        {
            private static readonly Dictionary<string, string> _srsNames = new()
            {
                { "22", "http://www.opengis.net/def/crs/EPSG/0/25832" },
                { "23", "http://www.opengis.net/def/crs/EPSG/0/25833" },
                { "25", "http://www.opengis.net/def/crs/EPSG/0/25835" }
            };

            private SosiDocument()
            {
            }

            public string SrsName { get; set; }
            public int DecimalCount { get; set; }
            public Envelope Envelope { get; set; }
            public string SosiVersion { get; set; }
            public Dictionary<string, List<SosiObject>> SosiObjects { get; set; }

            public List<SosiObject> GetSosiObjects<T>() where T : Feature
            {
                var objectName = MapperHelper.GetSosiObjectName<T>();

                if (SosiObjects.TryGetValue(objectName, out var sosiObjects))
                    return sosiObjects;

                return new();
            }

            public static SosiDocument Create(Dictionary<string, List<string>> sosiLines)
            {
                var sosiObjects = sosiLines
                    .Select(kvp => SosiObject.Create(kvp.Key, kvp.Value))
                    .GroupBy(sosiObject => sosiObject.GetValue("..OBJTYPE") ?? sosiObject.ElementType)
                    .ToDictionary(grouping => grouping.Key, grouping => grouping.Select(sosiObject => sosiObject).ToList());

                var head = sosiObjects["HODE"].SingleOrDefault();
                var code = head.GetValue("...KOORDSYS");

                if (!_srsNames.TryGetValue(code, out var srsName))
                    return null;

                var unit = head.GetValue("...ENHET");
                var minNorthEasth = head.GetValue("...MIN-NØ");
                var maxNorthEasth = head.GetValue("...MAX-NØ");
                var sosiVersion = head.GetValue("..SOSI-VERSJON");

                sosiObjects.Remove("HODE");

                return new SosiDocument
                {
                    SrsName = srsName,
                    DecimalCount = unit.Length - 1 - unit.IndexOf('.'),
                    Envelope = Envelope.Create(minNorthEasth, maxNorthEasth),
                    SosiVersion = sosiVersion,
                    SosiObjects = sosiObjects
                };
            }
        }

        private static async Task<SosiDocument> ReadSosiFileAsync(IFormFile sosiFile)
        {
            using var streamReader = new StreamReader(sosiFile.OpenReadStream(), Encoding.UTF8);
            var sosiLines = new Dictionary<string, List<string>>();
            string line;
            
            while ((line = await streamReader.ReadLineAsync()) != null)
            {
                if (_sosiObjectRegex.IsMatch(line) && !line.Equals(".SLUTT"))
                    sosiLines.Add(line, new List<string>());
                else
                    sosiLines.Last().Value.Add(line);
            }

            return SosiDocument.Create(sosiLines);
        }
    }
}
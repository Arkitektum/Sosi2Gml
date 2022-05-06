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
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.GmlHelper;

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

        public void MapFeature<TFeature>(SosiDocument document, Func<SosiDocument, TFeature> func, List<Feature> features)
            where TFeature : Feature
        {
            var feature = func.Invoke(document);
            features.Add(feature);
        }

        private static readonly XNamespace _xmlNs = "http://www.w3.org/2001/XMLSchema";
        private static readonly XNamespace _gmlNs = "http://www.opengis.net/gml/3.2";
        private static readonly XNamespace _appNs = "http://skjema.geonorge.no/SOSI/produktspesifikasjon/Reguleringsplanforslag/5.0";
        private static readonly XNamespace _scNs = "http://www.interactive-instruments.de/ShapeChange/AppInfo";
        private static readonly XNamespace _xlinkNs = "http://www.w3.org/1999/xlink";
        private static readonly XNamespace _xsiNs = "http://www.w3.org/2001/XMLSchema-instance";
        private static readonly string _schemaLocation = "https://skjema.geonorge.no/SOSITEST/produktspesifikasjon/Reguleringsplanforslag/5.0/reguleringsplanforslag-5.0_rev20211104.xsd";

        private static async Task<MemoryStream> CreateGmlDocument(SosiDocument sosiDocument, List<Feature> features)
        {
            var document = new XDocument(new XDeclaration("1.0", "utf-8", null));

            var featureCollection = new XElement(_gmlNs + "FeatureCollection",
                new XAttribute(XNamespace.Xmlns + "gml", _gmlNs),
                new XAttribute(XNamespace.Xmlns + "app", _appNs),
                new XAttribute(XNamespace.Xmlns + "sc", _scNs),
                new XAttribute("xmlns", _xmlNs),
                new XAttribute(XNamespace.Xmlns + "xlink", _xlinkNs),
                new XAttribute(XNamespace.Xmlns + "xsi", _xsiNs),
                new XAttribute(_xsiNs + "schemaLocation", $"{_appNs} {_schemaLocation}"),
                new XAttribute(_gmlNs + "id", CreateGmlId())
            );

            var envelope = CreateEnvelope(sosiDocument.Envelope, sosiDocument.SrsName);

            featureCollection.Add(envelope);

            foreach (var feature in features)
            {
                var element = feature.ToGml();

                if (element != null)
                    featureCollection.Add(new XElement(_gmlNs + "featureMember", element));
            }

            document.Add(featureCollection);

            var memoryStream = new MemoryStream();
            await document.SaveAsync(memoryStream, SaveOptions.None, default);
            memoryStream.Position = 0;

            return memoryStream;
        }

        [HttpPost]
        public async Task<IActionResult> Sosi2Gml(IFormFile sosiFile)
        {
            var document = await ReadSosiFileAsync(sosiFile);
            var features = new List<Feature>();

            MapFeature<Arealplan>(document, document => new(document.GetSosiObjects<RpOmråde>().First()), features);
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

            features.ForEach(feature => feature.AddAssociations(features));

            var memoryStream = await CreateGmlDocument(document, features);

            return File(memoryStream, "application/xml+gml", $"{Path.GetFileNameWithoutExtension(sosiFile.FileName)}.gml");
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
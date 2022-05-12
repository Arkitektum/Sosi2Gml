using Microsoft.AspNetCore.Http;
using Sosi2Gml.Application.Models.Config;
using Sosi2Gml.Application.Models.Features;
using Sosi2Gml.Application.Models.Sosi;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.GmlHelper;

namespace Sosi2Gml.Application.Services.Sosi2Gml
{
    public abstract class Sosi2GmlService
    {
        private static readonly XNamespace _xmlNs = "http://www.w3.org/2001/XMLSchema";
        private static readonly XNamespace _xsiNs = "http://www.w3.org/2001/XMLSchema-instance";
        private static readonly XNamespace _gmlNs = "http://www.opengis.net/gml/3.2";
        private static readonly Regex _sosiObjectRegex = new(@"^\.[A-ZÆØÅ]+", RegexOptions.Compiled);

        private readonly CrsSettings _crsSettings;

        public Sosi2GmlService(
            CrsSettings crsSettings)
        {
            _crsSettings = crsSettings;
        }

        protected async Task<SosiDocument> ReadSosiFileAsync(IFormFile sosiFile)
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

            return CreateSosiDocument(sosiLines);
        }

        protected static async Task<MemoryStream> CreateGmlDocumentAsync(SosiDocument sosiDocument, DatasetSettings settings, Func<List<Feature>>[] mappingActions)
        {
            var features = await RunMappingActionsAsync(mappingActions);

            Parallel.ForEach(features, feature => feature.AddAssociations(features));

            return await CreateGmlDocumentAsync(sosiDocument, settings, features);
        }



        private static async Task<MemoryStream> CreateGmlDocumentAsync(SosiDocument sosiDocument, DatasetSettings settings, List<Feature> features)
        {
            var document = new XDocument(new XDeclaration("1.0", "utf-8", null));

            var featureCollection = new XElement(_gmlNs + "FeatureCollection",
                new XAttribute("xmlns", _xmlNs),
                new XAttribute(XNamespace.Xmlns + "xsi", _xsiNs),
                new XAttribute(XNamespace.Xmlns + "gml", _gmlNs)
            );

            foreach (var ns in settings.Namespaces)
                featureCollection.Add(new XAttribute(XNamespace.Xmlns + ns.Prefix, ns.Uri));

            featureCollection.Add(new XAttribute(_xsiNs + "schemaLocation", $"{settings.SchemaLocation.Namespace} {settings.SchemaLocation.Location}"));
            featureCollection.Add(new XAttribute(_gmlNs + "id", CreateGmlId()));

            var envelope = CreateEnvelope(sosiDocument.Envelope, sosiDocument.SrsName);

            featureCollection.Add(envelope);

            foreach (var feature in features)
            {
                var element = feature.ToGml(settings.SchemaLocation.Namespace);

                if (element != null)
                    featureCollection.Add(new XElement(_gmlNs + "featureMember", element));
            }

            document.Add(featureCollection);

            var memoryStream = new MemoryStream();
            await document.SaveAsync(memoryStream, SaveOptions.None, default);
            memoryStream.Position = 0;

            return memoryStream;
        }

        private SosiDocument CreateSosiDocument(Dictionary<string, List<string>> sosiLines)
        {
            var sosiObjects = sosiLines
                .Select(kvp => SosiObject.Create(kvp.Key, kvp.Value))
                .GroupBy(sosiObject => sosiObject.GetValue("..OBJTYPE") ?? sosiObject.ElementType)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.Select(sosiObject => sosiObject).ToList());

            var head = sosiObjects["HODE"].SingleOrDefault();
            var code = head.GetValue("...KOORDSYS");
            var srsName = _crsSettings.GetSrsName(code);

            if (srsName == null)
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

        private static async Task<List<Feature>> RunMappingActionsAsync(Func<List<Feature>>[] mappingActions)
        {
            var tasks = new List<Task<List<Feature>>>();
            var features = new List<Feature>();

            foreach (var action in mappingActions)
                tasks.Add(Task.Run(action));

            await Task.WhenAll(tasks);

            foreach (var task in tasks)
                features.AddRange(task.Result);

            return features;
        }
    }
}

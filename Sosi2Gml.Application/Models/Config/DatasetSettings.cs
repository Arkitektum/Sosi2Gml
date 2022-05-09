namespace Sosi2Gml.Application.Models.Config
{
    public class DatasetSettings
    {
        public SchemaLocationSettings SchemaLocation { get; set; } = new();
        public List<NamespaceSettings> Namespaces { get; set; } = new();
    }
}

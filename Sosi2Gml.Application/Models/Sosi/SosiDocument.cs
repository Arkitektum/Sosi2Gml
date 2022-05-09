using Sosi2Gml.Application.Helpers;
using Sosi2Gml.Application.Models.Features;

namespace Sosi2Gml.Application.Models.Sosi
{
    public class SosiDocument
    {
        public string SrsName { get; set; }
        public int DecimalCount { get; set; }
        public Envelope Envelope { get; set; }
        public string SosiVersion { get; set; }
        public Dictionary<string, List<SosiObject>> SosiObjects { get; set; }

        public List<SosiObject> GetSosiObjects<T>() where T : Feature
        {
            var objectName = SosiHelper.GetSosiObjectName<T>();

            if (SosiObjects.TryGetValue(objectName, out var sosiObjects))
                return sosiObjects;

            return new();
        }
    }
}

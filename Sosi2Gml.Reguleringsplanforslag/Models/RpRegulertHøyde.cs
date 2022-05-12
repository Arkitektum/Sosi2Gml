using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Features;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.GeometryHelper;
using static Sosi2Gml.Application.Helpers.GmlHelper;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpRegulertHøyde")]
    public class RpRegulertHøyde : CurveFeature
    {
        public RpRegulertHøyde(SosiObject sosiObject, string srsName, int decimalPlaces) : 
            base(sosiObject, srsName, decimalPlaces)
        {
            RegulertHøyde = sosiObject.GetValue("...REGULERTHØYDE");
            Høydereferansesystem = sosiObject.GetValue("...HØYDE-REF") ?? "NN2000";
        }

        public override string FeatureName => "RpRegulertHøyde";
        public string RegulertHøyde { get; set; }
        public string Høydereferansesystem { get; set; }
        public string TypeHøyde { get; } = "GH";
        public RpOmråde Planområde { get; set; }
        public List<RpPåskrift> Påskrifter { get; set; } = new();

        public override void AddAssociations(List<Feature> features)
        {
            Planområde = GetClosestFeature<RpOmråde>(features, Geometry);

            if (Planområde != null)
                Planområde.RegulerteHøyder.Add(this);
        }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);
            
            featureMember.Add(new XElement(appNs + "senterlinje", GeomElement));

            featureMember.Add(new XElement(appNs + "høydeFraPlanbestemmelse",
                new XElement(appNs + "HøydeFraPlanbestemmelse",
                    new XElement(appNs + "regulerthøyde", RegulertHøyde),
                    new XElement(appNs + "vertikalReferanse",
                        new XElement(appNs + "VertikalReferanse",
                            new XElement(appNs + "høydereferansesystem", Høydereferansesystem)
                        )
                    )
                )
            ));

            featureMember.Add(new XElement(appNs + "typeHøyde", TypeHøyde));

            featureMember.Add(new XComment("Pga. manglende kompabilitet mellom SOSI 4.5 og SOSI 5.0, settes \"typeHøyde\" til \"GH\" (gesimshøyde). Tilgjengelige valg er: GH, MH, TH, BH og PH."));

            if (Planområde != null)
                featureMember.Add(CreateXLink(appNs + "planområde", Planområde.GmlId));

            foreach (var påskrift in Påskrifter)
                featureMember.Add(CreateXLink(appNs + "påskrift", påskrift.GmlId));

            return featureMember;
        }
    }
}

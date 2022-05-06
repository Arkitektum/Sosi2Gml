using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

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

        public override void AddAssociations(List<Feature> features)
        {
            var planområder = features.OfType<RpOmråde>().ToList();

            if (planområder.Count == 1)
                Planområde = planområder.First();
            else
                Planområde = planområder.FirstOrDefault(planområde => planområde.Geometry?.Intersects(Geometry) ?? false);

            if (Planområde != null)
                Planområde.RegulertHøyde.Add(this);
        }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);
            
            featureMember.Add(new XElement(AppNs + "senterlinje", GeomElement));

            featureMember.Add(new XElement(AppNs + "høydeFraPlanbestemmelse",
                new XElement(AppNs + "HøydeFraPlanbestemmelse",
                    new XElement(AppNs + "regulerthøyde", RegulertHøyde),
                    new XElement(AppNs + "vertikalReferanse",
                        new XElement(AppNs + "VertikalReferanse",
                            new XElement(AppNs + "høydereferansesystem", Høydereferansesystem)
                        )
                    )
                )
            ));

            featureMember.Add(new XElement(AppNs + "typeHøyde", TypeHøyde));

            featureMember.Add(new XComment("Pga. manglende kompabilitet mellom SOSI 4.5 og SOSI 5.0, settes \"typeHøyde\" til \"GH\" (gesimshøyde). Tilgjengelige valg er: GH, MH, TH, BH og PH."));

            if (Planområde != null)
                featureMember.Add(CreateXLink(AppNs + "planområde", Planområde.GmlId));

            return featureMember;
        }
    }
}

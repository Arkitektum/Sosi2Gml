using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.GeometryHelper;
using Sosi2Gml.Application.Models.Features;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpPåskrift")]
    public class RpPåskrift : PointFeature
    {
        public RpPåskrift(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
            Tekststreng = sosiObject.GetValue("..STRENG");
            PåskriftType = sosiObject.GetValue("..RPPÅSKRIFTTYPE");
        }

        public override string FeatureName => "RpPåskrift";
        public string Tekststreng { get; set; }
        public string PåskriftType { get; set; }
        public RpOmråde Planområde { get; set; }
        public RpArealformålOmråde Formål { get; set; }
        public RpHensynSone Hensyn { get; set; }
        public RpBestemmelseOmråde BestemmelseOmråde { get; set; }
        public RpBestemmelseMidlByggAnlegg MidlByggAnlegg { get; set; }
        public RpRegulertHøyde RegulertHøyde { get; set; }
        public RpJuridiskLinje JuridiskLinje { get; set; }

        public override void AddAssociations(List<Feature> features)
        {
            switch (PåskriftType)
            {
                case "1" or "3" or "5":
                    Formål = GetClosestFeature<RpArealformålOmråde>(features, Geometry);

                    if (Formål != null)
                    {
                        Formål.Påskrifter.Add(this);
                        break;
                    }

                    Planområde = GetClosestFeature<RpOmråde>(features, Geometry);

                    if (Planområde != null)
                        Planområde.Påskrifter.Add(this);

                    break;
                case "2":
                    BestemmelseOmråde = GetClosestFeature<RpBestemmelseOmråde>(features, Geometry);

                    if (BestemmelseOmråde != null)
                    {
                        BestemmelseOmråde.Påskrifter.Add(this);
                        break;
                    }

                    Hensyn = GetClosestFeature<RpHensynSone>(features, Geometry);

                    if (Hensyn != null)
                    {
                        Hensyn.Påskrifter.Add(this);
                        break;
                    }

                    Formål = GetClosestFeature<RpArealformålOmråde>(features, Geometry);

                    if (Formål != null)
                    {
                        Formål.Påskrifter.Add(this);
                        break;
                    }

                    Planområde = GetClosestFeature<RpOmråde>(features, Geometry);

                    if (Planområde != null)
                        Planområde.Påskrifter.Add(this);

                    break;
                case "4" or "6" or "7" or "8" or "9":
                    Planområde = GetClosestFeature<RpOmråde>(features, Geometry);

                    if (Planområde != null)
                        Planområde.Påskrifter.Add(this);

                    break;
                default:
                    break;
            }
        }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "posisjon", GeomElement));
            featureMember.Add(new XElement(appNs + "tekstplassering", CreateLineString(Points.Skip(1), $"{GmlId}-1", SrsName)));
            featureMember.Add(new XElement(appNs + "tekststreng", Tekststreng));
            featureMember.Add(new XElement(appNs + "påskriftType", PåskriftType));

            if (Planområde != null)
                featureMember.Add(CreateXLink(appNs + "planområde", Planområde.GmlId));

            if (Formål != null)
                featureMember.Add(CreateXLink(appNs + "formål", Formål.GmlId));

            if (Hensyn != null)
                featureMember.Add(CreateXLink(appNs + "hensyn", Hensyn.GmlId));

            if (BestemmelseOmråde != null)
                featureMember.Add(CreateXLink(appNs + "bestemmelseOmråde", BestemmelseOmråde.GmlId));

            if (MidlByggAnlegg != null)
                featureMember.Add(CreateXLink(appNs + "midlByggAnlegg", MidlByggAnlegg.GmlId));

            if (RegulertHøyde != null)
                featureMember.Add(CreateXLink(appNs + "regulertHøyde", RegulertHøyde.GmlId));

            if (JuridiskLinje != null)
                featureMember.Add(CreateXLink(appNs + "juridiskLinje", JuridiskLinje.GmlId));

            return featureMember;
        }
    }
}

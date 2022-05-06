using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.GmlHelper;

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
            if (PåskriftType == "1" || PåskriftType == "4" || PåskriftType == "6" || PåskriftType == "9")
            {
                var planområder = features.OfType<RpOmråde>().ToList();

                if (planområder.Count == 1)
                    Planområde = planområder.First();
                else
                    Planområde = planområder.FirstOrDefault(planområde => planområde.Geometry?.Intersects(Geometry) ?? false);

                if (Planområde != null)
                    Planområde.Påskrifter.Add(this);

                return;
            }
            
            if (PåskriftType == "2")
            {
                /*var bestemmelseOmråder = features.OfType<RpBestemmelseOmråde>().ToList();
                BestemmelseOmråde = bestemmelseOmråder.SingleOrDefault(bestemmelseOmråde => bestemmelseOmråde.Geometry.Intersects(Geometry));

                if (BestemmelseOmråde != null)
                {
                    BestemmelseOmråde.Påskrifter.Add(this);
                    return;
                }*/

                var hensynSoner = features.OfType<RpHensynSone>().ToList();
                Hensyn = hensynSoner.FirstOrDefault(hensynSone => hensynSone.HensynSonenavn == Tekststreng);

                if (Hensyn != null)
                {
                    Hensyn.Påskrifter.Add(this);
                    return;
                }

                var formålOmråder = features.OfType<RpArealformålOmråde>().ToList();
                Formål = formålOmråder.FirstOrDefault(formålOmråde => formålOmråde.Feltnavn == Tekststreng);

                if (Formål != null)
                    Formål.Påskrifter.Add(this);

                return;
            }

            if (PåskriftType == "3" || PåskriftType == "5")
            {
                var formålOmråder = features.OfType<RpArealformålOmråde>().ToList();
                Formål = formålOmråder.FirstOrDefault(formålOmråde => formålOmråde.Geometry?.Intersects(Geometry) ?? false);

                if (Formål != null)
                    Formål.Påskrifter.Add(this);

                return;
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

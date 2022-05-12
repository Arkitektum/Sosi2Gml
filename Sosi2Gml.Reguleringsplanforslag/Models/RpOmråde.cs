using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Features;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Helpers.GmlHelper;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpOmråde")]
    public class RpOmråde : SurfaceFeature
    {
        public RpOmråde(
            SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpGrense> rpGrenser) : base(sosiObject, srsName, decimalPlaces, rpGrenser)
        {
            Vertikalnivå = sosiObject.GetValue("..VERTNIV");
            AvgrensesAv.AddRange(Surface.GetFeatures<RpGrense>());
        }

        public string Vertikalnivå { get; set; }
        public List<RpGrense> AvgrensesAv { get; set; } = new();
        public Arealplan Arealplan { get; set; }
        public List<RpBestemmelseMidlByggAnlegg> MidlByggAnlegg { get; set; } = new();
        public List<RpPåskrift> Påskrifter { get; set; } = new();
        public List<RpArealformålOmråde> Formål { get; set; } = new();
        public List<RpRegulertHøyde> RegulerteHøyder { get; set; } = new();
        public List<RpHensynSone> Hensyn { get; set; } = new();
        public List<RpJuridiskPunkt> JuridiskePunkt { get; set; } = new();
        public List<RpJuridiskLinje> JuridiskeLinjer { get; set; } = new();
        public List<RpBestemmelseOmråde> BestemmelseOmråder { get; set; } = new();

        public override string FeatureName => "RpOmråde";

        public override void AddAssociations(List<Feature> features)
        {
            Arealplan = features.OfType<Arealplan>().SingleOrDefault();

            if (Arealplan != null)
                Arealplan.RpOmråder.Add(this);
        }

        public override XElement ToGml(XNamespace appNs)
        {
            var featureMember = base.ToGml(appNs);

            featureMember.Add(new XElement(appNs + "område", GeomElement));
            featureMember.Add(new XElement(appNs + "vertikalnivå", Vertikalnivå));

            if (AvgrensesAv.Any())
                featureMember.Add(AvgrensesAv.Select(formålGrense => CreateXLink(appNs + "avgrensesAv", formålGrense.GmlId)));

            if (Arealplan != null)
                featureMember.Add(CreateXLink(appNs + "arealplan", Arealplan.GmlId));

            if (MidlByggAnlegg.Any())
                featureMember.Add(MidlByggAnlegg.Select(midlByggAnlegg => CreateXLink(appNs + "midlByggAnlegg", midlByggAnlegg.GmlId)));

            if (Påskrifter.Any())
                featureMember.Add(Påskrifter.Select(påskrift => CreateXLink(appNs + "påskrift", påskrift.GmlId)));

            if (Formål.Any())
                featureMember.Add(Formål.Select(formål => CreateXLink(appNs + "formål", formål.GmlId)));

            if (RegulerteHøyder.Any())
                featureMember.Add(RegulerteHøyder.Select(regulertHøyde => CreateXLink(appNs + "regulertHøyde", regulertHøyde.GmlId)));

            if (Hensyn.Any())
                featureMember.Add(Hensyn.Select(hensyn => CreateXLink(appNs + "hensyn", hensyn.GmlId)));

            if (JuridiskePunkt.Any())
                featureMember.Add(JuridiskePunkt.Select(juridiskPunkt => CreateXLink(appNs + "juridiskPunkt", juridiskPunkt.GmlId)));

            if (JuridiskeLinjer.Any())
                featureMember.Add(JuridiskeLinjer.Select(juridiskLinje => CreateXLink(appNs + "juridiskLinje", juridiskLinje.GmlId)));

            if (BestemmelseOmråder.Any())
                featureMember.Add(BestemmelseOmråder.Select(bestemmelseOmråde => CreateXLink(appNs + "bestemmelseOmråde", bestemmelseOmråde.GmlId)));

            return featureMember;
        }
    }
}

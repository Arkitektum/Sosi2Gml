using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpOmråde")]
    public class RpOmråde : SurfaceFeature
    {
        public RpOmråde(
            SosiObject sosiObject, string srsName, int decimalPlaces, IEnumerable<RpGrense> rpGrenser) : base(sosiObject, srsName, decimalPlaces, rpGrenser)
        {
            Vertikalnivå = sosiObject.GetValue("..VERTNIV");
        }

        public string Vertikalnivå { get; set; }
        public List<RpGrense> AvgrensesAv { get; set; } = new();
        public Arealplan Arealplan { get; set; }
        public List<RpBestemmelseMidlByggAnlegg> MidlByggAnlegg { get; set; } = new();
        public List<RpPåskrift> Påskrifter { get; set; } = new();
        public List<RpArealformålOmråde> Formål { get; set; } = new();
        public List<RpRegulertHøyde> RegulertHøyde { get; set; } = new();
        public List<RpHensynSone> Hensyn { get; set; } = new();
        public List<RpJuridiskPunkt> JuridiskPunkt { get; set; } = new();
        public List<RpJuridiskLinje> JuridiskLinje { get; set; } = new();
        public List<RpBestemmelseOmråde> BestemmelseOmråde { get; set; } = new();

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

            featureMember.Add(new XElement(AppNs + "område", GeomElement));
            featureMember.Add(new XElement(AppNs + "vertikalnivå", Vertikalnivå));

            if (AvgrensesAv.Any())
                featureMember.Add(AvgrensesAv.Select(formålGrense => CreateXLink(AppNs + "avgrensesAv", formålGrense.GmlId)));

            if (Arealplan != null)
                featureMember.Add(CreateXLink(AppNs + "arealplan", Arealplan.GmlId));

            if (MidlByggAnlegg.Any())
                featureMember.Add(MidlByggAnlegg.Select(midlByggAnlegg => CreateXLink(AppNs + "midlByggAnlegg", midlByggAnlegg.GmlId)));

            if (Påskrifter.Any())
                featureMember.Add(Påskrifter.Select(påskrift => CreateXLink(AppNs + "påskrift", påskrift.GmlId)));

            if (Formål.Any())
                featureMember.Add(Formål.Select(formål => CreateXLink(AppNs + "formål", formål.GmlId)));

            if (RegulertHøyde.Any())
                featureMember.Add(RegulertHøyde.Select(regulertHøyde => CreateXLink(AppNs + "regulertHøyde", regulertHøyde.GmlId)));

            if (Hensyn.Any())
                featureMember.Add(Hensyn.Select(hensyn => CreateXLink(AppNs + "hensyn", hensyn.GmlId)));

            if (JuridiskPunkt.Any())
                featureMember.Add(JuridiskPunkt.Select(juridiskPunkt => CreateXLink(AppNs + "juridiskPunkt", juridiskPunkt.GmlId)));

            if (JuridiskLinje.Any())
                featureMember.Add(JuridiskLinje.Select(juridiskLinje => CreateXLink(AppNs + "juridiskLinje", juridiskLinje.GmlId)));

            if (BestemmelseOmråde.Any())
                featureMember.Add(BestemmelseOmråde.Select(bestemmelseOmråde => CreateXLink(AppNs + "bestemmelseOmråde", bestemmelseOmråde.GmlId)));

            return featureMember;
        }
    }
}

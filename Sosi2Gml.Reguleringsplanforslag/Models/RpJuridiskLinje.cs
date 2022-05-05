﻿using Sosi2Gml.Application.Attributes;
using Sosi2Gml.Application.Models.Sosi;
using System.Xml.Linq;
using static Sosi2Gml.Application.Constants.Namespace;
using static Sosi2Gml.Application.Helpers.GmlHelper;
using static Sosi2Gml.Application.Helpers.MapperHelper;
using static Sosi2Gml.Reguleringsplanforslag.Constants.Namespace;

namespace Sosi2Gml.Reguleringsplanforslag.Models
{
    [SosiObjectName("RpJuridiskLinje")]
    public class RpJuridiskLinje : CurveFeature
    {
        public RpJuridiskLinje(SosiObject sosiObject, string srsName, int decimalPlaces) : base(sosiObject, srsName, decimalPlaces)
        {
            JuridiskLinje = sosiObject.GetValue("..RPJURLINJE");
        }

        public string JuridiskLinje { get; set; }
        public RpOmråde Planområde { get; set; }

        public override string FeatureName => "RpJuridiskLinje";

        public override XElement ToGml()
        {
            var featureMember = new XElement(AppNs + FeatureName, new XAttribute(GmlNs + "id", GmlId));

            featureMember.Add(Identifikasjon.ToGml(AppNs));

            if (FørsteDigitaliseringsdato.HasValue)
                featureMember.Add(new XElement(AppNs + "førsteDigitaliseringsdato", FormatDateTime(FørsteDigitaliseringsdato.Value)));

            featureMember.Add(new XElement(AppNs + "oppdateringsdato", FormatDateTime(Oppdateringsdato)));

            if (Kvalitet != null)
                featureMember.Add(Kvalitet.ToGml(AppNs));

            featureMember.Add(new XElement(AppNs + "senterlinje", GeomElement));

            featureMember.Add(new XElement(AppNs + "juridisklinje", JuridiskLinje));

            if (Planområde != null)
                featureMember.Add(CreateXLink(AppNs + "planområde", Planområde.GmlId));

            return featureMember;
        }
    }
}

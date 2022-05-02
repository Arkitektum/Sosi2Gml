﻿using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Application.Mappers
{
    public class IdentifikasjonMapper : IGmlElementMapper<Identifikasjon>
    {
        public Identifikasjon Map(SosiObject sosiObject)
        {
            if (!sosiObject.HasValue("..IDENT"))
                return null;

            return new Identifikasjon
            {
                LokalId = sosiObject.GetValue("...LOKALID"),
                Navnerom = sosiObject.GetValue("...NAVNEROM"),
                VersjonId = sosiObject.GetValue("...VERSJONID"),
            };
        }
    }
}

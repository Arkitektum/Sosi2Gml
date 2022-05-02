using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Application.Mappers
{
    public class IdentifikasjonMapper : IGmlElementMapper<Identifikasjon>
    {
        public Identifikasjon Map(SosiObject sosiObject)
        {
            if (!sosiObject.SosiValues.Has("..IDENT"))
                return null;

            return new Identifikasjon
            {
                LokalId = sosiObject.SosiValues.Get("...LOKALID"),
                Navnerom = sosiObject.SosiValues.Get("...NAVNEROM"),
                VersjonId = sosiObject.SosiValues.Get("...VERSJONID"),
            };
        }
    }
}

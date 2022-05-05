using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Application.Mappers
{
    public class IdentifikasjonMapper : IGmlElementMapper<Identifikasjon>
    {
        public Identifikasjon Map(SosiObject sosiObject)
        {
            if (!sosiObject.HasValue("..IDENT"))
                return null;

            return null;
        }
    }
}

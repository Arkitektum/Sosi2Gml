using Sosi2Gml.Application.Mappers.Interfaces;
using Sosi2Gml.Application.Models.Sosi;

namespace Sosi2Gml.Application.Mappers
{
    public class KvalitetMapper : IGmlElementMapper<Kvalitet>
    {
        public Kvalitet Map(SosiObject sosiObject)
        {
            var kvalitet = sosiObject.GetValue("..KVALITET");

            if (kvalitet == null)
                return null;

            return new Kvalitet
            {
                Målemetode = kvalitet
            };
        }
    }
}

﻿using Sosi2Gml.Application.Models.Sosi;
using Sosi2Gml.Reguleringsplanforslag.Models;

namespace Sosi2Gml.Reguleringsplanforslag.Mappers.Interfaces
{
    public interface IRpFormålGrenseMapper
    {
        RpFormålGrense Map(SosiObject sosiObject);
    }
}

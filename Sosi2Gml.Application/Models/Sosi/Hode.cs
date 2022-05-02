using Sosi2Gml.Application.Models.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sosi2Gml.Application.Models.Sosi
{
    public class Hode
    {
        public Koordinatsystem Koordinatsystem { get; set; }
        public int AntallDesimaler { get; set; }
        public Envelope Område { get; set; }
        public string SosiVersjon { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    public class Rainbow
    {
        public List<Color> Array { get; set; }

        public enum PaintType
        {
            Axial_X, Axial_Y, Radial
        }
        public PaintType Type = PaintType.Axial_X;
    }
}

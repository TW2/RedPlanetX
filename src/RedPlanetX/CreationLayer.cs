using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    public class CreationLayer
    {
        //Base class
    }

    public class GraphicalCL : CreationLayer
    {
        public List<CreationObject> Objects = new List<CreationObject>();
    }

    public class TrajectoryCL : CreationLayer
    {
        public List<GeometryObject> Curves = new List<GeometryObject>();
    }
}

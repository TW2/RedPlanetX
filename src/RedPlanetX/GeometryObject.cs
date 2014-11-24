using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    public interface GeometryObject
    {
        //Base class
    }

    public class LineObject : GeometryObject
    {
        public Point Start { get; set; }
        public Point Stop { get; set; }
    }
    public class BezierObject : GeometryObject
    {
        public Point Start { get; set; }
        public Point CP1 { get; set; }
        public Point CP2 { get; set; }
        public Point Stop { get; set; }
    }

    public class GeometryPoint
    {
        public enum PointType { Start, Stop, CP1, CP2, None };
        public PointType SelectedPoint { get; set; }
        public GeometryObject SelectedObject { get; set; }
        public void Init()
        {
            SelectedPoint = PointType.None;
            SelectedObject = null;
        }
    }
}

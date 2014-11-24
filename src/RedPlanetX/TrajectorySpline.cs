using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    public class TrajectorySpline
    {
        private List<GeometryObject> trajectory = new List<GeometryObject>();

        public void AddLine(Point p1, Point p2)
        {
            LineObject l = new LineObject();
            l.Start = p1;
            l.Stop = p2;
            trajectory.Add(l);
        }

        public void AddBezier(Point p1, Point p2)
        {
            int xdiff = p2.X - p1.X;
            int ydiff = p2.Y - p1.Y;
            int x1_3 = p1.X + xdiff / 3;
            int x2_3 = p1.X + xdiff * 2 / 3;
            int y1_3 = p1.Y + ydiff / 3;
            int y2_3 = p1.Y + ydiff * 2 / 3;
            AddBezier(p1, new Point(x1_3, y1_3), new Point(x2_3, y2_3), p2);
        }

        public void AddBezier(Point p1, Point cp1, Point cp2, Point p2)
        {
            BezierObject b = new BezierObject();
            b.Start = p1;
            b.CP1 = cp1;
            b.CP2 = cp2;
            b.Stop = p2;
            trajectory.Add(b);
        }

        public void AddGeometry(GeometryObject go)
        {
            trajectory.Add(go);
        }

        public List<GeometryObject> GetTrajectory()
        {
            return trajectory;
        }
    }
}

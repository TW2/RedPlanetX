using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    public class PathObject
    {
        Point lastPoint = new Point(0, 0);
        GraphicsPath gp = new GraphicsPath();
        public void Init()
        {
            gp = new GraphicsPath();
            lastPoint = new Point(0, 0);
        }
        public void MoveTo(int x, int y)
        {
            lastPoint = new Point(x, y);
        }
        public void LineTo(int x, int y)
        {
            gp.AddLine(lastPoint, new PointF(x, y));
            lastPoint = new Point(x, y);
        }
        public void BezierTo(int px1, int py1, int px2, int py2, int x, int y)
        {
            gp.AddBezier(lastPoint, new Point(px1, py1), new Point(px2, py2), new Point(x, y));
            lastPoint = new Point(x, y);
        }
        public void Close()
        {
            gp.CloseFigure();
        }
        public GraphicsPath GetPath()
        {
            return gp;
        }
        // Mode de construction par objet
        public GraphicsPath FromArray(List<GeometryObject> array, int translateX, int translateY)
        {
            if (array == null)
            {
                return null;
            }

            Init();
            bool first = true;

            foreach (GeometryObject go in array)
            {
                if (go.GetType() == typeof(LineObject))
                {
                    LineObject l = (LineObject)go;
                    if (first == true)
                    {
                        MoveTo(l.Start.X - translateX, l.Start.Y - translateY);
                        first = false;
                    }
                    LineTo(l.Stop.X - translateX, l.Stop.Y - translateY);
                }
                else if (go.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)go;
                    if (first == true)
                    {
                        MoveTo(b.Start.X - translateX, b.Start.Y - translateY);
                        first = false;
                    }
                    BezierTo(b.CP1.X - translateX, b.CP1.Y - translateY, b.CP2.X - translateX, b.CP2.Y - translateY, b.Stop.X - translateX, b.Stop.Y - translateY);
                }
            }
            Close();
            return gp;
        }

    }
}

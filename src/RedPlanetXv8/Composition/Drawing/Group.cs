using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RedPlanetXv8.Composition.Drawing
{
    public class Group : IGraphicObject
    {
        private List<IGraphicObject> graobjs = new List<IGraphicObject>();
        private GraphicsPath path = new GraphicsPath();

        public Group()
        {

        }

        public void AddLine(Line l)
        {
            graobjs.Add(l);
        }

        public void AddCurve(Curve c)
        {
            graobjs.Add(c);
        }

        public void RemoveObject(IGraphicObject go)
        {
            graobjs.Remove(go);
        }

        public List<IGraphicObject> GetGroup()
        {
            return graobjs;
        }

        private void CreatePath()
        {
            path = new GraphicsPath();
            path.StartFigure();
            foreach (IGraphicObject igo in graobjs)
            {
                if (igo.GetType() == typeof(Line))
                {
                    Line l = (Line)igo;
                    if (l.End.IsEmpty == false)
                    {
                        path.AddLine(l.Start, l.End);
                    }
                }

                if (igo.GetType() == typeof(Curve))
                {
                    Curve c = (Curve)igo;
                    if (c.End.IsEmpty == false)
                    {
                        path.AddBezier(c.Start, c.CP1, c.CP2, c.End);
                    }
                }
            }
            path.CloseFigure();
        }

        public void Draw(Graphics g)
        {
            CreatePath();
            g.FillPath(Brushes.Chartreuse, path);

            foreach (IGraphicObject igo in graobjs)
            {
                igo.Draw(g);
            }
        }

        public static List<Line> GetLinesOfPoint(Point p, Group g)
        {
            List<Line> lines = new List<Line>();

            foreach (IGraphicObject igo in g.GetGroup())
            {
                if (igo.GetType() == typeof(Line))
                {
                    Line l = (Line)igo;
                    if (Line.ContainsPoint(p, l) == true)
                    {
                        lines.Add(l);
                    }
                }
            }
            return lines;
        }

        public static List<Curve> GetCurvesOfPoint(Point p, Group g)
        {
            List<Curve> curves = new List<Curve>();

            foreach (IGraphicObject igo in g.GetGroup())
            {
                if (igo.GetType() == typeof(Curve))
                {
                    Curve c = (Curve)igo;
                    if (Curve.ContainsPoint(p, c) == true)
                    {
                        curves.Add(c);
                    }
                }
            }
            return curves;
        }
    }
}

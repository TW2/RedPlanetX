using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Composition.Drawing
{
    /// <summary>
    /// Cette classe est destinée à être utilisée pour les chemins suivi par les figures.
    /// Ces chemins peuvent être ou non courbés.
    /// Ces chemins peuvent faire ou non une boucle temporelle.
    /// Cette classe est une remastérisation de la classe Group adaptée aux chemins temporels.
    /// </summary>
    public class Path
    {
        private List<IGraphicObject> graobjs = new List<IGraphicObject>();
        private Insert _insert = new Insert();

        public Path()
        {

        }

        public void AddCurve(Curve c)
        {
            graobjs.Add(c);
        }

        public void RemoveCurve(Curve c)
        {
            graobjs.Remove(c);
        }

        public List<IGraphicObject> GetGroup()
        {
            return graobjs;
        }

        public Insert Insert
        {
            get { return _insert; }
            set { _insert = value; }
        }

        public void Draw(Graphics g)
        {
            foreach (IGraphicObject igo in graobjs)
            {
                igo.Draw(g);
            }
            _insert.Draw(g);
        }

        public static List<Line> GetLinesOfPoint(Point p, Path g)
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

        public static List<Curve> GetCurvesOfPoint(Point p, Path g)
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

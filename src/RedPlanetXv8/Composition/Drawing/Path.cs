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

        public Path()
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

        public void Draw(Graphics g)
        {
            foreach (IGraphicObject igo in graobjs)
            {
                igo.Draw(g);
            }
        }
    }
}

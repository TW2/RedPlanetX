using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Composition.Drawing
{
    public class Insert : IGraphicObject
    {
        private Point _insert = Point.Empty;

        public Insert()
        {

        }

        public Point InsertPoint
        {
            get { return _insert; }
            set { _insert = value; }
        }

        public void Draw(Graphics g)
        {
            if(_insert != Point.Empty)
            {
                using(Pen pen = new Pen(Color.Red, 3f))
                {
                    g.DrawLine(pen, _insert.X - 15, _insert.Y, _insert.X + 15, _insert.Y);
                    g.DrawLine(pen, _insert.X, _insert.Y - 15, _insert.X, _insert.Y + 15);
                    g.DrawEllipse(pen, _insert.X - 5, _insert.Y - 5, 10, 10);
                }
            }            
        }
    }
}

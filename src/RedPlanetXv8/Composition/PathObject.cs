using RedPlanetXv8.Composition.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Composition
{
    public class PathObject
    {
        private Path path = new Path(); //Dans un seul path on peut avoir plusieurs courbes.
        private long START = 0L;
        private long END = 5000L;

        public PathObject()
        {

        }

        public PathObject(Path path)
        {
            this.path = path;
        }
        
        public long Start
        {
            get { return START; }
            set { START = value; }
        }
        
        public long End
        {
            get { return END; }
            set { END = value; }
        }

        // part :: parts
        // time :: times
        // part = time * parts / times
        public void GetXY(long approximative_time, out double x, out double y, double offset_x, double offset_y)
        {
            int parts = path.GetGroup().Count;
            int current_part = Convert.ToInt32(approximative_time * parts / END);
            double current = Convert.ToDouble(approximative_time) * Convert.ToDouble(parts) / Convert.ToDouble(END);
            //long regionSTART = END * (current_part - 1) / parts;
            //long regionEND = END * current_part / parts;

            IGraphicObject current_igo = path.GetGroup()[current_part];

            Curve c = (Curve)current_igo;

            x =
                Math.Pow((1 - current), 3) * (c.Start.X - offset_x) +
                3 * current * Math.Pow((1 - current), 2) * (c.CP1.X - offset_x) +
                3 * Math.Pow(current, 2) * (1 - current) * (c.CP2.X - offset_x) +
                Math.Pow(current, 3) * (c.End.X - offset_x);
            y =
                Math.Pow((1 - current), 3) * (c.Start.Y - offset_y) +
                3 * current * Math.Pow((1 - current), 2) * (c.CP1.Y - offset_y) +
                3 * Math.Pow(current, 2) * (1 - current) * (c.CP2.Y - offset_y) +
                Math.Pow(current, 3) * (c.End.Y - offset_y);
        }
    }
}

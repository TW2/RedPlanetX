using RedPlanetXv8.Composition.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
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
        private bool hide = false;

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

        public Path Path
        {
            get { return path; }
            set { path = value; }
        }

        public bool Hide
        {
            get { return hide; }
            set { hide = value; }
        }

        // part :: parts
        // time :: times
        // part = time * parts / times
        public void GetXY(long approximative_time, out PointF pf)
        {
            double x, y;
            double apptime = Convert.ToDouble(approximative_time);
            //Il y a toujours une courbe initialisée avec des valeurs Start
            //(there is always a curve initialized with Start values)
            //Nous devons ne pas en tenir compte d'où le -1d
            //(we get rid of it so there is -1d)
            double parts = Convert.ToDouble(path.GetGroup().Count) - 1d;
            double start = Convert.ToDouble(START);
            double end = Convert.ToDouble(END);

            double current = parts * (apptime - start) / (end - start);

            int currentPhase = 0;
            while (current > 1d)
            {
                current = current - 1d;
                currentPhase++;
            }
            
            Curve c = (Curve)path.GetGroup()[currentPhase];

            if(approximative_time >= START & approximative_time <= END && currentPhase < path.GetGroup().Count)
            {
                x =
                    Math.Pow((1 - current), 3) * (c.Start.X) +
                    3 * current * Math.Pow((1 - current), 2) * (c.CP1.X) +
                    3 * Math.Pow(current, 2) * (1 - current) * (c.CP2.X) +
                    Math.Pow(current, 3) * (c.End.X);
                y =
                    Math.Pow((1 - current), 3) * (c.Start.Y) +
                    3 * current * Math.Pow((1 - current), 2) * (c.CP1.Y) +
                    3 * Math.Pow(current, 2) * (1 - current) * (c.CP2.Y) +
                    Math.Pow(current, 3) * (c.End.Y);

                pf = new PointF(Convert.ToSingle(x), Convert.ToSingle(y));
            }
            else
            {
                pf = PointF.Empty;
            }
        }


    }
}

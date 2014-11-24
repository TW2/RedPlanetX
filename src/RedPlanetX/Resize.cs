using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    class Resize
    {
        private int xo = -1, yo = -1;
        private bool set = false;
        private double dx = 0d, dy = 0d;
        private List<GeometryObject> previewList = new List<GeometryObject>();

        public Resize()
        {

        }

        //This method has been added
        public Resize(JPoint T)
        {
            this.xo = (int)T.X;
            this.yo = (int)T.Y;
            if (xo != -1 && yo != -1)
            {
                set = true;
            }
        }

        public Resize(int xo, int yo)
        {
            this.xo = xo;
            this.yo = yo;
            if (xo != -1 && yo != -1)
            {
                set = true;
            }
        }

        public void clear()
        {
            xo = -1;
            yo = -1;
            set = false;
            dx = 0d;
            dy = 0d;
            previewList.Clear();
        }

        public void setResize(int xo, int yo)
        {
            this.xo = xo;
            this.yo = yo;
            if (xo != -1 && yo != -1)
            {
                set = true;
            }
        }

        public bool isSet()
        {
            return set;
        }

        public int getX()
        {
            return xo;
        }

        public int getY()
        {
            return yo;
        }

        public void setDistance(int dx, int dy)
        {
            this.dx = dx;
            this.dy = dy;
        }

        public double getRX()
        {
            return dx;
        }

        public double getRY()
        {
            return dy;
        }

        public void setResizePreview(List<GeometryObject> pshapes, double percent){
            if(percent==0d){
                percent = getPercent();
            }
            previewList.Clear();
            JPoint O, L, CP1, CP2, M = getReStartPoint(pshapes);

            foreach(GeometryObject go in pshapes){
                if(go.GetType() == typeof(LineObject)){
                    LineObject l = (LineObject)go;

                    O = resizeWithPoint((int)M.X, (int)M.Y, l.Start.X, l.Start.Y, percent);
                    L = resizeWithPoint((int)M.X, (int)M.Y, l.Stop.X, l.Stop.Y, percent);

                    LineObject lPRIME = new LineObject();
                    lPRIME.Start = new Point((int)O.X, (int)O.Y);
                    lPRIME.Stop = new Point((int)L.X, (int)L.Y);

                    previewList.Add(lPRIME);

                }else if (go.GetType() == typeof(BezierObject)){
                    BezierObject b = (BezierObject)go;

                    O = resizeWithPoint((int)M.X, (int)M.Y, b.Start.X, b.Start.Y, percent);
                    CP1 = resizeWithPoint((int)M.X, (int)M.Y, b.CP1.X, b.CP1.Y, percent);
                    CP2 = resizeWithPoint((int)M.X, (int)M.Y, b.CP2.X, b.CP2.Y, percent);
                    L = resizeWithPoint((int)M.X, (int)M.Y, b.Stop.X, b.Stop.Y, percent);

                    BezierObject bPRIME = new BezierObject();
                    bPRIME.Start = new Point((int)O.X, (int)O.Y);
                    bPRIME.CP1 = new Point((int)CP1.X, (int)CP1.Y);
                    bPRIME.CP2 = new Point((int)CP2.X, (int)CP2.Y);
                    bPRIME.Stop = new Point((int)L.X, (int)L.Y);

                    previewList.Add(bPRIME);
                }
            }        
        }

        public List<GeometryObject> getPreviewShapes()
        {
            return previewList;
        }

        //This method has been added
        public GraphicsPath GetPreview(List<GeometryObject> listToResize)
        {
            setResizePreview(listToResize, 0d);
            PathObject p = new PathObject();
            return p.FromArray(getPreviewShapes(), 0, 0);
        }

        /** Rotation d'un point par rapport à un autre.
         * @param xo Centre en xo
         * @param yo Centre en yo
         * @param xa Abscisse du point
         * @param ya Ordonnée du point
         * @param angle Angle en degré (positif ou négatif)
         * @return Le point modifié avec la rotation */
        private JPoint resizeWithPoint(int x, int y, int xa, int ya, double percent)
        {
            //S est le point issu d'un ReStart, c'est donc le point de référence
            //P est le point d'insertion (pour lequel on veut calculer P')
            JPoint S = new JPoint(x, y);
            JPoint P = new JPoint(xa, ya);
            JPoint Pprime = P;
            //Si P est égal à S alors, on n'a pas besoin de faire le calcule on retourne le même point.
            if (P.equals(S)) { return P; }
            //On veut que le point S soit toujours l'origine
            //On donc calcule la distance de S à P pour en sortir une distance en fonction du pourcentage
            double SP = GetDistance(S, P);
            double SPprime = SP * percent / 100;
            //On calcule l'angle S afin de savoir où resituer le point P'
            double tanS = (P.Y - S.Y) / (P.X - S.X);
            double angleS = RadiansToDegrees(Math.Atan(tanS));
            if (P.X - S.X > 0 && P.Y - S.Y >= 0)
            {
                angleS = RadiansToDegrees(Math.Atan(tanS));
            }
            if (P.X - S.X > 0 && P.Y - S.Y < 0)
            {
                angleS = RadiansToDegrees(Math.Atan(tanS) + 2 * Math.PI);
            }
            if (P.X - S.X < 0)
            {
                angleS = RadiansToDegrees(Math.Atan(tanS) + Math.PI);
            }
            //La distance en fonction du pourcentage vient s'ajouter aux coordonnées de S avec l'angle S.
            double xPprime = SPprime * Math.Cos(DegreesToRadians(angleS)) + S.X;
            double yPprime = SPprime * Math.Sin(DegreesToRadians(angleS)) + S.Y;
            //S.x-(S.x-xo+dx-xo)+(P.getX()-dx)*percent/100, S.y-(S.y-yo+dy-yo)+(P.getY()-dy)*percent/100
            //S.x-(S.x-dx)+(P.getX()-dx)*percent/100, S.y-(S.y-dy)+(P.getY()-dy)*percent/100
            Pprime.setLocation(xPprime, yPprime);
            return Pprime;
        }

        public double getPercent()
        {
            //100% = xo
            //?% = dx
            //?% = 100*dx/xo
            double percent = 100d * dx / xo;
            return percent;
        }

        public JPoint getReStartPoint(List<GeometryObject> shapes){
            GeometryObject gONE = shapes[0];

            if(gONE.GetType() == typeof(LineObject)){
                LineObject l = (LineObject)gONE;
                return new JPoint(l.Start.X, l.Start.Y);
            }else if(gONE.GetType() == typeof(BezierObject)){
                BezierObject b = (BezierObject)gONE;
                return new JPoint(b.Start.X, b.Start.Y);
            }
            return new JPoint(0, 0);
        }

        //This method has been added
        public void SetResize(List<GeometryObject> listToTranslate, double percent)
        {
            if (percent == 0d)
            {
                percent = getPercent();
            }
            JPoint O, L, CP1, CP2, M = getReStartPoint(listToTranslate);

            foreach (GeometryObject go in listToTranslate)
            {
                if (go.GetType() == typeof(LineObject))
                {
                    LineObject l = (LineObject)go;

                    O = resizeWithPoint((int)M.X, (int)M.Y, l.Start.X, l.Start.Y, percent);
                    L = resizeWithPoint((int)M.X, (int)M.Y, l.Stop.X, l.Stop.Y, percent);
                    l.Start = new Point((int)O.X, (int)O.Y);
                    l.Stop = new Point((int)L.X, (int)L.Y);
                }
                else if (go.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)go;

                    O = resizeWithPoint((int)M.X, (int)M.Y, b.Start.X, b.Start.Y, percent);
                    CP1 = resizeWithPoint((int)M.X, (int)M.Y, b.CP1.X, b.CP1.Y, percent);
                    CP2 = resizeWithPoint((int)M.X, (int)M.Y, b.CP2.X, b.CP2.Y, percent);
                    L = resizeWithPoint((int)M.X, (int)M.Y, b.Stop.X, b.Stop.Y, percent);
                    b.Start = new Point((int)O.X, (int)O.Y);
                    b.CP1 = new Point((int)CP1.X, (int)CP1.Y);
                    b.CP2 = new Point((int)CP2.X, (int)CP2.Y);
                    b.Stop = new Point((int)L.X, (int)L.Y);
                }
            } 
        }

        //This method has been added
        private double DegreesToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        //This method has been added
        private double RadiansToDegrees(double angle)
        {
            return angle * (180.0 / Math.PI);
        }

        //This method has been added
        private double GetDistance(JPoint point1, JPoint point2)
        {
            //pythagorean theorem c^2 = a^2 + b^2
            //thus c = square root(a^2 + b^2)
            double a = point2.X - point1.X;
            double b = point2.Y - point1.Y;

            return Math.Sqrt(a * a + b * b);
        }
    }
}

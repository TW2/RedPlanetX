using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    // This class is a port of Translation class of Feuille in Java
    class Translation
    {
        private JPoint T = null;
        private bool set = false;
        private double dx = 0d, dy = 0d;
        private List<GeometryObject> previewList = new List<GeometryObject>();

        public Translation()
        {
            //Public constructor without init
        }

        public Translation(JPoint T)
        {
            this.T = T;
            if (T != null)
            {
                set = true;
            }
        }

        public void clear()
        {
            T = null;
            set = false;
            dx = 0d;
            dy = 0d;
            previewList.Clear();
        }

        public void setTranslation(JPoint T)
        {
            this.T = T;
            if (T != null)
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
            return (int)T.X;
        }

        public int getY()
        {
            return (int)T.Y;
        }

        public void setDistance(double dx, double dy)
        {
            this.dx = dx;
            this.dy = dy;
        }

        public double getTX()
        {
            return dx;
        }

        public double getTY()
        {
            return dy;
        }

        public double getDX()
        {
            return dx - T.X;
        }

        public double getDY()
        {
            return dy - T.Y;
        }

        public void setTranslatonPreview(List<GeometryObject> pshapes){        
            previewList.Clear();
            JPoint O, L, CP1, CP2;
            foreach(GeometryObject go in pshapes){
                if(go.GetType() == typeof(LineObject)){
                    LineObject l = (LineObject)go;

                    O = translateWithPoint(getDX(), getDY(), l.Start.X, l.Start.Y);
                    L = translateWithPoint(getDX(), getDY(), l.Stop.X, l.Stop.Y);

                    LineObject lPRIME = new LineObject();
                    lPRIME.Start = new Point((int)O.X, (int)O.Y);
                    lPRIME.Stop = new Point((int)L.X, (int)L.Y);

                    previewList.Add(lPRIME);

                }else if (go.GetType() == typeof(BezierObject)){
                    BezierObject b = (BezierObject)go;

                    O = translateWithPoint(getDX(), getDY(), b.Start.X, b.Start.Y);
                    CP1 = translateWithPoint(getDX(), getDY(), b.CP1.X, b.CP1.Y);
                    CP2 = translateWithPoint(getDX(), getDY(), b.CP2.X, b.CP2.Y);
                    L = translateWithPoint(getDX(), getDY(), b.Stop.X, b.Stop.Y);

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

        private JPoint translateWithPoint(double rdx, double rdy, int xa, int ya)
        {
            JPoint P = new JPoint(xa, ya);
            JPoint Pprime = P;
            double xPprime = P.X + rdx;
            double yPprime = P.Y + rdy;
            Pprime.setLocation(xPprime, yPprime);
            return Pprime;
        }

        //This method has been added
        public void SetTranslation(List<GeometryObject> listToTranslate)
        {
            foreach (GeometryObject go in listToTranslate)
            {
                if (go.GetType() == typeof(LineObject))
                {
                    LineObject l = (LineObject)go;
                    l.Start = new Point(l.Start.X + (int)getDX(), l.Start.Y + (int)getDY());
                    l.Stop = new Point(l.Stop.X + (int)getDX(), l.Stop.Y + (int)getDY());
                }
                else if (go.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)go;
                    b.Start = new Point(b.Start.X + (int)getDX(), b.Start.Y + (int)getDY());
                    b.CP1 = new Point(b.CP1.X + (int)getDX(), b.CP1.Y + (int)getDY());
                    b.CP2 = new Point(b.CP2.X + (int)getDX(), b.CP2.Y + (int)getDY());
                    b.Stop = new Point(b.Stop.X + (int)getDX(), b.Stop.Y + (int)getDY());
                }
            }
        }

        //This method has been added
        public GraphicsPath GetPreview(List<GeometryObject> listToTranslate)
        {
            setTranslatonPreview(listToTranslate);
            PathObject p = new PathObject();
            return p.FromArray(getPreviewShapes(), 0, 0);
        }
    }

    // Let's use a Point that can be null like in Java
    public class JPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public JPoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public JPoint(Point p)
        {
            X = p.X;
            Y = p.Y;
        }

        public JPoint(PointF pf)
        {
            X = Convert.ToDouble(pf.X);
            Y = Convert.ToDouble(pf.Y);
        }

        public void setLocation(double x, double y)
        {
            X = x;
            Y = y;
        }

        public bool equals(JPoint K)
        {
            if (X == K.X && Y == K.Y)
            {
                return true;
            }
            return false;
        }

        public Point ToPoint()
        {
            if (this == null)
            {
                return new Point(0, 0);
            }
            else
            {
                return new Point((int)X, (int)Y);
            }
        }

        public PointF ToPointF()
        {
            if (this == null)
            {
                return new PointF(0f, 0f);
            }
            else
            {
                return new PointF(Convert.ToSingle(X), Convert.ToSingle(Y));
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    class Rotation
    {
        private int xo = -1, yo = -1;
        private bool set = false;
        private double rx = 0d, ry = 0d;
        private List<GeometryObject> previewList = new List<GeometryObject>();
    
        public Rotation(){
        
        }

        //This method has been added
        public Rotation(JPoint T)
        {
            this.xo = (int)T.X;
            this.yo = (int)T.Y;
            if (xo != -1 && yo != -1)
            {
                set = true;
            }
        }

        public Rotation(int xo, int yo)
        {
            this.xo = xo;
            this.yo = yo;
            if(xo != -1 && yo != -1){
                set = true;
            }
        }
    
        public void clear(){
            xo = -1;
            yo = -1;
            set = false;
            rx = 0d;
            ry = 0d;
            previewList.Clear();
        }
    
        public void setCenter(int xo, int yo){
            this.xo = xo;
            this.yo = yo;
            if(xo != -1 && yo != -1){
                set = true;
            }
        }
    
        public bool isSet(){
            return set;
        }
    
        public int getX(){
            return xo;
        }
    
        public int getY(){
            return yo;
        }
    
        public void setRotation(int rx, int ry){
            this.rx = rx;
            this.ry = ry;
        }
    
        public double getRX(){
            return rx;
        }
    
        public double getRY(){
            return ry;
        }
    
        public void setRotationPreview(List<GeometryObject> pshapes, double angle){
            if(angle==0d){
                angle = getAngle();
            }
            previewList.Clear();
            JPoint O, L, CP1, CP2;

            foreach(GeometryObject go in pshapes)
            {
                if(go.GetType() == typeof(LineObject)){
                    LineObject l = (LineObject)go;

                    O = rotateWithPoint(xo, yo, l.Start.X, l.Start.Y, angle);
                    L = rotateWithPoint(xo, yo, l.Stop.X, l.Stop.Y, angle);

                    LineObject lPRIME = new LineObject();
                    lPRIME.Start = new Point((int)O.X, (int)O.Y);
                    lPRIME.Stop = new Point((int)L.X, (int)L.Y);

                    previewList.Add(lPRIME);
                }else if (go.GetType() == typeof(BezierObject)){
                    BezierObject b = (BezierObject)go;

                    O = rotateWithPoint(xo, yo, b.Start.X, b.Start.Y, angle);
                    CP1 = rotateWithPoint(xo, yo, b.CP1.X, b.CP1.Y, angle);
                    CP2 = rotateWithPoint(xo, yo, b.CP2.X, b.CP2.Y, angle);
                    L = rotateWithPoint(xo, yo, b.Stop.X, b.Stop.Y, angle);

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
        public GraphicsPath GetPreview(List<GeometryObject> listToRotate)
        {
            setRotationPreview(listToRotate, 0d);
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
        private JPoint rotateWithPoint(int xo, int yo, int xa, int ya, double angle){
            JPoint S = new JPoint(xo, yo);
            JPoint P = new JPoint(xa, ya);
            JPoint Pprime = P;
            double SP = Math.Sqrt(Math.Pow(P.X-S.X, 2)+Math.Pow(P.Y-S.Y, 2));
            double tan_PSN = (P.Y - S.Y) / (P.X - S.X);
            double angle_PSN = RadiansToDegrees(Math.Atan(tan_PSN));
            if(P.X - S.X > 0 && P.Y - S.Y >= 0){
                angle_PSN = RadiansToDegrees(Math.Atan(tan_PSN));
            }
            if(P.X - S.X > 0 && P.Y - S.Y < 0){
                angle_PSN = RadiansToDegrees(Math.Atan(tan_PSN) + 2 * Math.PI);
            }
            if(P.X - S.X < 0){
                angle_PSN = RadiansToDegrees(Math.Atan(tan_PSN) + Math.PI);
            }
            double xPprime = SP * Math.Cos(DegreesToRadians(angle + angle_PSN)) + S.X;
            double yPprime = SP * Math.Sin(DegreesToRadians(angle + angle_PSN)) + S.Y;
            Pprime.setLocation(xPprime, yPprime);
            return Pprime;
        }
    
        public double getAngle(){
            JPoint S = new JPoint(xo, yo);
            JPoint P = new JPoint((int)rx, (int)ry);
            double tan_PSN = (P.Y - S.Y) / (P.X - S.X);
            double angle_PSN = RadiansToDegrees(Math.Atan(tan_PSN));
            if(P.X - S.X > 0 && P.Y - S.Y >= 0){
                angle_PSN = RadiansToDegrees(Math.Atan(tan_PSN));
            }
            if(P.X - S.X > 0 && P.Y - S.Y < 0){
                angle_PSN = RadiansToDegrees(Math.Atan(tan_PSN) + 2 * Math.PI);
            }
            if(P.X - S.X < 0){
                angle_PSN = RadiansToDegrees(Math.Atan(tan_PSN) + Math.PI);
            }
            return angle_PSN;
        }

        //This method has been added
        public void SetRotation(List<GeometryObject> listToTranslate, double angle)
        {
            if (angle == 0d)
            {
                angle = getAngle();
            }

            JPoint O, L, CP1, CP2;
            foreach (GeometryObject go in listToTranslate)
            {
                if (go.GetType() == typeof(LineObject))
                {
                    LineObject l = (LineObject)go;
                    O = rotateWithPoint(xo, yo, l.Start.X, l.Start.Y, angle);
                    L = rotateWithPoint(xo, yo, l.Stop.X, l.Stop.Y, angle);
                    l.Start = new Point((int)O.X, (int)O.Y);
                    l.Stop = new Point((int)L.X, (int)L.Y);
                }
                else if (go.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)go;
                    O = rotateWithPoint(xo, yo, b.Start.X, b.Start.Y, angle);
                    CP1 = rotateWithPoint(xo, yo, b.CP1.X, b.CP1.Y, angle);
                    CP2 = rotateWithPoint(xo, yo, b.CP2.X, b.CP2.Y, angle);
                    L = rotateWithPoint(xo, yo, b.Stop.X, b.Stop.Y, angle);
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
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    class Drawing
    {
        //===============================================================================
        //EN : Draw a landmark for volumes or particles
        //FR : Dessine un repère pour les volumes ou les particules
        //===============================================================================
        public static void DrawLandmark(Graphics g, int sizeToPaintX, int sizeToPaintY)
        {
            int i = 0;
            int j = 0;
            int x = 0;

            //Draw horizontal marks from middle
            i = sizeToPaintY / 2;
            while (i > 0)
            {
                g.DrawLine(new Pen(Brushes.Cyan, 1), 0, i, sizeToPaintX, i);
                i -= 20;
            }
            i = sizeToPaintY / 2;
            while (i < sizeToPaintY)
            {
                g.DrawLine(new Pen(Brushes.Cyan, 1), 0, i, sizeToPaintX, i);
                i += 20;
            }

            //Draw vertical marks from middle
            i = sizeToPaintX / 2;
            while (i > 0)
            {
                g.DrawLine(new Pen(Brushes.Cyan, 1), i, 0, i, sizeToPaintY);
                i -= 20;
            }
            i = sizeToPaintX / 2;
            while (i < sizeToPaintY)
            {
                g.DrawLine(new Pen(Brushes.Cyan, 1), i, 0, i, sizeToPaintY);
                i += 20;
            }

            //Draw circle marks from middle
            i = (sizeToPaintX / 2) - 20;
            j = (sizeToPaintY / 2) - 20;
            x = 40;
            while (i > -(sizeToPaintX / 2))
            {
                g.DrawEllipse(new Pen(Brushes.Cyan, 1), i, j, x, x);
                i -= 20;
                j -= 20;
                x += 40;
            }

            //Draw middle marks red
            g.DrawLine(new Pen(Brushes.LightPink, 2), 0, sizeToPaintY / 2, sizeToPaintX, sizeToPaintY / 2);
            g.DrawLine(new Pen(Brushes.LightPink, 2), sizeToPaintX / 2, 0, sizeToPaintX / 2, sizeToPaintY);
        }

        //===============================================================================
        //EN : Draw a volume without the view of its structure
        //FR : Dessine un volume en ne montrant pas sa structure
        //===============================================================================
        public static void DrawVolume(Graphics g, Volume v, int frame)
        {
            int anchorX = 0, anchorY = 0;
            bool front_rainbow = false, back_rainbow = false, thickness_rainbow = false, border_rainbow = false, shadow_rainbow = false;

            foreach (Parameter p in v.Parameters.GetValues())
            {
                if (p.Name == "Parent") { }
                if (p.Name == "Use front rainbow") { front_rainbow = (bool)p.Object; }
                if (p.Name == "Use back rainbow") { back_rainbow = (bool)p.Object; }
                if (p.Name == "Use thickness rainbow") { thickness_rainbow = (bool)p.Object; }
                if (p.Name == "Use border rainbow") { border_rainbow = (bool)p.Object; }
                if (p.Name == "Use shadow rainbow") { shadow_rainbow = (bool)p.Object; }
                if (p.Name == "Anchor X") { anchorX = (int)p.Object; }
                if (p.Name == "Anchor Y") { anchorY = (int)p.Object; }
                if (p.Name == "Position") { }
            }


            float posX = 0f, posY = 0f, posZ = 0f;
            float scaleX = 100f, scaleY = 100f;
            float angleX = 0f, angleY = 0f, angleZ = 0f;
            float quakeX = 0f, quakeY = 0f, quakeZ = 0f;
            int thickness = 0, border = 1, shadow = 0;
            Color front_c = Color.Pink, back_c = Color.Pink, thickness_c = Color.Pink, border_c = Color.Pink, shadow_c = Color.Pink;
            Event evt = Volume.GetCurrentEvent(v, anchorX, anchorY, frame);

            foreach (Parameter p in evt.Parameters.GetValues())
            {
                if (p.Name == "Position X") { posX = (float)p.Object; }
                if (p.Name == "Position Y") { posY = (float)p.Object; }
                if (p.Name == "Position Z") { posZ = (float)p.Object; }
                if (p.Name == "Scale X") { scaleX = (float)p.Object; }
                if (p.Name == "Scale Y") { scaleY = (float)p.Object; }
                if (p.Name == "Angle X") { angleX = (float)p.Object; }
                if (p.Name == "Angle Y") { angleY = (float)p.Object; }
                if (p.Name == "Angle Z") { angleZ = (float)p.Object; }
                if (p.Name == "Center X") { }
                if (p.Name == "Center Y") { }
                if (p.Name == "Quake X") { quakeX = (float)p.Object; }
                if (p.Name == "Quake Y") { quakeY = (float)p.Object; }
                if (p.Name == "Quake Z") { quakeZ = (float)p.Object; }
                if (p.Name == "Thickness") { thickness = (int)p.Object; }
                if (p.Name == "Border") { border = (int)p.Object; }
                if (p.Name == "Shadow") { shadow = (int)p.Object; }
                if (p.Name == "Front color") { front_c = (Color)p.Object; }
                if (p.Name == "Back color") { back_c = (Color)p.Object; }
                if (p.Name == "Thickness color") { thickness_c = (Color)p.Object; }
                if (p.Name == "Border color") { border_c = (Color)p.Object; }
                if (p.Name == "Shadow color") { shadow_c = (Color)p.Object; }
                if (p.Name == "Front rainbow") { }
                if (p.Name == "Back rainbow") { }
                if (p.Name == "Thickness rainbow") { }
                if (p.Name == "Border rainbow") { }
                if (p.Name == "Shadow rainbow") { }
            }

            PathObject path = new PathObject();

            foreach (CreationObject cro in v.Objects)
            {

                PointF center = GetVolumeGravityCenter(cro);
                GraphicsPath gp = path.FromArray(cro.Array, (int)center.X, (int)center.Y);

                if (gp != null)
                {
                    g.ResetTransform();
                    g.TranslateTransform(center.X, center.Y);

                    g.RotateTransform(angleZ);
                    g.ScaleTransform(scaleX / 100, scaleY / 100);

                    //g.FillPath(new SolidBrush(front_c), gp);
                    //g.DrawPath(new Pen(border_c, border), gp);

                    //g.FillRectangle(Brushes.Red, center.X - 4, center.Y - 4, 8f, 8f); Don't look it or drink before to view it

                    //GraphicsPath mirror = GetMirror(g, cro.Array, center);
                    //g.FillPath(new SolidBrush(back_c), mirror);

                    GraphicsPath gp_2 = GetRotationXY(cro.Array, center, angleX, angleY);
                    Random random = new Random();

                    if (quakeX > 0)
                    {
                        quakeX = random.Next(-(int)quakeX, (int)quakeX + 1);
                    }

                    if (quakeY > 0)
                    {
                        quakeY = random.Next(-(int)quakeY, (int)quakeY + 1);
                    }

                    g.TranslateTransform(posX + quakeX, posY + quakeY);

                    if (shadow > 0)
                    {
                        g.TranslateTransform(shadow, shadow);
                        g.FillPath(new SolidBrush(shadow_c), gp_2);
                        g.TranslateTransform(-shadow, -shadow);
                    }

                    g.FillPath(new SolidBrush(front_c), gp_2);

                    if (border > 0)
                    {
                        g.DrawPath(new Pen(border_c, border), gp_2);
                    }
                    
                    
                }
            }

        }

        //===============================================================================
        //EN : Draw a volume with the view of its structure
        //FR : Dessine un volume en montrant sa structure
        //===============================================================================
        public static void DrawVolumeStructure(Graphics g, Volume v, int frame, List<GeometryPoint> volume_geoPoints)
        {
            PathObject path = new PathObject();

            foreach (CreationObject cro in v.Objects)
            {
                GraphicsPath gp = path.FromArray(cro.Array, 0, 0);

                if (cro.Selected == true)
                {
                    if (gp != null)
                    {
                        g.FillPath(new SolidBrush(Color.FromArgb(127, Color.Lime)), gp);
                        g.DrawPath(new Pen(Color.Red, 1), gp);
                        DrawAllControlStructures(cro, g);
                        DrawAllPoints(cro, g, volume_geoPoints);
                    }
                }
                else
                {
                    if (gp != null)
                    {
                        g.FillPath(new SolidBrush(Color.FromArgb(127, Color.White)), gp);
                        g.DrawPath(new Pen(Color.Crimson, 1), gp);
                        DrawAllControlStructures(cro, g);
                        DrawAllPoints(cro, g, volume_geoPoints);
                    }
                }
            }
        }

        //===============================================================================
        //EN : Draw all points of a structure (including control points of bezier)
        //FR : Dessine tous les points d'une structure (incluant les points de contrôle d'un bézier)
        //===============================================================================
        private static void DrawAllPoints(CreationObject co, Graphics g, List<GeometryPoint> geoPoints)
        {
            foreach (GeometryObject go in co.Array)
            {
                if (go.GetType() == typeof(LineObject))
                {
                    LineObject l = (LineObject)go;
                    g.FillRectangle(new SolidBrush(Color.Blue), l.Start.X - 2, l.Start.Y - 2, 4, 4);
                    g.FillRectangle(new SolidBrush(Color.Blue), l.Stop.X - 2, l.Stop.Y - 2, 4, 4);
                }
                else if (go.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)go;
                    g.FillRectangle(new SolidBrush(Color.Blue), b.Start.X - 2, b.Start.Y - 2, 4, 4);
                    g.FillRectangle(new SolidBrush(Color.Blue), b.Stop.X - 2, b.Stop.Y - 2, 4, 4);
                    g.FillEllipse(new SolidBrush(Color.Orange), b.CP1.X - 2, b.CP1.Y - 2, 4, 4);
                    g.FillEllipse(new SolidBrush(Color.Orange), b.CP2.X - 2, b.CP2.Y - 2, 4, 4);
                }
            }
            if (geoPoints.Count > 0)
            {
                foreach (GeometryPoint geoPoint in geoPoints)
                {
                    if (geoPoint.SelectedObject.GetType() == typeof(LineObject))
                    {
                        LineObject l = (LineObject)geoPoint.SelectedObject;
                        if (geoPoint.SelectedPoint == GeometryPoint.PointType.Start)
                        {
                            g.DrawEllipse(new Pen(Brushes.Fuchsia, 3), l.Start.X - 6, l.Start.Y - 6, 12, 12);
                        }
                        else if (geoPoint.SelectedPoint == GeometryPoint.PointType.Stop)
                        {
                            g.DrawEllipse(new Pen(Brushes.Fuchsia, 3), l.Stop.X - 6, l.Stop.Y - 6, 12, 12);
                        }
                    }
                    else if (geoPoint.SelectedObject.GetType() == typeof(BezierObject))
                    {
                        BezierObject b = (BezierObject)geoPoint.SelectedObject;
                        if (geoPoint.SelectedPoint == GeometryPoint.PointType.Start)
                        {
                            g.DrawEllipse(new Pen(Brushes.Fuchsia, 3), b.Start.X - 6, b.Start.Y - 6, 12, 12);
                        }
                        else if (geoPoint.SelectedPoint == GeometryPoint.PointType.Stop)
                        {
                            g.DrawEllipse(new Pen(Brushes.Fuchsia, 3), b.Stop.X - 6, b.Stop.Y - 6, 12, 12);
                        }
                        else if (geoPoint.SelectedPoint == GeometryPoint.PointType.CP1)
                        {
                            g.DrawEllipse(new Pen(Brushes.Fuchsia, 3), b.CP1.X - 6, b.CP1.Y - 6, 12, 12);
                        }
                        else if (geoPoint.SelectedPoint == GeometryPoint.PointType.CP2)
                        {
                            g.DrawEllipse(new Pen(Brushes.Fuchsia, 3), b.CP2.X - 6, b.CP2.Y - 6, 12, 12);
                        }
                    }
                }
            }
        }

        //===============================================================================
        //EN : Draw the dashed lines between control points of a bezier 
        //FR : Dessine les lignes en pointillé entre les points de contrôle d'un bézier
        //===============================================================================
        private static void DrawAllControlStructures(CreationObject co, Graphics g)
        {
            foreach (GeometryObject go in co.Array)
            {
                if (go.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)go;
                    Pen dashedPen = new Pen(Brushes.Gray, 1.5f);
                    dashedPen.DashStyle = DashStyle.Dash;
                    g.DrawLine(dashedPen, b.Start, b.CP1);
                    g.DrawLine(dashedPen, b.CP1, b.CP2);
                    g.DrawLine(dashedPen, b.CP2, b.Stop);
                }
            }
        }

        public static void DrawPreview(Graphics g, List<GeometryObject> goList, Color c)
        {
            foreach (GeometryObject go in goList)
            {
                if (go.GetType() == typeof(LineObject))
                {
                    LineObject l = (LineObject)go;
                }
                else if (go.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)go;
                    Pen dashedPen = new Pen(c, 1.5f);
                    dashedPen.DashStyle = DashStyle.Dash;
                    g.DrawLine(dashedPen, b.Start, b.CP1);
                    g.DrawLine(dashedPen, b.CP1, b.CP2);
                    g.DrawLine(dashedPen, b.CP2, b.Stop);
                }
            }
        }

        //===============================================================================
        //EN : Draw a volume without the view of its structure and with insert point
        //FR : Dessine un volume en ne montrant pas sa structure et montrant l'insert point
        //===============================================================================
        public static void DrawVolumeAndInsertPoint(Graphics g, Volume v, int frame, int maxFrame, List<GeometryPoint> geoPoints)
        {
            int anchorX = 0, anchorY = 0;
            bool front_rainbow = false, back_rainbow = false, thickness_rainbow = false, border_rainbow = false, shadow_rainbow = false;

            foreach (Parameter p in v.Parameters.GetValues())
            {
                if (p.Name == "Parent") { }
                if (p.Name == "Use front rainbow") { front_rainbow = (bool)p.Object; }
                if (p.Name == "Use back rainbow") { back_rainbow = (bool)p.Object; }
                if (p.Name == "Use thickness rainbow") { thickness_rainbow = (bool)p.Object; }
                if (p.Name == "Use border rainbow") { border_rainbow = (bool)p.Object; }
                if (p.Name == "Use shadow rainbow") { shadow_rainbow = (bool)p.Object; }
                if (p.Name == "Anchor X") { anchorX = (int)p.Object; }
                if (p.Name == "Anchor Y") { anchorY = (int)p.Object; }
                if (p.Name == "Position") { }
            }


            float posX = 0f, posY = 0f, posZ = 0f;
            float scaleX = 100f, scaleY = 100f;
            float angleX = 0f, angleY = 0f, angleZ = 0f;
            float quakeX = 0f, quakeY = 0f, quakeZ = 0f;
            int thickness = 0, border = 1, shadow = 0;
            Color front_c = Color.Pink, back_c = Color.Pink, thickness_c = Color.Pink, border_c = Color.Pink, shadow_c = Color.Pink;
            Event evt = Volume.GetCurrentEvent(v, anchorX, anchorY, frame);

            foreach (Parameter p in evt.Parameters.GetValues())
            {
                if (p.Name == "Position X") { posX = (float)p.Object; }
                if (p.Name == "Position Y") { posY = (float)p.Object; }
                if (p.Name == "Position Z") { posZ = (float)p.Object; }
                if (p.Name == "Scale X") { scaleX = (float)p.Object; }
                if (p.Name == "Scale Y") { scaleY = (float)p.Object; }
                if (p.Name == "Angle X") { angleX = (float)p.Object; }
                if (p.Name == "Angle Y") { angleY = (float)p.Object; }
                if (p.Name == "Angle Z") { angleZ = (float)p.Object; }
                if (p.Name == "Center X") { }
                if (p.Name == "Center Y") { }
                if (p.Name == "Quake X") { quakeX = (float)p.Object; }
                if (p.Name == "Quake Y") { quakeY = (float)p.Object; }
                if (p.Name == "Quake Z") { quakeZ = (float)p.Object; }
                if (p.Name == "Thickness") { thickness = (int)p.Object; }
                if (p.Name == "Border") { border = (int)p.Object; }
                if (p.Name == "Shadow") { shadow = (int)p.Object; }
                if (p.Name == "Front color") { front_c = (Color)p.Object; }
                if (p.Name == "Back color") { back_c = (Color)p.Object; }
                if (p.Name == "Thickness color") { thickness_c = (Color)p.Object; }
                if (p.Name == "Border color") { border_c = (Color)p.Object; }
                if (p.Name == "Shadow color") { shadow_c = (Color)p.Object; }
                if (p.Name == "Front rainbow") { }
                if (p.Name == "Back rainbow") { }
                if (p.Name == "Thickness rainbow") { }
                if (p.Name == "Border rainbow") { }
                if (p.Name == "Shadow rainbow") { }
            }

            foreach (InsertPoint ip in v.GettInsertPoints())
            {
                float ip_posX = 0f, ip_posY = 0f, ip_size = 100f;
                Color ip_c = Color.Red;

                foreach (Parameter p in ip.Parameters.GetValues())
                {
                    if (p.Name == "Position X") { ip_posX = (float)p.Object; }
                    if (p.Name == "Position Y") { ip_posY = (float)p.Object; }
                    if (p.Name == "Color") { ip_c = (Color)p.Object; }
                    if (p.Name == "Size") { ip_size = (float)p.Object; }
                }

                foreach (GeometryObject go in ip.GetTrajectorySpline().GetTrajectory())
                {
                    if (go.GetType() == typeof(LineObject))
                    {
                        //Main drawing of trajectory
                        LineObject l = (LineObject)go;
                        g.FillRectangle(new SolidBrush(Color.Blue), l.Start.X - 2, l.Start.Y - 2, 4, 4);
                        g.FillRectangle(new SolidBrush(Color.Blue), l.Stop.X - 2, l.Stop.Y - 2, 4, 4);
                        g.DrawLine(new Pen(Color.Red), l.Start, l.Stop);

                    }
                    else if (go.GetType() == typeof(BezierObject))
                    {
                        //Main drawing of trajectory
                        BezierObject b = (BezierObject)go;
                        g.FillRectangle(new SolidBrush(Color.Blue), b.Start.X - 2, b.Start.Y - 2, 4, 4);
                        g.FillRectangle(new SolidBrush(Color.Blue), b.Stop.X - 2, b.Stop.Y - 2, 4, 4);
                        g.FillEllipse(new SolidBrush(Color.Orange), b.CP1.X - 2, b.CP1.Y - 2, 4, 4);
                        g.FillEllipse(new SolidBrush(Color.Orange), b.CP2.X - 2, b.CP2.Y - 2, 4, 4);
                        g.DrawBezier(new Pen(Color.Red), b.Start, b.CP1, b.CP2, b.Stop);
                        Pen dashed = new Pen(Brushes.Black, 1f);
                        dashed.DashStyle = DashStyle.Dash;
                        g.DrawLine(dashed, b.Start.X, b.Start.Y, b.CP1.X, b.CP1.Y);
                        g.DrawLine(dashed, b.CP1.X, b.CP1.Y, b.CP2.X, b.CP2.Y);
                        g.DrawLine(dashed, b.CP2.X, b.CP2.Y, b.Stop.X, b.Stop.Y);
                    }
                }

                PathObject path = new PathObject();

                foreach (CreationObject cro in v.Objects)
                {

                    PointF center = GetVolumeGravityCenter(cro);
                    GraphicsPath gp = path.FromArray(cro.Array, (int)center.X, (int)center.Y);

                    JPoint Start = null;

                    // Drawing of the trajectory =========================================================
                    foreach (GeometryObject go in ip.GetTrajectorySpline().GetTrajectory())
                    {
                        if (go.GetType() == typeof(LineObject))
                        {
                            LineObject l = (LineObject)go;

                            if (Start == null)
                            {
                                Start = new JPoint(l.Start);
                            }

                            //Real trajectory
                            g.ResetTransform();
                            g.TranslateTransform(center.X, center.Y);
                            g.TranslateTransform(ip_posX, ip_posY); //InsertPoint
                            g.TranslateTransform(posX , posY);
                            float diffX = Start.ToPointF().X;
                            float diffY = Start.ToPointF().Y;
                            g.FillRectangle(new SolidBrush(ip_c), l.Start.X - 2 - diffX, l.Start.Y - 2 - diffY, 4, 4);
                            g.FillRectangle(new SolidBrush(ip_c), l.Stop.X - 2 - diffX, l.Stop.Y - 2 - diffY, 4, 4);
                            g.DrawLine(new Pen(ip_c), l.Start.X - diffX, l.Start.Y - diffY, l.Stop.X - diffX, l.Stop.Y - diffY);
                        }
                        else if (go.GetType() == typeof(BezierObject))
                        {
                            BezierObject b = (BezierObject)go;

                            if (Start == null)
                            {
                                Start = new JPoint(b.Start);
                            }

                            //Real trajectory
                            g.ResetTransform();
                            g.TranslateTransform(center.X, center.Y);
                            g.TranslateTransform(ip_posX, ip_posY); //InsertPoint
                            g.TranslateTransform(posX, posY);
                            float diffX = Start.ToPointF().X;
                            float diffY = Start.ToPointF().Y;
                            g.FillRectangle(new SolidBrush(ip_c), b.Start.X - 2 - diffX, b.Start.Y - 2 - diffY, 4, 4);
                            g.FillRectangle(new SolidBrush(ip_c), b.Stop.X - 2 - diffX, b.Stop.Y - 2 - diffY, 4, 4);
                            g.FillEllipse(new SolidBrush(ip_c), b.CP1.X - 2 - diffX, b.CP1.Y - 2 - diffY, 4, 4);
                            g.FillEllipse(new SolidBrush(ip_c), b.CP2.X - 2 - diffX, b.CP2.Y - 2 - diffY, 4, 4);
                            g.DrawBezier(new Pen(ip_c), b.Start.X - diffX, b.Start.Y - diffY, b.CP1.X - diffX, b.CP1.Y - diffY, b.CP2.X - diffX, b.CP2.Y - diffY, b.Stop.X - diffX, b.Stop.Y - diffY);
                            //Pen dashed = new Pen(Brushes.Black, 1f);
                            //dashed.DashStyle = DashStyle.Dash;
                            //g.DrawLine(dashed, b.Start.X - diffX, b.Start.Y - diffY, b.CP1.X - diffX, b.CP1.Y - diffY);
                            //g.DrawLine(dashed, b.CP1.X - diffX, b.CP1.Y - diffY, b.CP2.X - diffX, b.CP2.Y - diffY);
                            //g.DrawLine(dashed, b.CP2.X - diffX, b.CP2.Y - diffY, b.Stop.X - diffX, b.Stop.Y - diffY);
                        }
                    }

                    if (geoPoints.Count > 0)
                    {
                        float diffX = Start.ToPointF().X;
                        float diffY = Start.ToPointF().Y;

                        foreach (GeometryPoint geoPoint in geoPoints)
                        {
                            if (geoPoint.SelectedObject.GetType() == typeof(LineObject))
                            {
                                LineObject l = (LineObject)geoPoint.SelectedObject;
                                if (geoPoint.SelectedPoint == GeometryPoint.PointType.Start)
                                {
                                    g.DrawEllipse(new Pen(Brushes.Fuchsia, 3), l.Start.X - 6 - diffX, l.Start.Y - 6 - diffY, 12, 12);
                                }
                                else if (geoPoint.SelectedPoint == GeometryPoint.PointType.Stop)
                                {
                                    g.DrawEllipse(new Pen(Brushes.Fuchsia, 3), l.Stop.X - 6 - diffX, l.Stop.Y - 6 - diffY, 12, 12);
                                }
                            }
                            else if (geoPoint.SelectedObject.GetType() == typeof(BezierObject))
                            {
                                BezierObject b = (BezierObject)geoPoint.SelectedObject;
                                if (geoPoint.SelectedPoint == GeometryPoint.PointType.Start)
                                {
                                    g.DrawEllipse(new Pen(Brushes.Fuchsia, 3), b.Start.X - 6 - diffX, b.Start.Y - 6 - diffY, 12, 12);
                                }
                                else if (geoPoint.SelectedPoint == GeometryPoint.PointType.Stop)
                                {
                                    g.DrawEllipse(new Pen(Brushes.Fuchsia, 3), b.Stop.X - 6 - diffX, b.Stop.Y - 6 - diffY, 12, 12);
                                }
                                else if (geoPoint.SelectedPoint == GeometryPoint.PointType.CP1)
                                {
                                    g.DrawEllipse(new Pen(Brushes.Fuchsia, 3), b.CP1.X - 6 - diffX, b.CP1.Y - 6 - diffY, 12, 12);
                                }
                                else if (geoPoint.SelectedPoint == GeometryPoint.PointType.CP2)
                                {
                                    g.DrawEllipse(new Pen(Brushes.Fuchsia, 3), b.CP2.X - 6 - diffX, b.CP2.Y - 6 - diffY, 12, 12);
                                }
                            }
                        }
                    }
                    // End - Drawing of the trajectory ===================================================

                    // Position on trajectory ============================================================
                    double xPiece = 0f, yPiece = 0f;
                    try
                    {
                        int loops = 0;
                        int pieces = ip.GetTrajectorySpline().GetTrajectory().Count;
                        //double fraction = 1 / pieces * (loops + 1);
                        double current = Convert.ToDouble(pieces) * Convert.ToDouble(frame) / Convert.ToDouble(maxFrame);
                        //double realCurrent = current / fraction;
                        //double realPiece = Math.Ceiling(current);
                        //double pieceCurrent = realCurrent - realPiece;

                        int currentPhase = 0;
                        while (current > 1d)
                        {
                            current = current - 1d;
                            currentPhase++;
                        }

                        if (currentPhase >= 0)
                        {
                            GeometryObject goo = ip.GetTrajectorySpline().GetTrajectory()[currentPhase];
                            if (goo.GetType() == typeof(LineObject))
                            {
                                LineObject l = (LineObject)goo;

                            }
                            else if (goo.GetType() == typeof(BezierObject))
                            {
                                BezierObject b = (BezierObject)goo;
                                xPiece =
                                    Math.Pow((1 - current), 3) * (b.Start.X - Start.ToPointF().X) +
                                    3 * current * Math.Pow((1 - current), 2) * (b.CP1.X - Start.ToPointF().X) +
                                    3 * Math.Pow(current, 2) * (1 - current) * (b.CP2.X - Start.ToPointF().X) +
                                    Math.Pow(current, 3) * (b.Stop.X - Start.ToPointF().X);
                                yPiece =
                                    Math.Pow((1 - current), 3) * (b.Start.Y - Start.ToPointF().Y) +
                                    3 * current * Math.Pow((1 - current), 2) * (b.CP1.Y - Start.ToPointF().Y) +
                                    3 * Math.Pow(current, 2) * (1 - current) * (b.CP2.Y - Start.ToPointF().Y) +
                                    Math.Pow(current, 3) * (b.Stop.Y - Start.ToPointF().Y);
                            }
                        }                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    // End - Position on trajectory ======================================================

                    if (gp != null)
                    {
                        g.ResetTransform();
                        g.TranslateTransform(center.X, center.Y);                        

                        g.TranslateTransform(ip_posX + (float)xPiece, ip_posY + (float)yPiece); //InsertPoint//Trajectory
                        g.ScaleTransform(ip_size / 100, ip_size / 100); //InsertPoint

                        Random random = new Random();
                        if (quakeX > 0)
                        {
                            quakeX = random.Next(-(int)quakeX, (int)quakeX + 1);
                        }

                        if (quakeY > 0)
                        {
                            quakeY = random.Next(-(int)quakeY, (int)quakeY + 1);
                        }

                        g.TranslateTransform(posX + quakeX, posY + quakeY); 

                        g.RotateTransform(angleZ);
                        g.ScaleTransform(scaleX / 100, scaleY / 100);                        

                        GraphicsPath gp_2 = GetRotationXY(cro.Array, center, angleX, angleY);
                        


                        if (shadow > 0)
                        {
                            g.TranslateTransform(shadow, shadow);
                            g.FillPath(new SolidBrush(shadow_c), gp_2);
                            g.TranslateTransform(-shadow, -shadow);
                        }

                        g.FillPath(new SolidBrush(front_c), gp_2);
                        
                        if (border > 0)
                        {
                            g.DrawPath(new Pen(border_c, border), gp_2);
                        }

                        g.FillEllipse(new SolidBrush(ip_c), -10f, -10f, 20f, 20f);
                    }
                }

            }

        }

        public static void DrawFXSubtitle(Graphics g, FXSubtitle fxs, long milliseconds)
        {
            List<AssLine> lines = fxs.GetLinesAt(milliseconds);
            foreach (AssLine al in lines)
            {
                g.DrawString(al.Line.String, al.Line.Font, Brushes.Red, al.Line.X - al.Line.Size.Width / 2, al.Line.Y - al.Line.Size.Height / 2);
            }

            List<AssAllSyllables> allsyllables = fxs.GetAllSyllablesAt(milliseconds);
            foreach (AssAllSyllables aas in allsyllables)
            {
                foreach(TString syllable in aas.GetSyllables()){
                    g.DrawString(syllable.String, syllable.Font, Brushes.Yellow, syllable.X - syllable.Size.Width / 2, syllable.Y - syllable.Size.Height / 2);
                }
            }
        }

        private static GraphicsPath GetRotationXY(List<GeometryObject> array, PointF gravity, float angleX, float angleY)
        {
            // 0 - 90 -> ScaleX 100 - 0 (quartType = 1)
            // 90 - 180 -> Reverse+Scale X 0 - 100 (quartType = 2)
            // 180 - 270 -> Reverse+Scale X 100 - 0 (quartType = 3)
            // 270 - 360 -> ScaleX 0 - 100 (quartType = 4)

            int quartTypeX = GetRotationXYQuartType(angleX);
            int quartTypeY = GetRotationXYQuartType(angleY);
            float percentX = GetRotationXYPercent(angleX);
            float percentY = GetRotationXYPercent(angleY);

            List<GeometryObject> new_array = new List<GeometryObject>();

            foreach (GeometryObject go in array)
            {
                if (go.GetType() == typeof(LineObject))
                {
                    LineObject l = (LineObject)go;

                    float newStartX = GetRotationXYPosition(quartTypeX, percentX, gravity.X, l.Start.X);
                    float newStartY = GetRotationXYPosition(quartTypeY, percentY, gravity.Y, l.Start.Y);

                    float newStopX = GetRotationXYPosition(quartTypeX, percentX, gravity.X, l.Stop.X);
                    float newStopY = GetRotationXYPosition(quartTypeY, percentY, gravity.Y, l.Stop.Y);

                    LineObject l2 = new LineObject();
                    l2.Start = new Point((int)newStartX, (int)newStartY);
                    l2.Stop = new Point((int)newStopX, (int)newStopY);
                    new_array.Add(l2);

                }
                else if (go.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)go;

                    float newStartX = GetRotationXYPosition(quartTypeX, percentX, gravity.X, b.Start.X);
                    float newStartY = GetRotationXYPosition(quartTypeY, percentY, gravity.Y, b.Start.Y);

                    float newCP1X = GetRotationXYPosition(quartTypeX, percentX, gravity.X, b.CP1.X);
                    float newCP1Y = GetRotationXYPosition(quartTypeY, percentY, gravity.Y, b.CP1.Y);

                    float newCP2X = GetRotationXYPosition(quartTypeX, percentX, gravity.X, b.CP2.X);
                    float newCP2Y = GetRotationXYPosition(quartTypeY, percentY, gravity.Y, b.CP2.Y);

                    float newStopX = GetRotationXYPosition(quartTypeX, percentX, gravity.X, b.Stop.X);
                    float newStopY = GetRotationXYPosition(quartTypeY, percentY, gravity.Y, b.Stop.Y);

                    BezierObject b2 = new BezierObject();
                    b2.Start = new Point((int)newStartX, (int)newStartY);
                    b2.CP1 = new Point((int)newCP1X, (int)newCP1Y);
                    b2.CP2 = new Point((int)newCP2X, (int)newCP2Y);
                    b2.Stop = new Point((int)newStopX, (int)newStopY);
                    new_array.Add(b2);
                }
            }

            PathObject path = new PathObject();

            return path.FromArray(new_array, 0, 0);
        }

        private static float GetRotationXYPercent(float angle)
        {
            float percent = 0f;

            if (angle > 360)
            {
                angle -= (360 * (angle / 360));
            }

            if (angle <= 90)
            {
                percent = 100 * angle / 90;
            }
            else if (angle > 90 && angle <= 180)
            {
                angle -= 90;
                percent = 100 * angle / 90;
            }
            else if (angle > 180 && angle <= 270)
            {
                angle -= 180;
                percent = 100 * angle / 90;
            }
            else if (angle > 270 && angle <= 360)
            {
                angle -= 270;
                percent = 100 * angle / 90;
            }

            return percent;
        }

        private static int GetRotationXYQuartType(float angle)
        {
            int quartType = 0;

            if (angle > 360)
            {
                angle -= (360 * (angle / 360));
            }

            if (angle <= 90)
            {
                quartType = 1;
            }
            else if (angle > 90 && angle <= 180)
            {
                quartType = 2;
            }
            else if (angle > 180 && angle <= 270)
            {
                quartType = 3;
            }
            else if (angle > 270 && angle <= 360)
            {
                quartType = 4;
            }

            return quartType;
        }

        private static float GetRotationXYPosition(int quartType, float percent, float gravity, int posBefore)
        {
            float posAfter = 0;
            float diff = posBefore - gravity;

            if (quartType == 1)
            {
                posAfter = diff - (diff * (percent / 100f)); 
            }
            else if (quartType == 2)
            {
                posAfter = -(diff - (diff * ((100 - percent) / 100f)));
            }
            else if (quartType == 3)
            {
                posAfter = -(diff - (diff * (percent / 100f)));
            }
            else if (quartType == 4)
            {
                posAfter = diff - (diff * ((100 - percent) / 100f));
            }

            return posAfter;
        }

        private static GraphicsPath GetMirror(Graphics g, List<GeometryObject> array, PointF gravity)
        {
            List<GeometryObject> new_array = new List<GeometryObject>();

            foreach (GeometryObject go in array)
            {
                if (go.GetType() == typeof(LineObject))
                {
                    LineObject l = (LineObject)go;

                    float diff_fromStartX = l.Start.X - gravity.X;
                    float newStartX = -diff_fromStartX;
                    float diff_fromStartY = l.Start.Y - gravity.Y;
                    float newStartY = -diff_fromStartY;

                    float diff_fromStopX = l.Stop.X - gravity.X;
                    float newStopX = -diff_fromStopX;
                    float diff_fromStopY = l.Stop.Y - gravity.Y;
                    float newStopY = -diff_fromStopY;

                    LineObject l2 = new LineObject();
                    l2.Start = new Point((int)newStartX, (int)newStartY);
                    l2.Stop = new Point((int)newStopX, (int)newStopY);
                    new_array.Add(l2);
                    
                }
                else if (go.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)go;

                    float diff_fromStartX = b.Start.X - gravity.X;
                    float newStartX = -diff_fromStartX;
                    float diff_fromStartY = b.Start.Y - gravity.Y;
                    float newStartY = -diff_fromStartY;

                    float diff_fromCP1X = b.CP1.X - gravity.X;
                    float newCP1X = -diff_fromCP1X;
                    float diff_fromCP1Y = b.CP1.Y - gravity.Y;
                    float newCP1Y = -diff_fromCP1Y;

                    float diff_fromCP2X = b.CP2.X - gravity.X;
                    float newCP2X = -diff_fromCP2X;
                    float diff_fromCP2Y = b.CP2.Y - gravity.Y;
                    float newCP2Y = -diff_fromCP2Y;

                    float diff_fromStopX = b.Stop.X - gravity.X;
                    float newStopX = -diff_fromStopX;
                    float diff_fromStopY = b.Stop.Y - gravity.Y;
                    float newStopY = -diff_fromStopY;

                    BezierObject b2 = new BezierObject();
                    b2.Start = new Point((int)newStartX, (int)newStartY);
                    b2.CP1 = new Point((int)newCP1X, (int)newCP1Y);
                    b2.CP2 = new Point((int)newCP2X, (int)newCP2Y);
                    b2.Stop = new Point((int)newStopX, (int)newStopY);
                    new_array.Add(b2);
                }
            }

            PathObject path = new PathObject();

            return path.FromArray(new_array, 0, 0);
        }

        // source @ http://stackoverflow.com/questions/9815699/how-to-calculate-centroid
        private static PointF GetVolumeGravityCenter(CreationObject co)
        {
            List<PointF> poly = new List<PointF>(); 
            PointF p = new PointF(0, 0);
            Point lastp = new Point(-1, -1);

            foreach (GeometryObject go in co.Array)
            {
                if (go.GetType() == typeof(LineObject))
                {
                    LineObject l = (LineObject)go;
                    if (l.Start != lastp)
                    {
                        poly.Add(new PointF(l.Start.X, l.Start.Y));
                        lastp = l.Start;
                    }

                    if (l.Stop != lastp)
                    {
                        poly.Add(new PointF(l.Stop.X, l.Stop.Y));
                        lastp = l.Stop;
                    }
                }
                else if (go.GetType() == typeof(BezierObject))
                {
                    BezierObject b = (BezierObject)go;
                    if (b.Start != lastp)
                    {
                        poly.Add(new PointF(b.Start.X, b.Start.Y));
                        lastp = b.Start;
                    }

                    if (b.Stop != lastp)
                    {
                        poly.Add(new PointF(b.Stop.X, b.Stop.Y));
                        lastp = b.Stop;
                    }
                }
            }

            float accumulatedArea = 0.0f;
            float centerX = 0.0f;
            float centerY = 0.0f;

            for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
            {
                float temp = poly[i].X * poly[j].Y - poly[j].X * poly[i].Y;
                accumulatedArea += temp;
                centerX += (poly[i].X + poly[j].X) * temp;
                centerY += (poly[i].Y + poly[j].Y) * temp;
            }

            if (accumulatedArea >= 1E-7f)
            {
                accumulatedArea *= 3f;
                p = new PointF(centerX / accumulatedArea, centerY / accumulatedArea);
            }

            return p;
        }

        public static void DrawInsertPoint(Graphics g, InsertPoint ip, int frame)
        {
            float ip_posX = 0f, ip_posY = 0f, ip_size = 100f;
            Color ip_c = Color.Red;

            foreach (Parameter p in ip.Parameters.GetValues())
            {
                if (p.Name == "Position X") { ip_posX = (float)p.Object; }
                if (p.Name == "Position Y") { ip_posY = (float)p.Object; }
                if (p.Name == "Color") { ip_c = (Color)p.Object; }
                if (p.Name == "Size") { ip_size = (float)p.Object; }
            }

            g.FillEllipse(new SolidBrush(ip_c), ip_posX - 10f, ip_posY - 10f, 20f, 20f);

            //int anchorX = 0, anchorY = 0;
            //bool front_rainbow = false, back_rainbow = false, thickness_rainbow = false, border_rainbow = false, shadow_rainbow = false;

            //foreach (Parameter p in v.Parameters.GetValues())
            //{
            //    if (p.Name == "Parent") { }
            //    if (p.Name == "Use front rainbow") { front_rainbow = (bool)p.Object; }
            //    if (p.Name == "Use back rainbow") { back_rainbow = (bool)p.Object; }
            //    if (p.Name == "Use thickness rainbow") { thickness_rainbow = (bool)p.Object; }
            //    if (p.Name == "Use border rainbow") { border_rainbow = (bool)p.Object; }
            //    if (p.Name == "Use shadow rainbow") { shadow_rainbow = (bool)p.Object; }
            //    if (p.Name == "Anchor X") { anchorX = (int)p.Object; }
            //    if (p.Name == "Anchor Y") { anchorY = (int)p.Object; }
            //    if (p.Name == "Position") { }
            //}


            //float posX = 0f, posY = 0f, posZ = 0f;
            //float scaleX = 100f, scaleY = 100f;
            //float angleX = 0f, angleY = 0f, angleZ = 0f;
            //float quakeX = 0f, quakeY = 0f, quakeZ = 0f;
            //int thickness = 0, border = 1, shadow = 0;
            //Color front_c = Color.Pink, back_c = Color.Pink, thickness_c = Color.Pink, border_c = Color.Pink, shadow_c = Color.Pink;
            //Event evt = Volume.GetCurrentEvent(v, anchorX, anchorY, frame);

            //foreach (Parameter p in evt.Parameters.GetValues())
            //{
            //    if (p.Name == "Position X") { posX = (float)p.Object; }
            //    if (p.Name == "Position Y") { posY = (float)p.Object; }
            //    if (p.Name == "Position Z") { posZ = (float)p.Object; }
            //    if (p.Name == "Scale X") { scaleX = (float)p.Object; }
            //    if (p.Name == "Scale Y") { scaleY = (float)p.Object; }
            //    if (p.Name == "Angle X") { angleX = (float)p.Object; }
            //    if (p.Name == "Angle Y") { angleY = (float)p.Object; }
            //    if (p.Name == "Angle Z") { angleZ = (float)p.Object; }
            //    if (p.Name == "Center X") { }
            //    if (p.Name == "Center Y") { }
            //    if (p.Name == "Quake X") { quakeX = (float)p.Object; }
            //    if (p.Name == "Quake Y") { quakeY = (float)p.Object; }
            //    if (p.Name == "Quake Z") { quakeZ = (float)p.Object; }
            //    if (p.Name == "Thickness") { thickness = (int)p.Object; }
            //    if (p.Name == "Border") { border = (int)p.Object; }
            //    if (p.Name == "Shadow") { shadow = (int)p.Object; }
            //    if (p.Name == "Front color") { front_c = (Color)p.Object; }
            //    if (p.Name == "Back color") { back_c = (Color)p.Object; }
            //    if (p.Name == "Thickness color") { thickness_c = (Color)p.Object; }
            //    if (p.Name == "Border color") { border_c = (Color)p.Object; }
            //    if (p.Name == "Shadow color") { shadow_c = (Color)p.Object; }
            //    if (p.Name == "Front rainbow") { }
            //    if (p.Name == "Back rainbow") { }
            //    if (p.Name == "Thickness rainbow") { }
            //    if (p.Name == "Border rainbow") { }
            //    if (p.Name == "Shadow rainbow") { }
            //}

            //PathObject path = new PathObject();

            //foreach (CreationObject cro in v.Objects)
            //{

            //    PointF center = GetVolumeGravityCenter(cro);
            //    GraphicsPath gp = path.FromArray(cro.Array, (int)center.X, (int)center.Y);

            //    if (gp != null)
            //    {
            //        g.ResetTransform();
            //        g.TranslateTransform(center.X, center.Y);

            //        g.RotateTransform(angleZ);
            //        g.ScaleTransform(scaleX / 100, scaleY / 100);

            //        //g.FillPath(new SolidBrush(front_c), gp);
            //        //g.DrawPath(new Pen(border_c, border), gp);

            //        //g.FillRectangle(Brushes.Red, center.X - 4, center.Y - 4, 8f, 8f); Don't look it or drink before to view it

            //        //GraphicsPath mirror = GetMirror(g, cro.Array, center);
            //        //g.FillPath(new SolidBrush(back_c), mirror);

            //        GraphicsPath gp_2 = GetRotationXY(cro.Array, center, angleX, angleY);
            //        Random random = new Random();

            //        if (quakeX > 0)
            //        {
            //            quakeX = random.Next(-(int)quakeX, (int)quakeX + 1);
            //        }

            //        if (quakeY > 0)
            //        {
            //            quakeY = random.Next(-(int)quakeY, (int)quakeY + 1);
            //        }

            //        g.TranslateTransform(posX + quakeX, posY + quakeY);

            //        if (shadow > 0)
            //        {
            //            g.TranslateTransform(shadow, shadow);
            //            g.FillPath(new SolidBrush(shadow_c), gp_2);
            //            g.TranslateTransform(-shadow, -shadow);
            //        }

            //        g.FillPath(new SolidBrush(front_c), gp_2);

            //        if (border > 0)
            //        {
            //            g.DrawPath(new Pen(border_c, border), gp_2);
            //        }


            //    }
            //}

        }

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

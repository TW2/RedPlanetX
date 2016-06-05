using RedPlanetXv8.Composition.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace RedPlanetXv8.Composition
{
    public class ShapeObject
    {
        private List<Group> groups = new List<Group>();
        private Dictionary<ShapeProfile, long> _moments = new Dictionary<ShapeProfile, long>();

        //private int _angle_x = 0;
        //private int _angle_y = 0;
        //private int _angle_z = 0;
        //private Color _front_color = Color.AliceBlue;
        //private Color _back_color = Color.AliceBlue; //(unused)
        //private Color _border_color = Color.Red;
        //private Color _shadow_color = Color.Gray;
        //private float _relative_position_x = 0f;
        //private float _relative_position_y = 0f;
        //private int _quake_x = 0;
        //private int _quake_y = 0;
        //private float _scale_x = 100f; //de 0.0 à 100.0
        //private float _scale_y = 100f; //de 0.0 à 100.0
        //private int _border_weight = 1;
        //private int _shadow_depth = 0;

        private long START = 0L;
        public long Start
        {
            get { return START; }
            set { START = value; }
        }
        private long END = 5000L;
        public long End
        {
            get { return END; }
            set { END = value; }
        }

        public ShapeObject()
        {

        }

        public ShapeObject(List<Group> groups)
        {
            this.groups = groups;
        }

        //public void AddMoment(long time)
        //{
        //    _moments.Add(new ShapeProfile(), time);
        //}

        public void AddStart(long time)
        {
            _moments.Add(new ShapeProfile(), time);
            START = time;
        }

        public void AddEnd(long time)
        {
            _moments.Add(new ShapeProfile(), time);
            END = time;
        }

        public Dictionary<ShapeProfile, long> GetMoments()
        {
            return _moments;
        }

        #region Draw
        public void Draw(Graphics g, long approximative_time)
        {
            if (approximative_time >= START && approximative_time <= END)
            {
                int _angle_x = GetAngleX(approximative_time);
                int _angle_y = GetAngleY(approximative_time);
                int _angle_z = GetAngleZ(approximative_time);
                Color _front_color = GetFrontColor(approximative_time);
                Color _back_color = GetBackColor(approximative_time); //(unused)
                Color _border_color = GetBorderColor(approximative_time);
                Color _shadow_color = GetShadowColor(approximative_time);
                float _relative_position_x = GetRelativePositionX(approximative_time);
                float _relative_position_y = GetRelativePositionY(approximative_time);
                int _quake_x = GetQuakeX(approximative_time);
                int _quake_y = GetQuakeY(approximative_time);
                float _scale_x = GetScaleX(approximative_time); //de 0.0 à 100.0
                float _scale_y = GetScaleY(approximative_time); //de 0.0 à 100.0
                int _border_weight = GetBorderWeight(approximative_time);
                int _shadow_depth = GetShadowDepth(approximative_time);

                PointF center = GetGravityCenter(groups);

                List<GraphicsPath> GPS = GetPaths(groups, center, _angle_x, _angle_y);
                foreach (GraphicsPath gp in GPS)
                {
                    g.ResetTransform();
                    g.TranslateTransform(center.X, center.Y);

                    g.RotateTransform(_angle_z);
                    g.ScaleTransform(_scale_x / 100, _scale_y / 100);

                    Random random = new Random();

                    if (_quake_x > 0)
                    {
                        _quake_x = random.Next(-_quake_x, _quake_x + 1);
                    }

                    if (_quake_y > 0)
                    {
                        _quake_y = random.Next(-_quake_y, _quake_y + 1);
                    }

                    g.TranslateTransform(_relative_position_x + _quake_x, _relative_position_y + _quake_y);

                    if (_shadow_depth > 0)
                    {
                        g.TranslateTransform(_shadow_depth, _shadow_depth);
                        g.FillPath(new SolidBrush(_shadow_color), gp);
                        g.TranslateTransform(-_shadow_depth, -_shadow_depth);
                    }

                    g.FillPath(new SolidBrush(_front_color), gp);

                    if (_border_weight > 0)
                    {
                        g.DrawPath(new Pen(_border_color, _border_weight), gp);
                    }
                }
            }
        }
        #endregion

        #region Path
        private List<GraphicsPath> GetPaths(List<Group> groups, PointF gravity, float angleX, float angleY)
        {
            // 0 - 90 -> ScaleX 100 - 0 (quartType = 1)
            // 90 - 180 -> Reverse+Scale X 0 - 100 (quartType = 2)
            // 180 - 270 -> Reverse+Scale X 100 - 0 (quartType = 3)
            // 270 - 360 -> ScaleX 0 - 100 (quartType = 4)

            int quartTypeX = GetRotationXYQuartType(angleX);
            int quartTypeY = GetRotationXYQuartType(angleY);
            float percentX = GetRotationXYPercent(angleX);
            float percentY = GetRotationXYPercent(angleY);

            List<GraphicsPath> gps = new List<GraphicsPath>();

            foreach (Group g in groups)
            {
                GraphicsPath gp = new GraphicsPath();

                foreach (IGraphicObject igo in g.GetGroup())
                {
                    if (igo.GetType() == typeof(Line))
                    {
                        Line l = (Line)igo;

                        if (l.End.IsEmpty == false)
                        {
                            float newStartX = GetRotationXYPosition(quartTypeX, percentX, gravity.X, l.Start.X);
                            float newStartY = GetRotationXYPosition(quartTypeY, percentY, gravity.Y, l.Start.Y);

                            float newStopX = GetRotationXYPosition(quartTypeX, percentX, gravity.X, l.End.X);
                            float newStopY = GetRotationXYPosition(quartTypeY, percentY, gravity.Y, l.End.Y);

                            gp.AddLine(
                                new PointF(newStartX, newStartY),
                                new PointF(newStopX, newStopY));
                        }
                    }

                    if (igo.GetType() == typeof(Curve))
                    {
                        Curve b = (Curve)igo;

                        if (b.End.IsEmpty == false)
                        {
                            float newStartX = GetRotationXYPosition(quartTypeX, percentX, gravity.X, b.Start.X);
                            float newStartY = GetRotationXYPosition(quartTypeY, percentY, gravity.Y, b.Start.Y);

                            float newCP1X = GetRotationXYPosition(quartTypeX, percentX, gravity.X, b.CP1.X);
                            float newCP1Y = GetRotationXYPosition(quartTypeY, percentY, gravity.Y, b.CP1.Y);

                            float newCP2X = GetRotationXYPosition(quartTypeX, percentX, gravity.X, b.CP2.X);
                            float newCP2Y = GetRotationXYPosition(quartTypeY, percentY, gravity.Y, b.CP2.Y);

                            float newStopX = GetRotationXYPosition(quartTypeX, percentX, gravity.X, b.End.X);
                            float newStopY = GetRotationXYPosition(quartTypeY, percentY, gravity.Y, b.End.Y);

                            gp.AddBezier(
                                new PointF(newStartX, newStartY),
                                new PointF(newCP1X, newCP1Y),
                                new PointF(newCP2X, newCP2Y),
                                new PointF(newStopX, newStopY));
                        }
                    }
                }

                if (gp.PointCount > 0)
                {
                    gps.Add(gp);
                }
            }

            return gps;
        }
        #endregion

        #region GravityCenter
        // source @ http://stackoverflow.com/questions/9815699/how-to-calculate-centroid
        private PointF GetGravityCenter(List<Group> groups)
        {
            List<PointF> poly = new List<PointF>();
            PointF p = new PointF(0, 0);
            Point lastp = new Point(-1, -1);
            bool hasChanged = false;

            foreach (Group group in groups)
            {
                foreach (IGraphicObject igo in group.GetGroup())
                {
                    if (igo.GetType() == typeof(Line))
                    {
                        Line l = (Line)igo;
                        if (l.Start != lastp)
                        {
                            poly.Add(new PointF(l.Start.X, l.Start.Y));
                            lastp = l.Start;
                        }

                        if (l.End != lastp)
                        {
                            poly.Add(new PointF(l.End.X, l.End.Y));
                            lastp = l.End;
                        }
                    }
                    else if (igo.GetType() == typeof(Curve))
                    {
                        Curve b = (Curve)igo;
                        if (b.Start != lastp)
                        {
                            poly.Add(new PointF(b.Start.X, b.Start.Y));
                            lastp = b.Start;
                        }

                        if (b.End != lastp)
                        {
                            poly.Add(new PointF(b.End.X, b.End.Y));
                            lastp = b.End;
                        }
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
                hasChanged = true;
            }

            // source @ http://mathcentral.uregina.ca/QQ/database/QQ.09.07/h/david7.html
            // source @ http://mathworld.wolfram.com/GeometricCentroid.html
            if (hasChanged == false)
            {
                float cumulX = 0f, cumulY = 0f, X = 0f, Y = 0f;
                for (int i = 0; i < poly.Count; i++)
                {
                    cumulX += poly[i].X;
                    cumulY += poly[i].Y;
                }
                X = cumulX / poly.Count;
                Y = cumulY / poly.Count;
                p = new PointF(X, Y);
            }

            return p;
        }
        #endregion

        #region Rotation
        private float GetRotationXYPercent(float angle)
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

        private int GetRotationXYQuartType(float angle)
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

        private float GetRotationXYPosition(int quartType, float percent, float gravity, int posBefore)
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
        #endregion

        public List<Group> Groups
        {
            get { return groups; }
        }

        #region Angle GET SET
        public void SetAngleX(long time, int angle)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.AngleX = angle; }
            }
        }

        public int GetAngleX(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            int angleX_min = list[0].AngleX, angleX_max = list[1].AngleX;

            if (lastMinValue == approximative_time)
            {
                return angleX_min;
            }

            if (lastMaxValue == approximative_time)
            {
                return angleX_max;
            }

            if (lastMinValue == 0)
            { //Particular case
                int diff = angleX_max - angleX_min;
                int delta = (int)(diff * approximative_time / lastMaxValue);
                return angleX_min + delta;
            }
            else
            {
                int diff = angleX_max - angleX_min;
                int delta = (int)(diff * (approximative_time - lastMinValue) / (lastMaxValue - lastMinValue));
                return angleX_min + delta;
            }
        }

        public void SetAngleY(long time, int angle)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.AngleY = angle; }
            }
        }

        public int GetAngleY(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            int angleY_min = list[0].AngleY, angleY_max = list[1].AngleY;

            if (lastMinValue == approximative_time)
            {
                return angleY_min;
            }

            if (lastMaxValue == approximative_time)
            {
                return angleY_max;
            }

            if (lastMinValue == 0)
            { //Particular case
                int diff = angleY_max - angleY_min;
                int delta = (int)(diff * approximative_time / lastMaxValue);
                return angleY_min + delta;
            }
            else
            {
                int diff = angleY_max - angleY_min;
                int delta = (int)(diff * (approximative_time - lastMinValue) / (lastMaxValue - lastMinValue));
                return angleY_min + delta;
            }
        }

        public void SetAngleZ(long time, int angle)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.AngleZ = angle; }
            }
        }

        public int GetAngleZ(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            int angleZ_min = list[0].AngleZ, angleZ_max = list[1].AngleZ;

            if (lastMinValue == approximative_time)
            {
                return angleZ_min;
            }

            if (lastMaxValue == approximative_time)
            {
                return angleZ_max;
            }

            if (lastMinValue == 0)
            { //Particular case
                int diff = angleZ_max - angleZ_min;
                int delta = (int)(diff * approximative_time / lastMaxValue);
                return angleZ_min + delta;
            }
            else
            {
                int diff = angleZ_max - angleZ_min;
                int delta = (int)(diff * (approximative_time - lastMinValue) / (lastMaxValue - lastMinValue));
                return angleZ_min + delta;
            }
        }
        #endregion

        #region Color GET SET
        public void SetFrontColor(long time, Color c)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.FrontColor = c; }
            }
        }

        public Color GetFrontColor(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            Color c_min = list[0].FrontColor, c_max = list[1].FrontColor;

            int diff_R = c_max.R - c_min.R;
            int diff_G = c_max.G - c_min.G;
            int diff_B = c_max.B - c_min.B;

            approximative_time = lastMinValue == 0 ? approximative_time : approximative_time - lastMinValue;
            lastMaxValue = lastMinValue == 0 ? lastMaxValue : lastMaxValue - lastMinValue;

            int delta_R = (int)(diff_R * approximative_time / lastMaxValue);
            int delta_G = (int)(diff_G * approximative_time / lastMaxValue);
            int delta_B = (int)(diff_B * approximative_time / lastMaxValue);

            int r = c_min.R + delta_R > 0 ? c_min.R + delta_R : 0; r = r > 255 ? 255 : r;
            int g = c_min.G + delta_G > 0 ? c_min.G + delta_G : 0; g = g > 255 ? 255 : g;
            int b = c_min.B + delta_B > 0 ? c_min.B + delta_B : 0; b = b > 255 ? 255 : b;

            return Color.FromArgb(r, g, b);
        }

        public void SetBackColor(long time, Color c)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.BackColor = c; }
            }
        }

        public Color GetBackColor(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            Color c_min = list[0].BackColor, c_max = list[1].BackColor;

            int diff_R = c_max.R - c_min.R;
            int diff_G = c_max.G - c_min.G;
            int diff_B = c_max.B - c_min.B;

            approximative_time = lastMinValue == 0 ? approximative_time : approximative_time - lastMinValue;
            lastMaxValue = lastMinValue == 0 ? lastMaxValue : lastMaxValue - lastMinValue;

            int delta_R = (int)(diff_R * approximative_time / lastMaxValue);
            int delta_G = (int)(diff_G * approximative_time / lastMaxValue);
            int delta_B = (int)(diff_B * approximative_time / lastMaxValue);

            int r = c_min.R + delta_R > 0 ? c_min.R + delta_R : 0; r = r > 255 ? 255 : r;
            int g = c_min.G + delta_G > 0 ? c_min.G + delta_G : 0; g = g > 255 ? 255 : g;
            int b = c_min.B + delta_B > 0 ? c_min.B + delta_B : 0; b = b > 255 ? 255 : b;

            return Color.FromArgb(r, g, b);
        }

        public void SetBorderColor(long time, Color c)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.BorderColor = c; }
            }
        }

        public Color GetBorderColor(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            Color c_min = list[0].BorderColor, c_max = list[1].BorderColor;

            int diff_R = c_max.R - c_min.R;
            int diff_G = c_max.G - c_min.G;
            int diff_B = c_max.B - c_min.B;

            approximative_time = lastMinValue == 0 ? approximative_time : approximative_time - lastMinValue;
            lastMaxValue = lastMinValue == 0 ? lastMaxValue : lastMaxValue - lastMinValue;

            int delta_R = (int)(diff_R * approximative_time / lastMaxValue);
            int delta_G = (int)(diff_G * approximative_time / lastMaxValue);
            int delta_B = (int)(diff_B * approximative_time / lastMaxValue);

            int r = c_min.R + delta_R > 0 ? c_min.R + delta_R : 0; r = r > 255 ? 255 : r;
            int g = c_min.G + delta_G > 0 ? c_min.G + delta_G : 0; g = g > 255 ? 255 : g;
            int b = c_min.B + delta_B > 0 ? c_min.B + delta_B : 0; b = b > 255 ? 255 : b;

            return Color.FromArgb(r, g, b);
        }

        public void SetShadowColor(long time, Color c)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.ShadowColor = c; }
            }
        }

        public Color GetShadowColor(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            Color c_min = list[0].ShadowColor, c_max = list[1].ShadowColor;

            int diff_R = c_max.R - c_min.R;
            int diff_G = c_max.G - c_min.G;
            int diff_B = c_max.B - c_min.B;

            approximative_time = lastMinValue == 0 ? approximative_time : approximative_time - lastMinValue;
            lastMaxValue = lastMinValue == 0 ? lastMaxValue : lastMaxValue - lastMinValue;

            int delta_R = (int)(diff_R * approximative_time / lastMaxValue);
            int delta_G = (int)(diff_G * approximative_time / lastMaxValue);
            int delta_B = (int)(diff_B * approximative_time / lastMaxValue);

            int r = c_min.R + delta_R > 0 ? c_min.R + delta_R : 0; r = r > 255 ? 255 : r;
            int g = c_min.G + delta_G > 0 ? c_min.G + delta_G : 0; g = g > 255 ? 255 : g;
            int b = c_min.B + delta_B > 0 ? c_min.B + delta_B : 0; b = b > 255 ? 255 : b;

            return Color.FromArgb(r, g, b);
        }
        #endregion

        #region Relative Position GET SET
        public void SetRelativePositionX(long time, float pos)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.RelativePositionX = pos; }
            }
        }

        public float GetRelativePositionX(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            float pos_min = list[0].RelativePositionX, pos_max = list[1].RelativePositionX;

            if (lastMinValue == approximative_time)
            {
                return pos_min;
            }

            if (lastMaxValue == approximative_time)
            {
                return pos_max;
            }

            if (lastMinValue == 0)
            { //Particular case
                float diff = pos_max - pos_min;
                float delta = (float)(diff * approximative_time / lastMaxValue);
                return pos_min + delta;
            }
            else
            {
                float diff = pos_max - pos_min;
                float delta = (float)(diff * (approximative_time - lastMinValue) / (lastMaxValue - lastMinValue));
                return pos_min + delta;
            }
        }

        public void SetRelativePositionY(long time, float pos)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.RelativePositionY = pos; }
            }
        }

        public float GetRelativePositionY(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            float pos_min = list[0].RelativePositionY, pos_max = list[1].RelativePositionY;

            if (lastMinValue == approximative_time)
            {
                return pos_min;
            }

            if (lastMaxValue == approximative_time)
            {
                return pos_max;
            }

            if (lastMinValue == 0)
            { //Particular case
                float diff = pos_max - pos_min;
                float delta = (float)(diff * approximative_time / lastMaxValue);
                return pos_min + delta;
            }
            else
            {
                float diff = pos_max - pos_min;
                float delta = (float)(diff * (approximative_time - lastMinValue) / (lastMaxValue - lastMinValue));
                return pos_min + delta;
            }
        }
        #endregion

        #region Quake GET SET
        public void SetQuakeX(long time, int quake)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.QuakeX = quake; }
            }
        }

        public int GetQuakeX(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            int quake_min = list[0].QuakeX, quake_max = list[1].QuakeX;

            if (lastMinValue == approximative_time)
            {
                return quake_min;
            }

            if (lastMaxValue == approximative_time)
            {
                return quake_max;
            }

            if (lastMinValue == 0)
            { //Particular case
                int diff = quake_max - quake_min;
                int delta = (int)(diff * approximative_time / lastMaxValue);
                return quake_min + delta;
            }
            else
            {
                int diff = quake_max - quake_min;
                int delta = (int)(diff * (approximative_time - lastMinValue) / (lastMaxValue - lastMinValue));
                return quake_min + delta;
            }
        }

        public void SetQuakeY(long time, int quake)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.QuakeY = quake; }
            }
        }

        public int GetQuakeY(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            int quake_min = list[0].QuakeY, quake_max = list[1].QuakeY;

            if (lastMinValue == approximative_time)
            {
                return quake_min;
            }

            if (lastMaxValue == approximative_time)
            {
                return quake_max;
            }

            if (lastMinValue == 0)
            { //Particular case
                int diff = quake_max - quake_min;
                int delta = (int)(diff * approximative_time / lastMaxValue);
                return quake_min + delta;
            }
            else
            {
                int diff = quake_max - quake_min;
                int delta = (int)(diff * (approximative_time - lastMinValue) / (lastMaxValue - lastMinValue));
                return quake_min + delta;
            }
        }
        #endregion

        #region Scale GET SET
        public void SetScaleX(long time, float scale)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.ScaleX = scale; }
            }
        }

        public float GetScaleX(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            float scale_min = list[0].ScaleX, scale_max = list[1].ScaleX;

            if (lastMinValue == approximative_time)
            {
                return scale_min;
            }

            if (lastMaxValue == approximative_time)
            {
                return scale_max;
            }

            if (lastMinValue == 0)
            { //Particular case
                float diff = scale_max - scale_min;
                float delta = (float)(diff * approximative_time / lastMaxValue);
                return scale_min + delta;
            }
            else
            {
                float diff = scale_max - scale_min;
                float delta = (float)(diff * (approximative_time - lastMinValue) / (lastMaxValue - lastMinValue));
                return scale_min + delta;
            }
        }

        public void SetScaleY(long time, float scale)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.ScaleY = scale; }
            }
        }

        public float GetScaleY(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            float scale_min = list[0].ScaleY, scale_max = list[1].ScaleY;

            if (lastMinValue == approximative_time)
            {
                return scale_min;
            }

            if (lastMaxValue == approximative_time)
            {
                return scale_max;
            }

            if (lastMinValue == 0)
            { //Particular case
                float diff = scale_max - scale_min;
                float delta = (float)(diff * approximative_time / lastMaxValue);
                return scale_min + delta;
            }
            else
            {
                float diff = scale_max - scale_min;
                float delta = (float)(diff * (approximative_time - lastMinValue) / (lastMaxValue - lastMinValue));
                return scale_min + delta;
            }
        }
        #endregion

        #region Border and Shadow GET SET
        public void SetBorderWeight(long time, int b)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.BorderWeight = b; }
            }
        }

        public int GetBorderWeight(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            int b_min = list[0].BorderWeight, b_max = list[1].BorderWeight;

            if (lastMinValue == approximative_time)
            {
                return b_min;
            }

            if (lastMaxValue == approximative_time)
            {
                return b_max;
            }

            if (lastMinValue == 0)
            { //Particular case
                int diff = b_max - b_min;
                int delta = (int)(diff * approximative_time / lastMaxValue);
                return b_min + delta;
            }
            else
            {
                int diff = b_max - b_min;
                int delta = (int)(diff * (approximative_time - lastMinValue) / (lastMaxValue - lastMinValue));
                return b_min + delta;
            }
        }

        public void SetShadowDepth(long time, int b)
        {
            foreach (KeyValuePair<ShapeProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.ShadowDepth = b; }
            }
        }

        public int GetShadowDepth(long approximative_time)
        {
            List<ShapeProfile> list = new List<ShapeProfile>(_moments.Keys);
            long lastMinValue = START, lastMaxValue = END;
            int b_min = list[0].ShadowDepth, b_max = list[1].ShadowDepth;

            if (lastMinValue == approximative_time)
            {
                return b_min;
            }

            if (lastMaxValue == approximative_time)
            {
                return b_max;
            }

            if (lastMinValue == 0)
            { //Particular case
                int diff = b_max - b_min;
                int delta = (int)(diff * approximative_time / lastMaxValue);
                return b_min + delta;
            }
            else
            {
                int diff = b_max - b_min;
                int delta = (int)(diff * (approximative_time - lastMinValue) / (lastMaxValue - lastMinValue));
                return b_min + delta;
            }
        }
        #endregion
    }

    /*
    Quand un ShapeObject sert de classe de stockage pour l'instant t alors un ShapeProfile est un espace
    de stockage à l'instant défini sur une limite ou un checkpoint pour l'instant A(start) B(checkpoint)
    ou C(end). Le ShapeObject se servant des ShapeProfile(s) pour définir l'instant t.
    C'est pour ça qu'il ont à peu près les mêmes paramètres.
    */
    public class ShapeProfile
    {
        private long _moment_milliseconds = 0L;
        private int _angle_x = 0;
        private int _angle_y = 0;
        private int _angle_z = 0;
        private Color _front_color = Color.AliceBlue;
        private Color _back_color = Color.AliceBlue; //(unused)
        private Color _border_color = Color.Red;
        private Color _shadow_color = Color.Gray;
        private float _relative_position_x = 0f;
        private float _relative_position_y = 0f;
        private int _quake_x = 0;
        private int _quake_y = 0;
        private float _scale_x = 100f; //de 0.0 à 100.0
        private float _scale_y = 100f; //de 0.0 à 100.0
        private int _border_weight = 1;
        private int _shadow_depth = 0;

        public ShapeProfile()
        {

        }

        public ShapeProfile(long moment_milliseconds)
        {
            _moment_milliseconds = moment_milliseconds;
        }

        public long MomentMilliseconds
        {
            get { return _moment_milliseconds; }
            set { _moment_milliseconds = value; }
        }

        public int AngleX
        {
            get { return _angle_x; }
            set { _angle_x = value; }
        }

        public int AngleY
        {
            get { return _angle_y; }
            set { _angle_y = value; }
        }

        public int AngleZ
        {
            get { return _angle_z; }
            set { _angle_z = value; }
        }

        public Color FrontColor
        {
            get { return _front_color; }
            set { _front_color = value; }
        }

        public Color BackColor
        {
            get { return _back_color; }
            set { _back_color = value; }
        }

        public Color BorderColor
        {
            get { return _border_color; }
            set { _border_color = value; }
        }

        public Color ShadowColor
        {
            get { return _shadow_color; }
            set { _shadow_color = value; }
        }

        public float RelativePositionX
        {
            get { return _relative_position_x; }
            set { _relative_position_x = value; }
        }

        public float RelativePositionY
        {
            get { return _relative_position_y; }
            set { _relative_position_y = value; }
        }

        public int QuakeX
        {
            get { return _quake_x; }
            set { _quake_x = value; }
        }

        public int QuakeY
        {
            get { return _quake_y; }
            set { _quake_y = value; }
        }

        public float ScaleX
        {
            get { return _scale_x; }
            set { _scale_x = value; }
        }

        public float ScaleY
        {
            get { return _scale_y; }
            set { _scale_y = value; }
        }

        public int BorderWeight
        {
            get { return _border_weight; }
            set { _border_weight = value; }
        }

        public int ShadowDepth
        {
            get { return _shadow_depth; }
            set { _shadow_depth = value; }
        }
    }
}

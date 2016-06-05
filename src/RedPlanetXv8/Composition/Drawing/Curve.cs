using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Composition.Drawing
{
    public class Curve : IGraphicObject
    {
        private Curve _curveSelected = null;
        private Point _pointSelected = Point.Empty;

        public Curve()
        {

        }

        public Curve(Point _start)
        {
            Start = _start;
        }

        public static Curve CreateCurve(Point _start, Point _end)
        {
            Curve c = new Curve();

            int xdiff = _end.X - _start.X;
            int ydiff = _end.Y - _start.Y;

            int x1_3 = _start.X + xdiff / 3;
            int x2_3 = _start.X + xdiff * 2 / 3;
            int y1_3 = _start.Y + ydiff / 3;
            int y2_3 = _start.Y + ydiff * 2 / 3;

            c.CP1 = new Point(x1_3, y1_3);
            c.CP2 = new Point(x2_3, y2_3);

            c.Start = _start;
            c.End = _end;

            return c;
        }

        Point _Start = Point.Empty;

        public Point Start
        {
            get
            {
                return _Start;
            }
            set
            {
                _Start = value;
            }
        }

        Point _CP1 = Point.Empty;

        public Point CP1
        {
            get
            {
                return _CP1;
            }
            set
            {
                _CP1 = value;
            }
        }

        Point _CP2 = Point.Empty;

        public Point CP2
        {
            get
            {
                return _CP2;
            }
            set
            {
                _CP2 = value;
            }
        }

        Point _End = Point.Empty;

        public Point End
        {
            get
            {
                return _End;
            }
            set
            {
                _End = value;
            }
        }

        public void Draw(Graphics g)
        {
            if (Start.IsEmpty == false)
            {
                g.FillRectangle(Brushes.Blue, Start.X - 4, Start.Y - 4, 8, 8);
            }

            if (End.IsEmpty == false)
            {
                g.FillRectangle(Brushes.Blue, End.X - 4, End.Y - 4, 8, 8);
            }

            if (CP1.IsEmpty == false)
            {
                g.FillEllipse(Brushes.Orange, CP1.X - 4, CP1.Y - 4, 8, 8);
            }

            if (CP2.IsEmpty == false)
            {
                g.FillEllipse(Brushes.Orange, CP2.X - 4, CP2.Y - 4, 8, 8);
            }

            using (Pen dashed = new Pen(Brushes.Violet, 1.5f))
            {
                dashed.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                if (Start.IsEmpty == false && CP1.IsEmpty == false)
                {
                    g.DrawLine(dashed, Start, CP1);
                }

                if (CP1.IsEmpty == false && CP2.IsEmpty == false)
                {
                    g.DrawLine(dashed, CP1, CP2);
                }

                if (CP2.IsEmpty == false && End.IsEmpty == false)
                {
                    g.DrawLine(dashed, CP2, End);
                }
            }

            if (Start.IsEmpty == false && CP1.IsEmpty == false && CP2.IsEmpty == false && End.IsEmpty == false)
            {
                g.DrawBezier(Pens.Magenta, Start, CP1, CP2, End);
            }

            if (_pointSelected.IsEmpty == false)
            {
                using (Pen pen = new Pen(Color.Violet, 3f))
                {
                    if (IsStartSelected(_pointSelected, _curveSelected) == true)
                    {
                        g.DrawRectangle(pen, _curveSelected.Start.X - 8, _curveSelected.Start.Y - 8, 16, 16);
                    }

                    if (IsCP1Selected(_pointSelected, _curveSelected) == true)
                    {
                        g.DrawEllipse(pen, _curveSelected.CP1.X - 8, _curveSelected.CP1.Y - 8, 16, 16);
                    }

                    if (IsCP2Selected(_pointSelected, _curveSelected) == true)
                    {
                        g.DrawEllipse(pen, _curveSelected.CP2.X - 8, _curveSelected.CP2.Y - 8, 16, 16);
                    }

                    if (IsEndSelected(_pointSelected, _curveSelected) == true)
                    {
                        g.DrawRectangle(pen, _curveSelected.End.X - 8, _curveSelected.End.Y - 8, 16, 16);
                    }
                }
            }
        }

        public static bool ContainsPoint(Point p, Curve curve)
        {
            Rectangle selection = new Rectangle(p.X - 4, p.Y - 4, 8, 8);

            if (selection.Contains(curve.Start) | selection.Contains(curve.CP1) | selection.Contains(curve.CP2) | selection.Contains(curve.End))
            {
                return true;
            }
            return false;
        }

        public static bool IsStartSelected(Point p, Curve curve)
        {
            Rectangle selection = new Rectangle(p.X - 4, p.Y - 4, 8, 8);

            if (selection.Contains(curve.Start))
            {
                return true;
            }
            return false;
        }

        public static bool IsCP1Selected(Point p, Curve curve)
        {
            Rectangle selection = new Rectangle(p.X - 4, p.Y - 4, 8, 8);

            if (selection.Contains(curve.CP1))
            {
                return true;
            }
            return false;
        }

        public static bool IsCP2Selected(Point p, Curve curve)
        {
            Rectangle selection = new Rectangle(p.X - 4, p.Y - 4, 8, 8);

            if (selection.Contains(curve.CP2))
            {
                return true;
            }
            return false;
        }

        public static bool IsEndSelected(Point p, Curve curve)
        {
            Rectangle selection = new Rectangle(p.X - 4, p.Y - 4, 8, 8);

            if (selection.Contains(curve.End))
            {
                return true;
            }
            return false;
        }

        public Curve CurveSelected
        {
            get
            {
                return _curveSelected;
            }
            set
            {
                _curveSelected = value;
            }
        }

        public Point PointSelected
        {
            get
            {
                return _pointSelected;
            }
            set
            {
                _pointSelected = value;
            }
        }
    }
}

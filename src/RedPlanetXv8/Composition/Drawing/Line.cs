using System.Drawing;

namespace RedPlanetXv8.Composition.Drawing
{
    public class Line : IGraphicObject
    {
        private Line _lineSelected = null;
        private Point _pointSelected = Point.Empty;

        public Line()
        {

        }

        public Line(Point _start)
        {
            Start = _start;
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

            if (Start.IsEmpty == false && End.IsEmpty == false)
            {
                g.DrawLine(Pens.Red, Start, End);
            }

            if (_pointSelected.IsEmpty == false)
            {
                using (Pen pen = new Pen(Color.Violet, 3f))
                {
                    if (IsStartSelected(_pointSelected, _lineSelected) == true)
                    {
                        g.DrawRectangle(pen, _lineSelected.Start.X - 8, _lineSelected.Start.Y - 8, 16, 16);
                    }

                    if (IsEndSelected(_pointSelected, _lineSelected) == true)
                    {
                        g.DrawRectangle(pen, _lineSelected.End.X - 8, _lineSelected.End.Y - 8, 16, 16);
                    }
                }
            }
        }

        public static bool ContainsPoint(Point p, Line line)
        {
            Rectangle selection = new Rectangle(p.X - 4, p.Y - 4, 8, 8);

            if (selection.Contains(line.Start) | selection.Contains(line.End))
            {
                return true;
            }
            return false;
        }

        public static bool IsStartSelected(Point p, Line line)
        {
            Rectangle selection = new Rectangle(p.X - 4, p.Y - 4, 8, 8);

            if (selection.Contains(line.Start))
            {
                return true;
            }
            return false;
        }

        public static bool IsEndSelected(Point p, Line line)
        {
            Rectangle selection = new Rectangle(p.X - 4, p.Y - 4, 8, 8);

            if (selection.Contains(line.End))
            {
                return true;
            }
            return false;
        }

        public Line LineSelected
        {
            get
            {
                return _lineSelected;
            }
            set
            {
                _lineSelected = value;
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

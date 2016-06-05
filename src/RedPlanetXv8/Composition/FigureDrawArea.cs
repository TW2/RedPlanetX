using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedPlanetXv8.Composition
{
    public class FigureDrawArea : Panel
    {
        private List<Drawing.Group> groups = new List<Drawing.Group>();
        private Drawing.Group currentGroup = new Drawing.Group();
        private Drawing.IGraphicObject currentObject = new Drawing.Line();
        private bool _isLine = true;
        private bool _isCurve = false;
        private List<Drawing.Line> _linesSelected = new List<Drawing.Line>();
        private List<Drawing.Curve> _curvesSelected = new List<Drawing.Curve>();
        private Point _pointSelected = Point.Empty;

        public FigureDrawArea()
        {
            this.DoubleBuffered = true;
        }

        /// <summary>
        /// Define the current group (active it in the drawer).
        /// </summary>
        public void AddGroup()
        {
            groups.Add(currentGroup);
        }

        /// <summary>
        /// Close current group and add a new one.
        /// </summary>
        public void NewGroup()
        {
            currentGroup = new Drawing.Group();
        }

        public List<Drawing.Group> GetGroups()
        {
            return groups;
        }

        public void SetGroups(List<Drawing.Group> gs)
        {
            groups = gs;
        }

        public void SetLineMode()
        {
            _isLine = true;
            _isCurve = false;
        }

        public void SetCurveMode()
        {
            _isLine = false;
            _isCurve = true;
        }

        #region Paint
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            //==================================================================
            // DRAW THE BACKGROUND
            //==================================================================
            int sizeToPaintX = Width;
            int sizeToPaintY = Height;
            Graphics g = e.Graphics;

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

            //==================================================================
            // DRAW THE FOREGROUND (particules and figures)
            //==================================================================

            foreach (Drawing.Group group in groups)
            {
                group.Draw(g);
            }
        }
        #endregion

        #region MouseClick
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (_isLine && currentObject.GetType() == typeof(Drawing.Line))
            {
                Drawing.Line line = (Drawing.Line)currentObject;

                if (e.Button == MouseButtons.Left)
                {
                    if (currentGroup.GetGroup().Count == 0)
                    {
                        AddGroup();
                        currentObject = new Drawing.Line();

                        Drawing.Line l = (Drawing.Line)currentObject;
                        l.Start = e.Location;
                        currentGroup.AddLine(l);
                        Refresh();
                    }
                    else if (line.Start.IsEmpty == false && line.End.IsEmpty == true)
                    {
                        line.End = e.Location;

                        currentObject = new Drawing.Line();

                        Drawing.Line l = (Drawing.Line)currentObject;
                        l.Start = e.Location;
                        currentGroup.AddLine(l);
                        Refresh();
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    NewGroup();
                }
            }
            else if (_isLine && currentObject.GetType() == typeof(Drawing.Curve))
            {
                Drawing.Curve curve = (Drawing.Curve)currentObject;
                Point lastMouse = curve.Start;
                currentGroup.RemoveObject(currentObject);
                currentObject = new Drawing.Line(lastMouse);
                Drawing.Line line = (Drawing.Line)currentObject;

                if (e.Button == MouseButtons.Left)
                {
                    if (currentGroup.GetGroup().Count == 0)
                    {
                        AddGroup();
                        currentObject = new Drawing.Line();

                        Drawing.Line l = (Drawing.Line)currentObject;
                        l.Start = e.Location;
                        currentGroup.AddLine(l);
                        Refresh();
                    }
                    else if (line.Start.IsEmpty == false && line.End.IsEmpty == true)
                    {
                        line.End = e.Location;
                        currentGroup.AddLine(line);

                        currentObject = new Drawing.Line();

                        Drawing.Line l = (Drawing.Line)currentObject;
                        l.Start = e.Location;
                        currentGroup.AddLine(l);
                        Refresh();
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    NewGroup();
                }
            }



            if (_isCurve && currentObject.GetType() == typeof(Drawing.Curve))
            {
                Drawing.Curve curve = (Drawing.Curve)currentObject;

                if (e.Button == MouseButtons.Left)
                {
                    if (currentGroup.GetGroup().Count == 0)
                    {
                        AddGroup();
                        currentObject = new Drawing.Curve();

                        Drawing.Curve c = (Drawing.Curve)currentObject;
                        c.Start = e.Location;
                        currentGroup.AddCurve(c);
                        Refresh();
                    }
                    else if (curve.Start.IsEmpty == false && curve.End.IsEmpty == true)
                    {
                        Drawing.Curve cv = Drawing.Curve.CreateCurve(curve.Start, e.Location);
                        curve.CP1 = cv.CP1;
                        curve.CP2 = cv.CP2;
                        curve.End = cv.End;

                        currentObject = new Drawing.Curve();

                        Drawing.Curve c = (Drawing.Curve)currentObject;
                        c.Start = e.Location;
                        currentGroup.AddCurve(c);
                        Refresh();
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    NewGroup();
                }
            }
            else if (_isCurve && currentObject.GetType() == typeof(Drawing.Line))
            {
                Drawing.Line line = (Drawing.Line)currentObject;
                Point lastMouse = line.Start;
                currentGroup.RemoveObject(currentObject);
                currentObject = new Drawing.Curve(lastMouse);
                Drawing.Curve curve = (Drawing.Curve)currentObject;

                if (e.Button == MouseButtons.Left)
                {
                    if (currentGroup.GetGroup().Count == 0)
                    {
                        AddGroup();
                        currentObject = new Drawing.Curve();

                        Drawing.Curve c = (Drawing.Curve)currentObject;
                        c.Start = e.Location;
                        currentGroup.AddCurve(c);
                        Refresh();
                    }
                    else if (curve.Start.IsEmpty == false && curve.End.IsEmpty == true)
                    {
                        Drawing.Curve cv = Drawing.Curve.CreateCurve(curve.Start, e.Location);
                        curve.CP1 = cv.CP1;
                        curve.CP2 = cv.CP2;
                        curve.End = cv.End;
                        currentGroup.AddCurve(curve);

                        currentObject = new Drawing.Curve();

                        Drawing.Curve c = (Drawing.Curve)currentObject;
                        c.Start = e.Location;
                        currentGroup.AddCurve(c);
                        Refresh();
                    }
                }
                else if (e.Button == MouseButtons.Right)
                {
                    NewGroup();
                }
            }
        }
        #endregion

        #region MouseDown
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (e.Button == MouseButtons.Middle)
            {
                foreach (Drawing.Group g in groups)
                {
                    List<Drawing.Line> lines = Drawing.Group.GetLinesOfPoint(e.Location, g);
                    List<Drawing.Curve> curves = Drawing.Group.GetCurvesOfPoint(e.Location, g);

                    _pointSelected = e.Location;

                    foreach (Drawing.Line line in lines)
                    {
                        _linesSelected.Add(line);
                        line.LineSelected = line;
                        line.PointSelected = e.Location;
                    }

                    foreach (Drawing.Curve curve in curves)
                    {
                        _curvesSelected.Add(curve);
                        curve.CurveSelected = curve;
                        curve.PointSelected = e.Location;
                    }
                }
                Refresh();
            }
        }
        #endregion

        #region MouseUp
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Middle)
            {
                foreach (Drawing.Group g in groups)
                {
                    List<Drawing.Line> lines = Drawing.Group.GetLinesOfPoint(e.Location, g);
                    List<Drawing.Curve> curves = Drawing.Group.GetCurvesOfPoint(e.Location, g);

                    foreach (Drawing.Line line in lines)
                    {
                        line.LineSelected = null;
                        line.PointSelected = Point.Empty;
                    }

                    foreach (Drawing.Curve curve in curves)
                    {
                        curve.CurveSelected = null;
                        curve.PointSelected = Point.Empty;
                    }

                    _linesSelected.Clear();
                    _curvesSelected.Clear();
                    _pointSelected = Point.Empty;
                }
                Refresh();
            }
        }
        #endregion

        #region MouseMove
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Middle)
            {
                if (_linesSelected.Count != 0 && _curvesSelected.Count != 0)
                {
                    foreach (Drawing.Line line in _linesSelected)
                    {
                        if (Drawing.Line.IsStartSelected(_pointSelected, line))
                        {
                            line.Start = e.Location;
                        }

                        if (Drawing.Line.IsEndSelected(_pointSelected, line))
                        {
                            line.End = e.Location;
                        }
                    }

                    foreach (Drawing.Curve curve in _curvesSelected)
                    {
                        if (Drawing.Curve.IsStartSelected(_pointSelected, curve))
                        {
                            curve.Start = e.Location;
                        }

                        if (Drawing.Curve.IsCP1Selected(_pointSelected, curve))
                        {
                            curve.CP1 = e.Location;
                        }

                        if (Drawing.Curve.IsCP2Selected(_pointSelected, curve))
                        {
                            curve.CP2 = e.Location;
                        }

                        if (Drawing.Curve.IsEndSelected(_pointSelected, curve))
                        {
                            curve.End = e.Location;
                        }
                    }


                }
                else if (_linesSelected.Count != 0)
                {
                    foreach (Drawing.Line line in _linesSelected)
                    {
                        if (Drawing.Line.IsStartSelected(_pointSelected, line))
                        {
                            line.Start = e.Location;
                        }

                        if (Drawing.Line.IsEndSelected(_pointSelected, line))
                        {
                            line.End = e.Location;
                        }
                    }
                }
                else if (_curvesSelected.Count != 0)
                {
                    foreach (Drawing.Curve curve in _curvesSelected)
                    {
                        if (Drawing.Curve.IsStartSelected(_pointSelected, curve))
                        {
                            curve.Start = e.Location;
                        }

                        if (Drawing.Curve.IsCP1Selected(_pointSelected, curve))
                        {
                            curve.CP1 = e.Location;
                        }

                        if (Drawing.Curve.IsCP2Selected(_pointSelected, curve))
                        {
                            curve.CP2 = e.Location;
                        }

                        if (Drawing.Curve.IsEndSelected(_pointSelected, curve))
                        {
                            curve.End = e.Location;
                        }
                    }
                }
                _pointSelected = e.Location;
                Refresh();
            }

        }
        #endregion
    }
}

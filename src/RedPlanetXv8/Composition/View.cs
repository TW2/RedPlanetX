using RedPlanetXv8.ASS;
using RedPlanetXv8.Node;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedPlanetXv8.Composition
{
    public class View : Panel
    {
        private Bitmap _bitmap = null;
        private Size _size = new Size(640, 480);
        private long _approximative_time = 0L;
        private AssScript _asss = null;
        private double _fps = 23.976d;
        private Dictionary<ShapeTreeNode, ParentTreeNode> _shapelink = new Dictionary<ShapeTreeNode, ParentTreeNode>();
        private PathConstructorType _PathCType = PathConstructorType.None;
        private List<PathTreeNode> _pathlist = new List<PathTreeNode>();
        private PathTreeNode _last_pathtreenode = null;
        private Drawing.Curve _last_pathcurve = null;
        private List<Drawing.Line> _linesSelected = new List<Drawing.Line>();
        private List<Drawing.Curve> _curvesSelected = new List<Drawing.Curve>();
        private Point _pointSelected = Point.Empty;
        private List<Drawing.Path> _pathgroups = new List<Drawing.Path>();

        public View()
        {
            DoubleBuffered = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_bitmap != null)
            {
                e.Graphics.DrawImage(_bitmap, e.ClipRectangle);
            }
            else
            {
                e.Graphics.Clear(Color.LightBlue);
            }

            if(_asss != null)
            {
                foreach(Sentence sent in _asss.Sentences)
                {
                    sent.PrepareSentence(e.Graphics);

                    foreach (Syllable syl in sent.Syllables)
                    {
                        foreach (Letter let in syl.Letters)
                        {
                            let.DrawPlainTextWithoutKaraoke(e.Graphics, _approximative_time);
                            let.DrawPlainTextWithKaraoke(e.Graphics, _approximative_time);
                        }
                    }
                }
            }

            foreach (KeyValuePair<ShapeTreeNode, ParentTreeNode> pair in _shapelink)
            {
                pair.Key.ShapeObject.Draw(e.Graphics, _approximative_time);
            }

            //Montre le chemin emprunté par les formes liées (show shape path)
            foreach(PathTreeNode ptn in _pathlist)
            {
                PathObject pathobj = ptn.PathObject;
                if(pathobj.Hide == false)
                {
                    pathobj.Path.Draw(e.Graphics);
                }
            }

        }

        private void ReorderPathList()
        {
            _pathgroups.Clear();

            foreach(PathTreeNode ptn in _pathlist)
            {
                _pathgroups.Add(ptn.PathObject.Path);
            }
        }

        #region MouseClick
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (_PathCType == PathConstructorType.AddStart)
            {
                if (e.Button == MouseButtons.Left)
                {
                    _last_pathtreenode.PathObject.Path.Insert.InsertPoint = e.Location;

                    //Un point d'insert est le premier point d'un chemin (insert point is first point of path)
                    //Tous les chemins sont ici composés de courbes (all paths are composed by curves)
                    //Donc il est nécessaire de définir le premier point (so it's necessary to define first point)
                    //C'est juste au cas où (it's just a precaution)
                    //Sinon on décale tout (else we translate all)
                    if (_last_pathtreenode.PathObject.Path.GetGroup().Count == 0)
                    {
                        _last_pathcurve = new Drawing.Curve();

                        Drawing.Curve c = _last_pathcurve;
                        c.Start = e.Location;
                        _last_pathtreenode.PathObject.Path.AddCurve(c);
                    }
                    else
                    {
                        Point inverseLocationPoint = Point.Empty;
                        int counter = 0;

                        foreach(Drawing.IGraphicObject igo in _last_pathtreenode.PathObject.Path.GetGroup())
                        {
                            
                            if(igo.GetType() == typeof(Drawing.Curve))
                            {
                                counter++;
                                Drawing.Curve c = (Drawing.Curve)igo;
                                if(inverseLocationPoint.IsEmpty == true)
                                {
                                    //Ceci est notre point de référence
                                    //(this is our reference point)
                                    //Si le premier point de toutes les courbes est vide
                                    //(if first point of all curves is void)
                                    Point notReal = c.Start;
                                    Point real = e.Location;
                                    inverseLocationPoint.X = notReal.X - real.X;
                                    inverseLocationPoint.Y = notReal.Y - real.Y;
                                }

                                if (inverseLocationPoint.IsEmpty == false 
                                    && counter <= _last_pathtreenode.PathObject.Path.GetGroup().Count - 1)
                                {
                                    //Sinon on décale tout (else we translate all)
                                    //A l'aide du point inverse (with this inverse point)
                                    //A l'exception de la dernière courbe (except the last curve)
                                    //Car pour la dernière courbe seule le point de début est défini
                                    //(for the last curve only the first point is defined)
                                    c.Start = new Point(
                                        c.Start.X - inverseLocationPoint.X,
                                        c.Start.Y - inverseLocationPoint.Y);
                                    c.CP1 = new Point(
                                        c.CP1.X - inverseLocationPoint.X,
                                        c.CP1.Y - inverseLocationPoint.Y);
                                    c.CP2 = new Point(
                                        c.CP2.X - inverseLocationPoint.X,
                                        c.CP2.Y - inverseLocationPoint.Y);
                                    c.End = new Point(
                                        c.End.X - inverseLocationPoint.X,
                                        c.End.Y - inverseLocationPoint.Y);
                                }

                                if (inverseLocationPoint.IsEmpty == false
                                    && counter == _last_pathtreenode.PathObject.Path.GetGroup().Count)
                                {
                                    //Sinon on décale tout (else we translate all)
                                    //A l'aide du point inverse (with this inverse point)
                                    //A l'exception de la dernière courbe (except the last curve)
                                    //Car pour la dernière courbe seule le point de début est défini
                                    //(for the last curve only the first point is defined)
                                    c.Start = new Point(
                                        c.Start.X - inverseLocationPoint.X,
                                        c.Start.Y - inverseLocationPoint.Y);
                                }
                            }
                        }
                    }

                    Refresh();
                }
            }
            else if(_PathCType == PathConstructorType.AddCurve)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (_last_pathtreenode.PathObject.Path.GetGroup().Count == 0)
                    {
                        _last_pathcurve = new Drawing.Curve();

                        Drawing.Curve c = _last_pathcurve;
                        c.Start = e.Location;
                        _last_pathtreenode.PathObject.Path.AddCurve(c);
                    }
                    else if (_last_pathcurve.Start.IsEmpty == false && _last_pathcurve.End.IsEmpty == true)
                    {
                        Drawing.Curve cv = Drawing.Curve.CreateCurve(_last_pathcurve.Start, e.Location);
                        _last_pathcurve.CP1 = cv.CP1;
                        _last_pathcurve.CP2 = cv.CP2;
                        _last_pathcurve.End = cv.End;

                        _last_pathcurve = new Drawing.Curve();

                        Drawing.Curve c = _last_pathcurve;
                        c.Start = e.Location;
                        _last_pathtreenode.PathObject.Path.AddCurve(c);
                        
                    }

                    bool add_PTN = true;
                    foreach (PathTreeNode ptn in _pathlist)
                    {
                        if (ptn.Equals(_last_pathtreenode))
                        {
                            add_PTN = false;
                        }
                    }
                    if (add_PTN == true)
                    {
                        _pathlist.Add(_last_pathtreenode);
                    }
                    Refresh();
                }
            }            
        }
        #endregion

        #region MouseDown
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            ReorderPathList();

            if (e.Button == MouseButtons.Middle)
            {
                foreach (Drawing.Path g in _pathgroups)
                {
                    List<Drawing.Line> lines = Drawing.Path.GetLinesOfPoint(e.Location, g);
                    List<Drawing.Curve> curves = Drawing.Path.GetCurvesOfPoint(e.Location, g);

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
            ReorderPathList();

            if (e.Button == MouseButtons.Middle)
            {
                foreach (Drawing.Path g in _pathgroups)
                {
                    List<Drawing.Line> lines = Drawing.Path.GetLinesOfPoint(e.Location, g);
                    List<Drawing.Curve> curves = Drawing.Path.GetCurvesOfPoint(e.Location, g);

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

        public void ChangeViewImage(Bitmap b)
        {
            _bitmap = b;
        }

        /// <summary>
        /// Compile des images organisées en couches et appele OnPaint.
        /// </summary>
        /// <param name="main">L'image principale sur laquelle on dessine.</param>
        /// <param name="bs">Les images secondaires qui doivent être tranparentes.</param>
        public void ChangeViewImage(Bitmap main, List<Bitmap> bs)
        {
            Graphics g = Graphics.FromImage(main);
            foreach(Bitmap bmp in bs)
            {
                g.DrawImage(bmp, 0, 0);
            }
            g.Dispose();

            _bitmap = main;
        }

        public void ChangeViewSize(Size s)
        {
            _size = s;
        }

        public void AddShapeLink(ShapeTreeNode stn, ParentTreeNode ptn)
        {
            _shapelink.Add(stn, ptn);
        }

        public Dictionary<ShapeTreeNode, ParentTreeNode> ShapeLink
        {
            get { return _shapelink; }
        }

        public void AddPathToList(PathTreeNode ptn)
        {
            _pathlist.Add(ptn);
        }

        public List<PathTreeNode> PathList
        {
            get { return _pathlist; }
        }

        public PathTreeNode LastPathTreeNode
        {
            get { return _last_pathtreenode; }
            set { _last_pathtreenode = value; }
        }

        public void ChangeFrameAndRefresh(int frame)
        {
            double fpm = _fps / 1000d;
            _approximative_time = Convert.ToInt64(Math.Round(Convert.ToDouble(frame) / fpm));
            Refresh();
        }

        public void UpdatePaintOnly()
        {
            Refresh();
        }

        public AssScript Script
        {
            get { return _asss; }
            set { _asss = value; }
        }

        public long Time
        {
            get { return _approximative_time; }
            set { _approximative_time = value; }
        }

        public double FPS
        {
            get { return _fps; }
            set { _fps = value; }
        }

        public PathConstructorType PathConstructorType
        {
            get { return _PathCType; }
            set { _PathCType = value; }
        }
    }

    public enum PathConstructorType
    {
        None, AddCurve, AddStart
    }
}

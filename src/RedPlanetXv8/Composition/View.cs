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
        Dictionary<ShapeTreeNode, ParentTreeNode> _shapelink = new Dictionary<ShapeTreeNode, ParentTreeNode>();

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
        }

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
    }
}

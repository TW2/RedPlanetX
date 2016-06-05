using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.ASS
{
    public class Letter
    {
        private string _string = "";
        private Color _mainColor = Color.Red;
        private Color _karaColor = Color.Yellow;
        private Color _outlineColor = Color.Black;
        private Color _shadowColor = Color.Black;
        private RectangleF _rf = RectangleF.Empty;
        private Font _font = new Font("Arial", 40, FontStyle.Bold);

        //Ces données sont remplit dans Syllable
        private long _duration_ms = 0;
        private long _start_ms = 0;
        private long _end_ms = 0;
        private long _sentence_duration_ms = 0;
        private long _sentence_start_ms = 0;
        private long _sentence_end_ms = 0;

        public Letter()
        {

        }

        public Letter(char c)
        {
            _string = c.ToString();
        }

        public string String
        {
            get { return _string; }
            set { _string = value; }
        }

        public Color MainColor
        {
            get { return _mainColor; }
            set { _mainColor = value; }
        }

        public Color KaraColor
        {
            get { return _karaColor; }
            set { _karaColor = value; }
        }

        public Color OutlineColor
        {
            get { return _outlineColor; }
            set { _outlineColor = value; }
        }

        public Color ShadowColor
        {
            get { return _shadowColor; }
            set { _shadowColor = value; }
        }

        public RectangleF Rect
        {
            get { return _rf; }
            set { _rf = value; }
        }

        public Font Font
        {
            get { return _font; }
            set { _font = value; }
        }

        public long SyllableDuration
        {
            get { return _duration_ms; }
            set { _duration_ms = value; }
        }

        public long SyllableStart
        {
            get { return _start_ms; }
            set { _start_ms = value; }
        }

        public long SyllableEnd
        {
            get { return _end_ms; }
            set { _end_ms = value; }
        }

        public long SentenceDuration
        {
            get { return _sentence_duration_ms; }
            set { _sentence_duration_ms = value; }
        }

        public long SentenceStart
        {
            get { return _sentence_start_ms; }
            set { _sentence_start_ms = value; }
        }

        public long SentenceEnd
        {
            get { return _sentence_end_ms; }
            set { _sentence_end_ms = value; }
        }

        public void DrawPlainTextWithoutKaraoke(Graphics g, long time)
        {
            if (time >= _sentence_start_ms && time <= _sentence_end_ms)
            {
                using (SolidBrush sb = new SolidBrush(_mainColor))
                {
                    RectangleF new_rf = new RectangleF(_rf.X, _rf.Y, _rf.Width * 2, _rf.Height);
                    g.DrawString(_string, _font, sb, new_rf);

                    //switch (AlphaPosition.AlphaPositionType)
                    //{
                    //    case AlphaPosition.Type.an1:                            
                    //        g.DrawString(_syl, f, sb, Point.X, Point.Y - measure.Height); break;
                    //    case AlphaPosition.Type.an2:
                    //        g.DrawString(_syl, f, sb, Point.X - measure.Width / 2, Point.Y - measure.Height); break;
                    //    case AlphaPosition.Type.an3:
                    //        g.DrawString(_syl, f, sb, Point.X - measure.Width, Point.Y - measure.Height); break;
                    //    case AlphaPosition.Type.an4:
                    //        g.DrawString(_syl, f, sb, Point.X, Point.Y - measure.Height / 2); break;
                    //    case AlphaPosition.Type.an5:
                    //        g.DrawString(_syl, f, sb, Point.X - measure.Width / 2, Point.Y - measure.Height / 2); break;
                    //    case AlphaPosition.Type.an6:
                    //        g.DrawString(_syl, f, sb, Point.X - measure.Width, Point.Y - measure.Height / 2); break;
                    //    case AlphaPosition.Type.an7:
                    //        g.DrawString(_syl, f, sb, Point.X, Point.Y); break;
                    //    case AlphaPosition.Type.an8:
                    //        g.DrawString(_syl, f, sb, Point.X - measure.Width / 2, Point.Y); break;
                    //    case AlphaPosition.Type.an9:
                    //        g.DrawString(_syl, f, sb, Point.X - measure.Width, Point.Y); break;
                    //}
                }
            }

        }

        public void DrawPlainTextWithKaraoke(Graphics g, long time)
        {
            if (time >= _start_ms && time <= _end_ms && time >= _sentence_start_ms && time <= _sentence_end_ms)
            {
                using (SolidBrush sb = new SolidBrush(_karaColor))
                {
                    RectangleF new_rf = new RectangleF(_rf.X, _rf.Y, _rf.Width * 2, _rf.Height);
                    g.DrawString(_string, _font, sb, new_rf);

                    //switch (AlphaPosition.AlphaPositionType)
                    //{
                    //    case AlphaPosition.Type.an1:                            
                    //        g.DrawString(_syl, f, sb, Point.X, Point.Y - measure.Height); break;
                    //    case AlphaPosition.Type.an2:
                    //        g.DrawString(_syl, f, sb, Point.X - measure.Width / 2, Point.Y - measure.Height); break;
                    //    case AlphaPosition.Type.an3:
                    //        g.DrawString(_syl, f, sb, Point.X - measure.Width, Point.Y - measure.Height); break;
                    //    case AlphaPosition.Type.an4:
                    //        g.DrawString(_syl, f, sb, Point.X, Point.Y - measure.Height / 2); break;
                    //    case AlphaPosition.Type.an5:
                    //        g.DrawString(_syl, f, sb, Point.X - measure.Width / 2, Point.Y - measure.Height / 2); break;
                    //    case AlphaPosition.Type.an6:
                    //        g.DrawString(_syl, f, sb, Point.X - measure.Width, Point.Y - measure.Height / 2); break;
                    //    case AlphaPosition.Type.an7:
                    //        g.DrawString(_syl, f, sb, Point.X, Point.Y); break;
                    //    case AlphaPosition.Type.an8:
                    //        g.DrawString(_syl, f, sb, Point.X - measure.Width / 2, Point.Y); break;
                    //    case AlphaPosition.Type.an9:
                    //        g.DrawString(_syl, f, sb, Point.X - measure.Width, Point.Y); break;
                    //}
                }
            }

        }
    }
}

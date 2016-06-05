using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RedPlanetXv8.ASS
{
    public class Sentence
    {
        private string _sentence = "";
        private string _sentence_karaoke = "";
        private long _sentence_duration_ms = 0;
        private long _sentence_start_ms = 0;
        private long _sentence_end_ms = 0;
        private List<Syllable> _syls = new List<Syllable>();
        private PointF _insertpoint = PointF.Empty;
        private AlphaPosition _aptype = new AlphaPosition();
        private Font _font = new Font("Arial", 40, FontStyle.Bold);

        public Sentence()
        {

        }

        public Sentence(string assline)
        {
            string[] table = assline.Split(new char[] { ',' }, 10);
            _sentence_start_ms = GetTime(table[1]);
            _sentence_duration_ms = GetTime(table[2]) - GetTime(table[1]);
            _sentence_karaoke = table[9];
            _sentence = StripRawText(table[9]);
            _syls = SplitASS(table[9], _sentence_start_ms, _sentence_duration_ms);
            _sentence_end_ms = _sentence_start_ms + _sentence_duration_ms;
        }

        public string String
        {
            get { return _sentence; }
            set { _sentence = value; }
        }

        public string Karaoke
        {
            get { return _sentence_karaoke; }
            set { _sentence_karaoke = value; }
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

        public List<Syllable> Syllables
        {
            get { return _syls; }
            set { _syls = value; }
        }

        public PointF InsertPoint
        {
            get { return _insertpoint; }
            set { _insertpoint = value; }
        }

        public AlphaPosition AlphaPosition
        {
            get { return _aptype; }
            set { _aptype = value; }
        }

        public Font Font
        {
            get { return _font; }
            set { _font = value; }
        }

        private string StripRawText(string rawText)
        {
            string[] table = rawText.Split(new char[] { '{' });
            string result = "";

            foreach (string s in table)
            {
                if (s.Length > 0)
                {
                    string asss = s.StartsWith("\\k") == true ? s.Replace("\\k", "") : s;
                    string[] table2 = asss.Split(new char[] { '}' });

                    result += table2[1];
                }
            }

            return result;
        }

        private long GetTime(string rawTime)
        {
            string pat = @"(\d+):(\d+):(\d+).(\d+)";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            int hour = 0, min = 0, sec = 0, ms = 0;

            Match m = r.Match(rawTime);
            if (m.Success)
            {
                hour = Convert.ToInt32(m.Groups[1].Value);
                min = Convert.ToInt32(m.Groups[2].Value);
                sec = Convert.ToInt32(m.Groups[3].Value);
                ms = Convert.ToInt32(m.Groups[4].Value) * 10;
            }

            long time = hour * 3600000 + min * 60000 + sec * 1000 + ms;

            return time;
        }

        private List<Syllable> SplitASS(string asstext, long sentence_start, long sentence_duration)
        {
            string[] table = asstext.Split(new char[] { '{' });
            List<Syllable> list = new List<Syllable>();
            long start = 0;

            foreach (string s in table)
            {
                if (s.Length > 0)
                {
                    string asss = s.StartsWith("\\k") == true ? s.Replace("\\k", "") : s;
                    string[] table2 = asss.Split(new char[] { '}' });
                    long dur_centi = Convert.ToInt32(table2[0]);
                    long dur_milli = dur_centi * 10;

                    Syllable syl = new Syllable(
                        table2[1], 
                        dur_milli, start + sentence_start, 
                        sentence_duration, sentence_start);

                    list.Add(syl);

                    start += dur_milli;
                }
            }

            return list;
        }

        public void PrepareSentence(Graphics g)
        {
            if(_insertpoint.IsEmpty == true) { _insertpoint = new PointF(5f, 5f); }
            SizeF sf = g.MeasureString(_sentence, _font);
            RectangleF rf = new RectangleF(_insertpoint, sf);

            List<RectangleF> rects = MeasureCharacters(g, rf, _font, _sentence);
            int index = 0;

            foreach (Syllable syl in _syls)
            {
                foreach (Letter let in syl.Letters)
                {
                    let.Rect = rects[index];
                    index++;
                }
            }

        }

        // Measure the characters in a string with no more than 32 characters. //More than 32 >> Overflow Exception
        private List<RectangleF> MeasureCharactersInWord(Graphics gr, RectangleF rf, Font font, string text)
        {
            List<RectangleF> result = new List<RectangleF>();

            using (StringFormat string_format = new StringFormat())
            {
                string_format.Alignment = StringAlignment.Near;
                string_format.LineAlignment = StringAlignment.Near;
                string_format.Trimming = StringTrimming.None;
                string_format.FormatFlags =
                    StringFormatFlags.MeasureTrailingSpaces;

                CharacterRange[] ranges = new CharacterRange[text.Length];
                for (int i = 0; i < text.Length; i++)
                {
                    ranges[i] = new CharacterRange(i, 1);
                }
                string_format.SetMeasurableCharacterRanges(ranges);

                // Find the character ranges.
                Region[] regions = gr.MeasureCharacterRanges(text, font, rf, string_format);

                // Convert the regions into rectangles.
                foreach (Region region in regions)
                    result.Add(region.GetBounds(gr));
            }

            return result;
        }

        // Measure the characters in the string.
        private List<RectangleF> MeasureCharacters(Graphics gr, RectangleF rf, Font font, string text)
        {
            List<RectangleF> results = new List<RectangleF>();

            // The X location for the next character.
            float x = 0;

            // Get the character sizes 31 characters at a time.
            for (int start = 0; start < text.Length; start += 32)
            {
                // Get the substring.
                int len = 32;
                if (start + len >= text.Length) len = text.Length - start;
                string substring = text.Substring(start, len);

                // Measure the characters.
                List<RectangleF> rects = MeasureCharactersInWord(gr, rf, font, substring);

                // Remove lead-in for the first character.
                if (start == 0) x += rects[0].Left;

                // Save all but the last rectangle.
                for (int i = 0; i < rects.Count + 1 - 1; i++)
                {
                    RectangleF new_rect = new RectangleF(x, rects[i].Top, rects[i].Width, rects[i].Height);
                    results.Add(new_rect);

                    // Move to the next character's X position.
                    x += rects[i].Width;
                }
            }

            // Return the results.
            return results;
        }

        // Draw a long string with boxes around each character.
        private void DrawTextInBoxes(Graphics gr, RectangleF rf, Font font, float start_x, float start_y, string text)
        {
            // Measure the characters.
            List<RectangleF> rects = MeasureCharacters(gr, rf, font, text);

            for (int i = 0; i < text.Length; i++)
            {
                gr.DrawRectangle(Pens.Red,
                    start_x + rects[i].Left, start_y + rects[i].Top,
                    rects[i].Width, rects[i].Height);
            }
            gr.DrawString(text, font, Brushes.Blue, start_x, start_y);
        }

    }
}

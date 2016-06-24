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

            this.Moments.Add(new SentenceProfile(), _sentence_start_ms);
            this.Moments.Add(new SentenceProfile(), _sentence_end_ms);
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
                        sentence_duration, sentence_start, this);

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


        //Cette collection doit être initialisée au moment de la création de la syllabe ou lettre
        //(this collection must be initialized when we create syllable or letter)
        //On doit donc passer par la propriété Moments pour cela
        //(We must initialized it by using Moments property)
        private Dictionary<SentenceProfile, long> _moments = new Dictionary<SentenceProfile, long>();

        public Dictionary<SentenceProfile, long> Moments
        {
            get { return _moments; }
            set { _moments = value; }
        }

        #region Angle GET SET
        public void SetAngleX(long time, int angle)
        {
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.AngleX = angle; }
            }
        }

        public int GetAngleX(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.AngleY = angle; }
            }
        }

        public int GetAngleY(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.AngleZ = angle; }
            }
        }

        public int GetAngleZ(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.FrontColor = c; }
            }
        }

        public Color GetFrontColor(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.BackColor = c; }
            }
        }

        public Color GetBackColor(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.BorderColor = c; }
            }
        }

        public Color GetBorderColor(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.ShadowColor = c; }
            }
        }

        public Color GetShadowColor(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.RelativePositionX = pos; }
            }
        }

        public float GetRelativePositionX(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.RelativePositionY = pos; }
            }
        }

        public float GetRelativePositionY(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.QuakeX = quake; }
            }
        }

        public int GetQuakeX(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.QuakeY = quake; }
            }
        }

        public int GetQuakeY(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.ScaleX = scale; }
            }
        }

        public float GetScaleX(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.ScaleY = scale; }
            }
        }

        public float GetScaleY(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.BorderWeight = b; }
            }
        }

        public int GetBorderWeight(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
            foreach (KeyValuePair<SentenceProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.ShadowDepth = b; }
            }
        }

        public int GetShadowDepth(long approximative_time)
        {
            List<SentenceProfile> list = new List<SentenceProfile>(_moments.Keys);
            long lastMinValue = _sentence_start_ms, lastMaxValue = _sentence_end_ms;
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
    Quand un Sentence sert de classe de stockage pour l'instant t alors un SentenceProfile est un espace
    de stockage à l'instant défini sur une limite ou un checkpoint pour l'instant A(start) B(checkpoint)
    ou C(end). Le Sentence se servant des SentenceProfile(s) pour définir l'instant t.
    C'est pour ça qu'il ont à peu près les mêmes paramètres.
    */
    public class SentenceProfile
    {
        private long _moment_milliseconds = 0L;
        private int _angle_x = 0;
        private int _angle_y = 0;
        private int _angle_z = 0;
        private Color _front_color = Color.AliceBlue; //(unused)
        private Color _back_color = Color.AliceBlue; //(unused)
        private Color _border_color = Color.White;
        private Color _shadow_color = Color.Gray;
        private float _relative_position_x = 0f;
        private float _relative_position_y = 0f;
        private int _quake_x = 0;
        private int _quake_y = 0;
        private float _scale_x = 100f; //de 0.0 à 100.0
        private float _scale_y = 100f; //de 0.0 à 100.0
        private int _border_weight = 1;
        private int _shadow_depth = 0;

        public SentenceProfile()
        {

        }

        public SentenceProfile(long moment_milliseconds)
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

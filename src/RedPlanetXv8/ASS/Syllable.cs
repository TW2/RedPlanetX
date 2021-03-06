﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.ASS
{
    public class Syllable
    {
        private string _syl = "";
        private long _duration_ms = 0;
        private long _start_ms = 0;
        private long _end_ms = 0;
        private long _sentence_duration_ms = 0;
        private long _sentence_start_ms = 0;
        private long _sentence_end_ms = 0;
        private Point _position_pixels = Point.Empty;
        private List<Letter> _letter_list = new List<Letter>();
        private Sentence _from_sentence = null;

        public Syllable()
        {

        }

        //On crée une Syllable dans Sentence dans la méthode SpiltASS
        //(We create a Syllable in Sentence in the SplitASS method)
        public Syllable(string syl, long kdur, long kstart, long sdur, long sstart, Sentence sen)
        {
            //Syllabe
            _syl = syl;
            _duration_ms = kdur;
            _start_ms = kstart;
            _end_ms = kstart + kdur;
            _sentence_duration_ms = sdur;
            _sentence_start_ms = sstart;
            _sentence_end_ms = sstart + sdur;

            this.Moments.Add(new SyllableProfile(), this.SyllableStart);
            this.Moments.Add(new SyllableProfile(), this.SyllableEnd);

            //Lettres
            char[] cs = _syl.ToCharArray();

            long letters_dur = 0;

            foreach (char c in cs)
            {
                Letter let = new Letter(c);
                let.SyllableDuration = _duration_ms;
                let.SyllableStart = _start_ms;
                let.SyllableEnd = _end_ms;
                let.SentenceDuration = _sentence_duration_ms;
                let.SentenceStart = _sentence_start_ms;
                let.SentenceEnd = _sentence_end_ms;

                let.FromSentence = sen;
                let.FromSyllable = this;

                let.LetterStart = _start_ms + letters_dur;
                let.LetterDuration = _duration_ms / cs.Length;
                letters_dur += let.LetterDuration;
                let.LetterEnd = letters_dur;

                let.Moments.Add(new LetterProfile(), let.LetterStart);
                let.Moments.Add(new LetterProfile(), let.LetterEnd);

                _letter_list.Add(let);
            }
        }

        public string Syl
        {
            get { return _syl; }
            set { _syl = value; }
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

        public Point Point
        {
            get { return _position_pixels; }
            set { _position_pixels = value; }
        }

        public List<Letter> Letters
        {
            get { return _letter_list; }
            set { _letter_list = value; }
        }

        public Sentence FromSentence
        {
            get { return _from_sentence; }
            set { _from_sentence = value; }
        }

        //Cette collection doit être initialisée au moment de la création de la syllabe ou lettre
        //(this collection must be initialized when we create syllable or letter)
        //On doit donc passer par la propriété Moments pour cela
        //(We must initialized it by using Moments property)
        private Dictionary<SyllableProfile, long> _moments = new Dictionary<SyllableProfile, long>();

        public Dictionary<SyllableProfile, long> Moments
        {
            get { return _moments; }
            set { _moments = value; }
        }

        #region Angle GET SET
        public void SetAngleX(long time, int angle)
        {
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.AngleX = angle; }
            }
        }

        public int GetAngleX(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.AngleY = angle; }
            }
        }

        public int GetAngleY(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.AngleZ = angle; }
            }
        }

        public int GetAngleZ(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.FrontColor = c; }
            }
        }

        public Color GetFrontColor(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.BackColor = c; }
            }
        }

        public Color GetBackColor(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.BorderColor = c; }
            }
        }

        public Color GetBorderColor(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.ShadowColor = c; }
            }
        }

        public Color GetShadowColor(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.RelativePositionX = pos; }
            }
        }

        public float GetRelativePositionX(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.RelativePositionY = pos; }
            }
        }

        public float GetRelativePositionY(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.QuakeX = quake; }
            }
        }

        public int GetQuakeX(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.QuakeY = quake; }
            }
        }

        public int GetQuakeY(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.ScaleX = scale; }
            }
        }

        public float GetScaleX(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.ScaleY = scale; }
            }
        }

        public float GetScaleY(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.BorderWeight = b; }
            }
        }

        public int GetBorderWeight(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
            foreach (KeyValuePair<SyllableProfile, long> pair in _moments)
            {
                if (pair.Value == time) { pair.Key.ShadowDepth = b; }
            }
        }

        public int GetShadowDepth(long approximative_time)
        {
            List<SyllableProfile> list = new List<SyllableProfile>(_moments.Keys);
            long lastMinValue = _start_ms, lastMaxValue = _end_ms;
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
    Quand un Syllable sert de classe de stockage pour l'instant t alors un SyllableProfile est un espace
    de stockage à l'instant défini sur une limite ou un checkpoint pour l'instant A(start) B(checkpoint)
    ou C(end). Le Syllable se servant des SyllableProfile(s) pour définir l'instant t.
    C'est pour ça qu'il ont à peu près les mêmes paramètres.
    */
    public class SyllableProfile
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

        public SyllableProfile()
        {

        }

        public SyllableProfile(long moment_milliseconds)
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

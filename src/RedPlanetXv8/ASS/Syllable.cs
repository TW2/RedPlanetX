using System;
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

        public Syllable(string syl, long kdur, long kstart, long sdur, long sstart)
        {
            //Syllabe
            _syl = syl;
            _duration_ms = kdur;
            _start_ms = kstart;
            _end_ms = kstart + kdur;
            _sentence_duration_ms = sdur;
            _sentence_start_ms = sstart;
            _sentence_end_ms = sstart + sdur;

            //Lettres
            char[] cs = _syl.ToCharArray();
            foreach (char c in cs)
            {
                Letter let = new Letter(c);
                let.SyllableDuration = _duration_ms;
                let.SyllableStart = _start_ms;
                let.SyllableEnd = _end_ms;
                let.SentenceDuration = _sentence_duration_ms;
                let.SentenceStart = _sentence_start_ms;
                let.SentenceEnd = _sentence_end_ms;

                let.FromSentence = this.FromSentence;
                let.FromSyllable = this;
                let.Moments.Add(new LetterProfile(), _start_ms);
                let.Moments.Add(new LetterProfile(), _end_ms);

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

    }
}

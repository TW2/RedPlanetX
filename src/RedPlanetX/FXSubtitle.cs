using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedPlanetX
{
    public class FXSubtitle
    {
        private Dictionary<TreeNode, AssSentence> eachsentences = new Dictionary<TreeNode, AssSentence>();
        private Dictionary<TreeNode, AssLine> eachlines = new Dictionary<TreeNode, AssLine>();
        private Dictionary<TreeNode, AssAllSyllables> allSyllables = new Dictionary<TreeNode, AssAllSyllables>();
        private Dictionary<TreeNode, AssSyllable> eachSyllables = new Dictionary<TreeNode, AssSyllable>();
        private Dictionary<TreeNode, GenericParticle> particles = new Dictionary<TreeNode, GenericParticle>();

        public TreeNode GetMappedTreeNodeForASS(List<String> rawAssLines, Font f, int centerX, int centerY)
        {
            eachsentences.Clear();
            eachlines.Clear();
            allSyllables.Clear();
            eachSyllables.Clear();

            TreeNode top = new TreeNode("ASS file");

            //Structure of an ASS Line :
            //Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
            //Interesting for us : Start, End, Text

            foreach (string s in rawAssLines){
                String[] table = s.Split(new char[]{','}, 10);

                long Start = GetTime(table[1]);
                long Duration = GetTime(table[2]) - GetTime(table[1]);
                string Text = StripRawText(table[9]);

                AssSentence sentence = new AssSentence();
                sentence.InitSentence(Text, Duration, Start, f, centerX, centerY);

                TreeNode t1 = new TreeNode(Text);
                eachsentences.Add(t1, sentence);
                top.Nodes.Add(t1);

                TreeNode t2 = new TreeNode("Applied effects to the line");
                eachlines.Add(t2, sentence.Sentence);
                t1.Nodes.Add(t2);

                if (table[9].Contains("{\\"))
                {
                    List<string> syllables = GetSyllables(table[9]);
                    List<long> startTimes = GetStartTimes(table[9]);
                    List<long> durations = GetDurations(table[9]);
                    sentence.InitSyllables(Text, syllables, durations, startTimes, f, centerX, centerY);

                    TreeNode t3 = new TreeNode("Applied effects to the syllables");
                    allSyllables.Add(t3, sentence.PackedSyllables);
                    t1.Nodes.Add(t3);

                    foreach (AssSyllable _as in sentence.UnpackedSyllables)
                    {
                        TreeNode t4 = new TreeNode(_as.Syllable.String);
                        eachSyllables.Add(t4, _as);
                        t1.Nodes.Add(t4);
                    }
                }

                
            }

            return top;
        }

        public List<AssLine> GetLinesAt(long milliseconds)
        {
            List<AssLine> templist = new List<AssLine>(eachlines.Values);
            List<AssLine> getlist = new List<AssLine>();

            foreach (AssLine al in templist)
            {
                long starttime = al.Line.StartTime;
                long endtime =  al.Line.StartTime + al.Line.Duration;
                if (starttime <= milliseconds && endtime > milliseconds)
                {
                    getlist.Add(al);
                }
            }

            return getlist;
        }

        public List<AssAllSyllables> GetAllSyllablesAt(long milliseconds)
        {
            List<AssAllSyllables> templist = new List<AssAllSyllables>(allSyllables.Values);
            List<AssAllSyllables> getlist = new List<AssAllSyllables>();

            foreach (AssAllSyllables aas in templist)
            {
                long starttime = aas.Line.StartTime;
                long endtime = aas.Line.StartTime + aas.Line.Duration;
                if (starttime <= milliseconds && endtime > milliseconds)
                {
                    getlist.Add(aas);
                }
            }

            return getlist;
        }

        public List<AssSyllable> GetSyllablesAt(long milliseconds)
        {
            List<AssSyllable> templist = new List<AssSyllable>(eachSyllables.Values);
            List<AssSyllable> getlist = new List<AssSyllable>();

            foreach (AssSyllable _as in templist)
            {
                long starttime = _as.Line.StartTime + _as.Syllable.StartTime;
                long endtime = _as.Line.StartTime + _as.Syllable.StartTime + _as.Syllable.Duration;
                if (starttime <= milliseconds && endtime > milliseconds)
                {
                    getlist.Add(_as);
                }
            }

            return getlist;
        }

        private String StripRawText(string rawText)
        {
            String str = "";
            if (rawText.Contains("{\\"))
            {
                string pat = @"\{\\k(\d+)\}(\w*\s*)";
                Regex r = new Regex(pat, RegexOptions.IgnoreCase);

                Match m = r.Match(rawText);
                while (m.Success)
                {
                    str = str + m.Groups[2].Value;
                    m = m.NextMatch();
                }
            }
            else
            {
                str = rawText;
            }
            return str;
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

        private List<string> GetSyllables(string rawText)
        {
            string pat = @"\{\\k(\d+)\}(\w*\s*)";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            List<string> syllables = new List<string>();

            Match m = r.Match(rawText);
            while (m.Success)
            {
                syllables.Add(m.Groups[2].Value);
                m = m.NextMatch();
            }

            return syllables;
        }

        private List<long> GetStartTimes(string rawText)
        {
            string pat = @"\{\\k(\d+)\}(\w*\s*)";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            List<long> startTimes = new List<long>();
            long start = 0;

            Match m = r.Match(rawText);
            while (m.Success)
            {
                long value = Convert.ToInt64(m.Groups[1].Value) * 10;
                startTimes.Add(start);
                start += value;
                m = m.NextMatch();
            }

            return startTimes;
        }

        private List<long> GetDurations(string rawText)
        {
            string pat = @"\{\\k(\d+)\}(\w*\s*)";
            Regex r = new Regex(pat, RegexOptions.IgnoreCase);

            List<long> durations = new List<long>();

            Match m = r.Match(rawText);
            while (m.Success)
            {
                long value = Convert.ToInt64(m.Groups[1].Value) * 10;
                durations.Add(value);
                m = m.NextMatch();
            }

            return durations;
        }

        public Dictionary<TreeNode, AssLine> GetAssLines()
        {
            return eachlines;
        }

        public Dictionary<TreeNode, AssAllSyllables> GetAssAllSyllables()
        {
            return allSyllables;
        }

        public Dictionary<TreeNode, AssSyllable> GetAssSyllables()
        {
            return eachSyllables;
        }

        public Dictionary<TreeNode, GenericParticle> GetParticles()
        {
            return particles;
        }
    }

    public enum SyllableDirection
    {
        Horizontal4To6, Horizontal6To4, Vertical8To2, Vertical2To8, Diagonal7To3, Diagonal1To9, Diagonal3To7, Diagonal9To1
    }

    public class TString
    {
        public String String { get; set; }
        public int X { get; set; } // This is the center of the syllable or line
        public int Y { get; set; } // This is the middle of height
        public Font Font { get; set; }
        public SyllableDirection Direction { get; set; }
        public long Duration { get; set; }
        public long StartTime { get; set; }
        public Size Size { get; set; }

        public TString()
        {
            String = "";
            X = 0;
            Y = 0;
            Font = new Font("Arial", 12f);
            Direction = SyllableDirection.Horizontal4To6;
            Duration = 0;
            StartTime = 0;
            Size = new Size(0, 0);
        }
    }

    public class AssSyllable
    {
        public TString Line { get; set; }
        public TString Syllable { get; set; }
        private List<GenericParticle> Particles = new List<GenericParticle>();

        public AssSyllable()
        {
            Line = new TString();
            Syllable = new TString();
        }

        public void AddParticle(GenericParticle p)
        {
            Particles.Add(p);
        }

        public void RemoveParticle(GenericParticle p)
        {
            if (Particles.Contains(p))
            {
                Particles.Remove(p);
            }
        }

        public void ClearParticles()
        {
            Particles.Clear();
        }

        public List<GenericParticle> GetParticles()
        {
            return Particles;
        }
    }

    public class AssAllSyllables
    {
        public TString Line { get; set; }
        private List<TString> Syllables = new List<TString>();
        private List<GenericParticle> Particles = new List<GenericParticle>();

        public AssAllSyllables()
        {
            Line = new TString();
        }

        public void AddSyllable(TString s)
        {
            Syllables.Add(s);
        }

        public void RemoveSyllable(TString s)
        {
            if (Syllables.Contains(s))
            {
                Syllables.Remove(s);
            }
        }

        public void ClearSyllables()
        {
            Syllables.Clear();
        }

        public List<TString> GetSyllables()
        {
            return Syllables;
        }

        public void AddParticle(GenericParticle p)
        {
            Particles.Add(p);
        }

        public void RemoveParticle(GenericParticle p)
        {
            if (Particles.Contains(p))
            {
                Particles.Remove(p);
            }
        }

        public void ClearParticles()
        {
            Particles.Clear();
        }

        public List<GenericParticle> GetParticles()
        {
            return Particles;
        }
    }

    public class AssLine
    {
        public TString Line { get; set; }
        private List<GenericParticle> Particles = new List<GenericParticle>();

        public AssLine()
        {
            Line = new TString();
        }

        public void AddParticle(GenericParticle p)
        {
            Particles.Add(p);
        }

        public void RemoveParticle(GenericParticle p)
        {
            if (Particles.Contains(p))
            {
                Particles.Remove(p);
            }
        }

        public void ClearParticles()
        {
            Particles.Clear();
        }

        public List<GenericParticle> GetParticles()
        {
            return Particles;
        }
    }

    public class AssSentence
    {
        public AssLine Sentence { get; set; }
        public AssAllSyllables PackedSyllables { get; set; }
        public List<AssSyllable> UnpackedSyllables { get; set; }
        public String SentenceString { get; set; }
        public Font MainFont { get; set; }
        public SyllableDirection MainDirection { get; set; }

        public AssSentence()
        {
            Sentence = new AssLine();
            PackedSyllables = new AssAllSyllables();
            UnpackedSyllables = new List<AssSyllable>();
            SentenceString = "";
            MainFont = new Font("Arial", 12f);
            MainDirection = SyllableDirection.Horizontal4To6;
        }

        public void InitSentence(string sentence, long duration, long startTime, Font f, int posX, int posY)
        {
            SentenceString = sentence;
            Sentence.Line.String = sentence;
            Sentence.Line.X = posX;
            Sentence.Line.Y = posY;
            Sentence.Line.Font = f;
            Sentence.Line.Duration = duration;
            Sentence.Line.StartTime = startTime;
            Sentence.Line.Size = TextRenderer.MeasureText(sentence, f);
        }

        public void InitSyllables(string sentence, List<String> syls, List<long> durations, List<long> startTimes, Font f, int linePosX, int linePosY)
        {
            Size sentenceSize = TextRenderer.MeasureText(sentence, f);
            List<int> positionsOnX = new List<int>();
            int startPos = linePosX - sentenceSize.Width / 2;

            for (int i = 0; i < syls.Count; i++)
            {
                Size syllableSize = TextRenderer.MeasureText(syls[i], f);

                int w = GetRealWidth(sentenceSize, syllableSize, syls, i, f);

                int start = 0;

                if (i == 0)
                {
                    start = startPos + w / 2;
                }
                else if (syls[i] == "i")
                {
                    start = startPos - w / 2; ;
                }
                else if (syls[i].Length == 1)
                {
                    start = startPos;
                }
                else
                {
                    start = startPos + w / 2 - w / 4;
                }

                positionsOnX.Add(start);
                startPos += w;
            }

            for (int i = 0; i < syls.Count; i++)
            {
                TString ts = new TString();
                ts.String = syls[i];
                ts.Duration = durations[i];
                ts.StartTime = startTimes[i];
                ts.Font = f;
                ts.X = positionsOnX[i];
                ts.Y = linePosY;
                ts.Size = TextRenderer.MeasureText(syls[i], f);

                PackedSyllables.AddSyllable(ts);

                AssSyllable _as = new AssSyllable();
                _as.Syllable = ts;
                _as.Line = Sentence.Line;
                UnpackedSyllables.Add(_as);
            }

            PackedSyllables.Line = Sentence.Line;
        }

        private int GetRealWidth(Size sentence, Size syl, List<string> syls, int index, Font f)
        {
            int value = 0;
            if (index != 0)
            {
                string _string = "";
                for (int i = 0; i < syls.Count; i++)
                {
                    if (i != index)
                    {
                        _string += syls[i];
                    }
                }
                Size _size = TextRenderer.MeasureText(_string, f);
                value = sentence.Width - _size.Width;
            }
            else
            {
                value = syl.Width;
            }

            return value;
        }
    }

}

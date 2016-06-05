using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.ASS
{
    public class AssScript
    {
        private string _script_filename = "";
        private List<Sentence> _sents = new List<Sentence>();

        public AssScript()
        {

        }

        public AssScript(string script_file)
        {
            _script_filename = script_file;
            List<string> asslines = GetAllKaraokeLines(_script_filename);
            foreach(string s in asslines)
            {
                Sentence sent = new Sentence(s);
                _sents.Add(sent);
            }
        }

        public List<Sentence> Sentences
        {
            get { return _sents; }
            set { _sents = value; }
        }

        private List<string> GetAllKaraokeLines(string filename)
        {
            List<string> klines = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(filename))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains(@"{\k"))
                        {
                            klines.Add(line);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read : ");
                Console.WriteLine(e.Message);
            }
            return klines;
        }

        public void Update()
        {

        }
    }
}

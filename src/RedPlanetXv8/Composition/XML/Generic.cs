using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RedPlanetXv8.Composition.XML
{
    public class Generic
    {
        //=====================================================================
        // ECRITURE - ECRITURE - ECRITURE - ECRITURE - ECRITURE - ECRITURE - 
        //=====================================================================

        private const string _INDENTATION = "    ";
        private int _LEVEL_INDENTATION = 0;

        public string GetTag_ON(string tag, Dictionary<string, string> param)
        {
            if (param.Count != 0)
            {
                string line = "<" + tag;
                foreach (KeyValuePair<string, string> pair in param)
                {
                    line += " " + pair.Key + "=\"" + pair.Value + "\"";
                }
                line += ">";

                return GetIndentation(1) + line;
            }
            else
            {
                return GetTag_ON(tag);
            }
        }

        public string GetTag_ON(string tag)
        {
            return GetIndentation(1) + "<" + tag + ">";
        }

        public string GetTag_OFF(string tag)
        {
            _LEVEL_INDENTATION -= 1;
            return GetIndentation(0) + "</" + tag + ">";
        }

        public string GetTag_VOID(string tag, Dictionary<string, string> param)
        {
            if (param.Count != 0)
            {
                string line = "<" + tag;
                foreach (KeyValuePair<string, string> pair in param)
                {
                    line += " " + pair.Key + "=\"" + pair.Value + "\"";
                }
                line += " />";
                return GetIndentation(0) + line;
            }
            else
            {
                return GetTag_VOID(tag);
            }
        }

        public string GetTag_VOID(string tag)
        {
            return GetIndentation(0) + "<" + tag + " />";
        }

        private string GetIndentation(int after)
        {
            string ind = "";
            for (int i = 0; i < _LEVEL_INDENTATION; i++)
            {
                ind += _INDENTATION;
            }

            _LEVEL_INDENTATION += after;

            return ind;
        }

        //=====================================================================
        // LECTURE - LECTURE - LECTURE - LECTURE - LECTURE - LECTURE - LECTURE  
        //=====================================================================

        public string FromON(string line, out Dictionary<string, string> param)
        {
            string output = "";
            param = new Dictionary<string, string>();

            Regex regex = new Regex(@"<(\w+)");
            Match match = regex.Match(line);
            if (match.Success)
            {
                output = match.Groups[1].Value;
            }

            regex = new Regex(@"(\w+)=.{1}(\w+)");
            MatchCollection matches = regex.Matches(line);
            foreach (Match m in matches)
            {
                param.Add(m.Groups[1].Value, m.Groups[2].Value);
            }

            return output;
        }

        public string FromOFF(string line)
        {
            string output = "";

            Regex regex = new Regex(@"</(\w+)");
            Match match = regex.Match(line);
            if (match.Success)
            {
                output = match.Groups[1].Value;
            }

            return output;
        }

        public string FromVOID(string line, out Dictionary<string, string> param)
        {
            string output = "";
            param = new Dictionary<string, string>();

            Regex regex = new Regex(@"<(\w+)");
            Match match = regex.Match(line);
            if (match.Success)
            {
                output = match.Groups[1].Value;
            }

            regex = new Regex(@"(\w+)=.{1}(\w+)");
            MatchCollection matches = regex.Matches(line);
            foreach (Match m in matches)
            {
                param.Add(m.Groups[1].Value, m.Groups[2].Value);
            }

            return output;
        }
    }
}

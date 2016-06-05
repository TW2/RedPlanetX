using RedPlanetXv8.ASS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Node
{
    public class SyllableTreeNode : MainTreeNode
    {
        private Syllable _syl = null;

        public SyllableTreeNode()
        {

        }

        public SyllableTreeNode(Syllable syl)
        {
            _syl = syl;
        }

        public Syllable String
        {
            get { return _syl; }
            set { _syl = value; }
        }
    }
}

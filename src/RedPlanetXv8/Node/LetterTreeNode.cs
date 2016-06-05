using RedPlanetXv8.ASS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Node
{
    public class LetterTreeNode : MainTreeNode
    {
        private Letter _let = null;

        public LetterTreeNode()
        {

        }

        public LetterTreeNode(Letter let)
        {
            _let = let;
        }

        public Letter String
        {
            get { return _let; }
            set { _let = value; }
        }
    }
}

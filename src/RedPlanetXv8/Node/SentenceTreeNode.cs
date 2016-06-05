using RedPlanetXv8.ASS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Node
{
    public class SentenceTreeNode : MainTreeNode
    {
        private Sentence _sent = null;

        public SentenceTreeNode()
        {

        }

        public SentenceTreeNode(Sentence sent)
        {
            _sent = sent;
        }

        public Sentence String
        {
            get { return _sent; }
            set { _sent = value; }
        }
    }
}

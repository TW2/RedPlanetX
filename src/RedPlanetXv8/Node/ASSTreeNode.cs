using RedPlanetXv8.ASS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Node
{
    public class ASSTreeNode : MainTreeNode
    {
        private AssScript _asss = null;

        public ASSTreeNode()
        {

        }

        public AssScript Script
        {
            get { return _asss; }
            set { _asss = value; }
        }
    }
}

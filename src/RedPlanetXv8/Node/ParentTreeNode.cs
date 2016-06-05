using RedPlanetXv8.Composition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Node
{
    public class ParentTreeNode : MainTreeNode
    {
        private ParentObject _po = null;

        public ParentTreeNode()
        {

        }

        public ParentTreeNode(ParentObject po)
        {
            _po = po;
        }

        public ParentObject ParentObject
        {
            get { return _po; }
            set { _po = value; }
        }
    }
}

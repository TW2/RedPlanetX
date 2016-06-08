using RedPlanetXv8.Composition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Node
{
    public class PathTreeNode : MainTreeNode
    {
        private PathObject _po = null;

        public PathTreeNode()
        {

        }

        public PathTreeNode(PathObject po)
        {
            _po = po;
        }

        public PathObject PathObject
        {
            get { return _po; }
            set { _po = value; }
        }
    }
}

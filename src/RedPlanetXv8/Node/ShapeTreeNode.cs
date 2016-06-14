using RedPlanetXv8.Composition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Node
{
    public class ShapeTreeNode : MainTreeNode
    {
        private ShapeObject _so = null;        

        public ShapeTreeNode()
        {

        }

        public ShapeTreeNode(ShapeObject so)
        {
            _so = so;
        }

        public ShapeObject ShapeObject
        {
            get { return _so; }
            set { _so = value; }
        }
    }
}

﻿using RedPlanetXv8.Composition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RedPlanetXv8.Node
{
    public class PathTreeNode : MainTreeNode
    {
        private PathObject _po = null;
        private int _index = 0; // Index for select the path in property grid

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

        public int PathIndex
        {
            get { return _index; }
            set { _index = value; }
        }

        public override string ToString()
        {
            return Text;
        }

        public static PathTreeNode GetFromString(AVSTreeNode reference, string name)
        {
            foreach (TreeNode tn in reference.Nodes)
            {
                if(tn.GetType() == typeof(PathTreeNode))
                {
                    PathTreeNode ptn = (PathTreeNode)tn;
                    if (ptn.Text.Equals(name))
                    {
                        return ptn;
                    }
                }
            }
            return null;
        }
    }
}

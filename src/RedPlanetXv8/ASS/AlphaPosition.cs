using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.ASS
{
    public class AlphaPosition
    {
        public enum Type
        {
            an1, an2, an3, an4, an5, an6, an7, an8, an9
        }

        private Type _type = Type.an5;

        public AlphaPosition()
        {

        }

        public Type AlphaPositionType
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}

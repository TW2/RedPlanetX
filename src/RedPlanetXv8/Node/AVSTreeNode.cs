using RedPlanetXv8.AviSynth;
using RedPlanetXv8.Composition;

namespace RedPlanetXv8.Node
{
    public class AVSTreeNode : MainTreeNode
    {
        private Settings _set = new Settings();
        private AviSynthObject _aso = new AviSynthObject();

        public AVSTreeNode()
        {

        }

        public Settings Composition
        {
            get { return _set; }
            set { _set = value; }
        }

        public AviSynthObject Avs
        {
            get { return _aso; }
            set { _aso = value; }
        }
    }
}

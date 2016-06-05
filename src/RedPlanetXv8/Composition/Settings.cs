using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Composition
{
    public class Settings
    {
        private string _projectname = "Karaoke";
        private string _authorname = "Baka";
        private FrameRate _fps = FrameRate._NTSC24;
        private FrameSize _size = FrameSize._HD720p;
        private long _duration = 90000;

        public Settings()
        {

        }

        public void SetDuration(int minutes, int seconds)
        {
            _duration = Convert.ToInt64(minutes) * 60000L + Convert.ToInt64(seconds) * 1000L;
        }

        public string ProjectName
        {
            get { return _projectname; }
            set { _projectname = value; }
        }

        public string AuthorName
        {
            get { return _authorname; }
            set { _authorname = value; }
        }

        public FrameRate FPS
        {
            get { return _fps; }
            set { _fps = value; }
        }

        public FrameSize Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public long Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
    }
}

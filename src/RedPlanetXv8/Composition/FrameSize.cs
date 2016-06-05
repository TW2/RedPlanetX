using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Composition
{
    public class FrameSize
    {
        public static readonly FrameSize _SD_SQUARE = new FrameSize("SD 640x480", 640, 480);
        public static readonly FrameSize _SD_RECT = new FrameSize("SD 640x360", 640, 360);
        public static readonly FrameSize _HD720p = new FrameSize("HD 1280x720", 1280, 720);
        public static readonly FrameSize _HD1080p = new FrameSize("FHD 1920x1080", 1920, 1080);
        public static readonly FrameSize _4K = new FrameSize("4K 3840x2160", 3840, 2160);
        public static readonly FrameSize _8K = new FrameSize("8K 7680x4320", 7680, 4320);

        public static IEnumerable<FrameSize> Values
        {
            get
            {
                yield return _SD_SQUARE;
                yield return _SD_RECT;
                yield return _HD720p;
                yield return _HD1080p;
                yield return _4K;
                yield return _8K;
            }
        }

        private readonly string name;
        private readonly int width;
        private readonly int height;

        FrameSize(string name, int width, int height)
        {
            this.name = name;
            this.width = width;
            this.height = height;
        }

        public string Name { get { return name; } }

        public int Width { get { return width; } }

        public int Height { get { return height; } }

        public override string ToString()
        {
            return name;
        }
    }
}

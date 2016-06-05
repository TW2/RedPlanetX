using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.Composition
{
    public class FrameRate
    {
        public static readonly FrameRate _FILM = new FrameRate("Film (24 Fps) =24", 24d);
        public static readonly FrameRate _NTSC30 = new FrameRate("NTSC (30 Fps) ~29.970", 30d/1001d);
        public static readonly FrameRate _NTSC24 = new FrameRate("NTSC (24 Fps) ~23.976", 24d/1001d);
        public static readonly FrameRate _PAL25 = new FrameRate("PAL (25 Fps) =25", 25d);

        public static IEnumerable<FrameRate> Values
        {
            get
            {
                yield return _FILM;
                yield return _NTSC30;
                yield return _NTSC24;
                yield return _PAL25;
            }
        }

        private readonly string name;
        private readonly double fps;

        FrameRate(string name, double fps)
        {
            this.name = name;
            this.fps = fps;
        }

        public string Name { get { return name; } }

        public double FPS { get { return fps; } }

        public override string ToString()
        {
            return name;
        }
    }
}

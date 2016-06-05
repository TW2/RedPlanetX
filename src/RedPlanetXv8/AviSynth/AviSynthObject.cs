using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetXv8.AviSynth
{
    public class AviSynthObject
    {
        private Bitmap _bitmap = null;
        private AviSynthClip _clip = null;
        private string _script = "";

        public AviSynthObject()
        {

        }

        public AviSynthObject(string filename)
        {
            Script = filename;
            AviSynthScriptEnvironment asse = new AviSynthScriptEnvironment();
            Clip = asse.OpenScriptFile(Script);
            //Image = ReadFrameBitmap(Clip, 0);
        }

        public Bitmap Image
        {
            get { return _bitmap; }
            set { _bitmap = value; }
        }

        public AviSynthClip Clip
        {
            get { return _clip; }
            set { _clip = value; }
        }

        public string Script
        {
            get { return _script; }
            set { _script = value; }
        }

        public void Update(int position)
        {
            Image = ReadFrameBitmap(Clip, position);
        }

        private Bitmap ReadFrameBitmap(AviSynthClip asc, int position)
        {
            Bitmap bmp = null;
            try
            {
                bmp = new Bitmap(asc.VideoWidth, asc.VideoHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                // Lock the bitmap's bits.
                Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(rect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
                try
                {
                    // Get the address of the first line.
                    IntPtr ptr = bmpData.Scan0;
                    // Read data
                    asc.ReadFrame(ptr, bmpData.Stride, position);
                }
                finally
                {
                    // Unlock the bits.
                    bmp.UnlockBits(bmpData);
                }
                bmp.RotateFlip(RotateFlipType.Rotate180FlipX);
                return bmp;
            }
            catch (Exception)
            {
                if (bmp != null) bmp.Dispose();
                throw;
            }
        }
    }
}

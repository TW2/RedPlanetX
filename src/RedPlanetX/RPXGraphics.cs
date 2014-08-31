using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedPlanetX
{
    class RPXGraphics
    {
        public RPXGraphics()
        {

        }

        public static void DrawStringWithFourCornersGradient(Graphics g, string str, Font font, float x, float y, Color c7, Color c9, Color c3, Color c1)
        {
            float width = g.MeasureString(str, font).Width;
            float height = g.MeasureString(str, font).Height;

            //Corner c7
            float c7x = x;
            float c7y = y;
            PointF p7 = new PointF(c7x, c7y);
            //c7 = Color.FromArgb(127, c7);

            g.FillRectangle(Brushes.Pink, c7x, c7y, 1, 1);

            //Corner c9
            float c9x = x + width;
            float c9y = y;
            PointF p9 = new PointF(c9x, c9y);
            c9 = Color.FromArgb(127, c9);

            g.FillRectangle(Brushes.Pink, c9x, c9y, 1, 1);

            //Corner c3
            float c3x = x + width;
            float c3y = y + height;
            PointF p3 = new PointF(c3x, c3y);
            //c3 = Color.FromArgb(127, c3);

            g.FillRectangle(Brushes.Pink, c3x, c3y, 1, 1);

            //Corner c1
            float c1x = x;
            float c1y = y + height;
            PointF p1 = new PointF(c1x, c1y);
            c1 = Color.FromArgb(127, c1);

            g.FillRectangle(Brushes.Pink, c1x, c1y, 1, 1);

            g.DrawString(str, font, Brushes.White, x, y);

            LinearGradientBrush br1 = new LinearGradientBrush(p7, p3, c7, c3);
            g.DrawString(str, font, br1, x, y);

            LinearGradientBrush br2 = new LinearGradientBrush(p9, p1, c9, c1);
            g.DrawString(str, font, br2, x, y);

        }

    }
}

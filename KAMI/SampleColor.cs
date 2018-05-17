using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace KAMI
{
    static class SampleColor
    {
        public static List<Color> GetSelectable(Bitmap sample, int types) // target 
        {
            List<Color> selectable = new List<Color>();
            int pixelength = sample.Width / types;
            int hpl = pixelength / 2;
            for (int i = 0; i < types; i++)
            {
                selectable.Add(Averagecolor(sample, i * pixelength + hpl, sample.Height / 2));
            }
            return selectable;
        }
        private static Color Averagecolor(Bitmap image, int x, int y)//get average color for a grid
        {
            int red = 0;
            int green = 0;
            int blue = 0;
            int radius = 5;
            int space = (2 * radius + 1) * (2 * radius + 1);
            for (int a = x - radius; a <= x + radius; a++)
            {
                for (int b = y - radius; b <= y + radius; b++)
                {
                    red += image.GetPixel(a, b).R;
                    green += image.GetPixel(a, b).G;
                    blue += image.GetPixel(a, b).B;
                }
            }
            red = (red / space);
            green = (green / space);
            blue = (blue / space);
            return System.Drawing.Color.FromArgb(red, green, blue);
        }
    }
}

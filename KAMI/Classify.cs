using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace KAMI
{
    static class Classify
    {
        public static Color Getclass(Color c, List<Color> sample)
        {
            Color temp = c;
            int min = 1000;
            foreach (Color selectable in sample)
            {
                if (length(c,selectable) < min)
                {
                    temp = selectable;
                    min = length(c, selectable);
                }
            }
            return temp;
        }
        private static int length(Color a, Color b)
        {
            int red = Math.Abs(a.R - b.R);
            int green = Math.Abs(a.G - b.G);
            int blue = Math.Abs(a.B - b.B);
            return red + green + blue;
        }
    }
}

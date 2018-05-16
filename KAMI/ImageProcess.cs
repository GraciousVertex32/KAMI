using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace KAMI
{
    class ImageProcess
    {
        Bitmap img;//loaded image
        Bitmap convert = new Bitmap(800, 500);//converted image
        Color[,] Color = new Color[16, 10];//unclassifed color map for loaded image
        string debug;
        int Width;
        int Height;
        int pixelength;
        int hpl; // half pixel length
        public ImageProcess(Bitmap inputimage)
        {
            img = inputimage;
            Width = Form1.imgwidth;//16
            Height = Form1.imgheight;//10
            pixelength = img.Width / Width;
            hpl = pixelength / 2;
            if (img.Height > 200) { processing(); }
        }
        public float GetAverageHue()
        {
            int hue = 0;
            for (int y = 0; y < img.Height; y++)
            {
                for (int x = 0; x < img.Width; x++)
                {
                    hue += (int)img.GetPixel(x, y).GetHue();
                }
            }
            return hue / (img.Width * img.Height);
        }
        public Color[,] GetColorArray()//get color[,]
        {
            return Color;
        }
        public Bitmap GetConvertedImage()//get small bitmap
        {
            return convert;
        }
        public string GetDebugString()
        {
            return debug;
        }
        private void processing()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color[x, y] = Classify(Averagecolor(x * pixelength + hpl, y * pixelength + hpl));
                    for (int i = 0; i < 50; i++)
                    {
                        for (int j = 0; j < 50; j++)
                        {
                            convert.SetPixel(x * 50 + i, y * 50 + j, Color[x, y]);
                        }
                    }
                    debug += Classifystring(Color[x, y]);//change parameter
                }
                debug += "\r\n";
                debug += "\r\n";
            }
        }
        private Color Classify(Color c)//classify color tybe
        {
            float hue = c.GetHue();
            float lgt = c.GetBrightness();
            float sat = c.GetSaturation();
            double satthreshold = 0.35;
            if (lgt < 0.2) return System.Drawing.Color.Black;
            if (hue < 20 && sat > satthreshold) return System.Drawing.Color.Red;
            if (hue < 90 && sat > satthreshold) return System.Drawing.Color.Yellow;
            if (hue < 150 && sat > satthreshold) return System.Drawing.Color.Green;
            if (hue < 210 && sat > satthreshold) return System.Drawing.Color.Blue;
            if (hue < 270 && sat > satthreshold) return System.Drawing.Color.DarkBlue;
            if (hue < 330 && sat > satthreshold) return System.Drawing.Color.Violet;
            if (hue < 360 && sat > satthreshold) return System.Drawing.Color.Red;
            return System.Drawing.Color.Black;
        }
        private string Classifystring(Color c)//for configuration
        {
            float hue = c.GetHue();
            float lgt = c.GetBrightness();
            float sat = c.GetSaturation();
            double satthreshold = 0.35;
            if (lgt < 0.2) return "黑 ";
            if (hue < 20 && sat > satthreshold) return "红 ";
            if (hue < 90 && sat > satthreshold) return "黄 ";
            if (hue < 150 && sat > satthreshold) return "绿 ";
            if (hue < 210 && sat > satthreshold) return "青 ";
            if (hue < 270 && sat > satthreshold) return "蓝 ";
            if (hue < 330 && sat > satthreshold) return "紫 ";
            if (hue < 360 && sat > satthreshold) return "红 ";
            return "黑 ";
        }
        private string Classifystring(Color c, int a)//for configuration
        {
            float hue = c.GetHue();
            float lgt = c.GetBrightness();
            float sat = c.GetSaturation();
            double satthreshold = 0.35;
            if (lgt < 0.2) return "黑 " + lgt + sat;
            if (hue < 20 && sat > satthreshold) return "红 " + lgt + sat;
            if (hue < 90 && sat > satthreshold) return "黄 " + lgt + sat;
            if (hue < 150 && sat > satthreshold) return "绿 " + lgt + sat;
            if (hue < 210 && sat > satthreshold) return "青 " + lgt + sat;
            if (hue < 270 && sat > satthreshold) return "蓝 " + lgt + sat;
            if (hue < 330 && sat > satthreshold) return "紫 " + lgt + sat;
            if (hue < 360 && sat > satthreshold) return "红 " + lgt + sat;
            return "黑 " + lgt + sat;
        }
        private Color Averagecolor(int x, int y)//get average color for a grid
        {
            int red = 0;
            int green = 0;
            int blue = 0;
            int radius = 5;
            for (int a = x - radius; a <= x + radius; a++)
            {
                for (int b = y - radius; b <= y + radius; b++)
                {
                    red += img.GetPixel(a, b).R;
                    green += img.GetPixel(a, b).G;
                    blue += img.GetPixel(a, b).B;
                }
            }
            red = (red / 121);
            green = (green / 121);
            blue = (blue / 121);
            Color gridcolor = System.Drawing.Color.FromArgb(red, green, blue);
            return gridcolor;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace KAMI
{
    public partial class Form1 : Form//for 
    {
        public static Bitmap img;
        public static int imgwidth;
        public static int imgheight;
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)//load image
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                img = new Bitmap(ofd.FileName);
                pictureBox1.Image = img;
            }
        }
        private void button1_Click_1(object sender, EventArgs e)//access imageprocess class and output result
        {
            imgwidth = int.Parse(textBox1.Text);
            imgheight = int.Parse(textBox2.Text);
            ImageProcess imageProcess = new ImageProcess();
            imageProcess.Regioning();
            textBox3.Text = ImageProcess.debug;
        }
        
    }
    class ImageProcess
    {
        Bitmap img;//loaded image
        Bitmap convert;//converted image
        public static Color[,] Color = new Color[16, 10];//unclassifed color map for loaded image
        public static string debug;
        public ImageProcess()
        {
            img = Form1.img;
        }
        public void Regioning()//imageprocess
        {
            int Width = Form1.imgwidth;//16
            int Height = Form1.imgheight;//10
            int pixelength = img.Width / Width;
            int hpl = pixelength / 2;
            convert = new Bitmap(Width, Height);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color[x, y] = GridReader(x * pixelength + hpl, y * pixelength + hpl);
                    convert.SetPixel(x, y, Color[x, y]);
                    debug += Classify(Color[x, y]);//change parameter
                }
                debug += "\r\n";
                debug += "\r\n";
            }
        }
        public string Classify(Color c)//classify color tybe
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
        public string Classify(Color c, int a)//for configuration
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
        public Color GridReader(int x, int y)//get average color for a grid
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
    class GameStructure
    {
        static Grid[,] imggrid = new Grid[16, 10];
        Color[,] Color = new Color[16, 10];
        public void GetColor()//get color from other class
        {
            Color = ImageProcess.Color;
        }
        public void Init()//set up the grid array
        {
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    Grid grid = new Grid();
                    grid.x = x;
                    grid.y = y;
                    grid.gridcolor = Color[x, y];
                    imggrid[x, y] = grid;
                }
            }
        }
        public class Grid//single color grid
        {
            public Color gridcolor;
            public Area area;
            public int x;
            public int y;
            public Boolean IsSameColor(Grid neighbour)//check if neighbour grid is same color
            {
                return gridcolor.Equals(neighbour);
            }
            public void Connect(Grid neighbour)//connect same color neighbour
            {
                if (neighbour.area == null && this.area == null)
                {
                    Area area = new Area();
                    area.list.Add(this);
                    area.list.Add(neighbour);
                }
                if (this.area == null && neighbour.area != null)
                {
                    this.area = neighbour.area;
                    neighbour.area.list.Add(this);
                }
                if (this.area != null && neighbour.area == null)
                {
                    neighbour.area = this.area;
                    this.area.list.Add(neighbour);
                }
                if (this.area != null && neighbour.area != null && !(this.area.list.SequenceEqual(neighbour.area.list)))//connect after color change
                {
                    this.area.list.AddRange(neighbour.area.list);
                    neighbour.area.list = this.area.list;
                    neighbour.area = this.area;
                }
            }
            public void CheckConnection(Grid current)//check surround grids and connect them if match color
            {
                if (current.x > 0)
                {
                    if (current.IsSameColor(imggrid[current.x - 1, current.y]))
                    {
                        current.Connect(imggrid[current.x - 1, current.y]);
                    }
                }
                if (current.x < 16)
                {
                    if (current.IsSameColor(imggrid[current.x + 1, current.y]))
                    {
                        current.Connect(imggrid[current.x + 1, current.y]);
                    }
                }
                if (current.y > 0)
                {
                    if (current.IsSameColor(imggrid[current.x, current.y - 1]))
                    {
                        current.Connect(imggrid[current.x, current.y - 1]);
                    }
                }
                if (current.y < 16)
                {
                    if (current.IsSameColor(imggrid[current.x, current.y + 1]))
                    {
                        current.Connect(imggrid[current.x, current.y + 1]);
                    }
                }
            }
        }
        public class Area
        {
            public List<Grid> list;//all the grids in the area is stored here
            public void AreaConnection()//do this after color change to merge areas
            {
                Grid grid1 = new Grid();
                foreach (Grid grid in this.list)
                {
                    grid1.CheckConnection(grid);
                }
            }
            public void ChangeColor(Color newcolor)//change area color
            {
                foreach (Grid grid in this.list)
                {
                    grid.gridcolor = newcolor;
                }
            }
        }
    }
}


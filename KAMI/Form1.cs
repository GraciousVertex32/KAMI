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
//Program for solving game KAMI
//step 1:rebuild gamestructure and logic(gamestructure class)
//step 2:image process ability(imageprocess class)
//step 3:get an image of a level(form1)
//step 4:process image into new gamestucture
//step 5:find/write an algorithm for game
namespace KAMI
{
    public partial class Form1 : Form
    {
        public Bitmap img;
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
            ImageProcess imageProcess = new ImageProcess(img);
            pictureBox2.Image = imageProcess.GetConvertedImage();
            textBox3.Text = imageProcess.GetDebugString();
        }
    }
    class ImageProcess
    {
        Bitmap img;//loaded image
        Bitmap convert;//converted image
        Color[,] Color = new Color[16, 10];//unclassifed color map for loaded image
        string debug;
        public ImageProcess(Bitmap inputimage)
        {
            img = inputimage;
            int Width = Form1.imgwidth;//16
            int Height = Form1.imgheight;//10
            int pixelength = img.Width / Width;
            int hpl = pixelength / 2;
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
        private string Classify(Color c)//classify color tybe
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
        private string Classify(Color c, int a)//for configuration
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
        private Color GridReader(int x, int y)//get average color for a grid
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
    public class GameStructure//fuck my life,too hard for me
    {
        Color[,] Color = new Color[16, 10];//from color input
        public Grid[,] imggrid = new Grid[16, 10];//to grid result
        public List<Area> arealist;
        public GameStructure(Color[,] InputColor)//set up the grid array imggrid[,]
        {
            Color = InputColor;
            for (int y = 0; y < 10; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    Grid grid = new Grid(x, y, Color[x, y]);
                    imggrid[x, y] = grid;
                }
            }
        }
        public void ConnectArea(Area changedarea)//do this to merage areas after area color change
        {
            foreach(Grid grid in changedarea.list)
            {
                MakeConnection(grid);
            }
        }
        private void Connect(Grid me, Grid neighbour)//connect same color neighbour
        {
            if (neighbour.area == null && me.area == null)
            {
                Area area = new Area();
                area.list.Add(me);
                area.list.Add(neighbour);
                me.area = area;
                neighbour.area = area;
                arealist.Add(area);
            }
            if (me.area == null && neighbour.area != null)
            {
                me.area = neighbour.area;
                neighbour.area.list.Add(me);
            }
            if (me.area != null && neighbour.area == null)
            {
                neighbour.area = me.area;
                me.area.list.Add(neighbour);
            }
            if (me.area != null && neighbour.area != null && !(me.area.list.SequenceEqual(neighbour.area.list)))//connect after color change
            {
                me.area.list.AddRange(neighbour.area.list);
                neighbour.area.list = me.area.list;
                neighbour.area = me.area;
                arealist.Remove(neighbour.area);//questionable----------------------------
            }
        }
        private void MakeConnection(Grid current)//check surround grids and connect them if match color
        {
            if (current.x > 0)
            {
                if (current.IsSameColor(imggrid[current.x - 1, current.y]))
                {
                    Connect(imggrid[current.x, current.y], imggrid[current.x - 1, current.y]);
                }
            }
            if (current.x < 16)
            {
                if (current.IsSameColor(imggrid[current.x + 1, current.y]))
                {
                    Connect(imggrid[current.x, current.y], imggrid[current.x + 1, current.y]);
                }
            }
            if (current.y > 0)
            {
                if (current.IsSameColor(imggrid[current.x, current.y - 1]))
                {
                    Connect(imggrid[current.x, current.y], imggrid[current.x, current.y - 1]);
                }
            }
            if (current.y < 16)
            {
                if (current.IsSameColor(imggrid[current.x, current.y + 1]))
                {
                    Connect(imggrid[current.x, current.y], imggrid[current.x, current.y + 1]);
                }
            }
        }
    }
    public class Grid//single color grid
    {
        public Area area;
        public Color gridcolor;
        public int x;
        public int y;
        public Grid(int x1, int y1, Color c)
        {
            x = x1;
            y = y1;
            gridcolor = c;
        }
        public Boolean IsSameColor(Grid neighbour)//check color with another grid
        {
            return gridcolor.Equals(neighbour);
        }
    }
    public class Area
    {
        public List<Grid> list;//all the grids in the area
        public void ChangeColor(Color newcolor)//change area color
        {
            foreach (Grid grid in this.list)
            {
                grid.gridcolor = newcolor;
            }
        }
    }
}


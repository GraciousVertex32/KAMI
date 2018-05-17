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
        private Color selectedcolor; // color want to change to 
        Bitmap img; // loaded image
        Bitmap img2; // loaded sample
        List<Color> sample;
        public static int imgwidth;
        public static int imgheight;
        public int chance;
        private GameStructure structure;
        private ImageProcess imageProcess1;
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
            int NumberOfTypes;
            NumberOfTypes = int.Parse(Tbfortypes.Text);
            sample = SampleColor.GetSelectable(img2, NumberOfTypes);
            imgwidth = int.Parse(textBox1.Text);
            imgheight = int.Parse(textBox2.Text);
            chance = int.Parse(textBox4.Text);
            label3.Text = chance.ToString();
            imageProcess1 = new ImageProcess(img, sample);
            pictureBox2.Image = imageProcess1.GetConvertedImage();
            textBox3.Text = imageProcess1.GetDebugString();
            structure = new GameStructure(imageProcess1.GetColorArray(),chance);
        }

        private void pictureBox2_Click(object sender, MouseEventArgs e) // player click this panel to play the game
        {
            int xCoordinate = e.X;
            int yCoordinate = e.Y;
            int x = xCoordinate / 50;
            int y = yCoordinate / 50;
            structure.Click(x, y, selectedcolor);
            label3.Text = structure.GetStep().ToString();
            imageProcess1.Update(structure.Updateimage());
            pictureBox2.Image = imageProcess1.GetConvertedImage();
            if(structure.End())
            {
                MessageBox.Show("YOU WIN!");
            }
        }

        private void pictureBox3_Click(object sender, MouseEventArgs e) // select color like ingame,not used
        {
            int xCoordinate = e.X;
            int yCoordinate = e.Y;
            Bitmap b = ((Bitmap)pictureBox3.Image);
            int x = xCoordinate * b.Width / pictureBox3.ClientSize.Width;
            int y = yCoordinate * b.Height / pictureBox3.ClientSize.Height;
            Color c = b.GetPixel(x, y);
            selectedcolor = Classify.Getclass(c, sample);
        }

        private void button2_Click(object sender, EventArgs e) // load sample file
        {
            OpenFileDialog ofd2 = new OpenFileDialog();
            ofd2.CheckFileExists = true;
            ofd2.CheckPathExists = true;
            if (ofd2.ShowDialog() == DialogResult.OK)
            {
                img2 = new Bitmap(ofd2.FileName);
                pictureBox3.Image = img2;
            }
        }

    }
}


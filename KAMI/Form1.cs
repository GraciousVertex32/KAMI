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
        Bitmap img;
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

        private void pictureBox2_Click(object sender, MouseEventArgs e)
        {
            int xCoordinate = e.X;
            int yCoordinate = e.Y;
            label3.Text = xCoordinate.ToString() + " " + yCoordinate.ToString();
        }
    }
}


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
        public static int imgwidth;
        public static int imgheight;
        public int chance;
        private GameStructure structure;
        private ImageProcess imageProcess;
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
            chance = int.Parse(textBox4.Text);
            label3.Text = chance.ToString();
            imageProcess = new ImageProcess(img);
            pictureBox2.Image = imageProcess.GetConvertedImage();
            textBox3.Text = imageProcess.GetDebugString();
            structure = new GameStructure(imageProcess.GetColorArray(),chance);
        }

        private void pictureBox2_Click(object sender, MouseEventArgs e) // player click this panel to play the game
        {
            int xCoordinate = e.X;
            int yCoordinate = e.Y;
            int x = xCoordinate / 50;
            int y = yCoordinate / 50;
            structure.Click(x, y, selectedcolor);
            label3.Text = structure.GetStep().ToString();
            imageProcess.Update(structure.Updateimage());
            pictureBox2.Image = imageProcess.GetConvertedImage();
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
            label3.Text = c.ToString();

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) // select color to change
        {
            if(comboBox1.SelectedIndex == 0)
            {
                selectedcolor = Color.Red;
            }
            if (comboBox1.SelectedIndex == 1)
            {
                selectedcolor = Color.Yellow;
            }
            if (comboBox1.SelectedIndex == 2)
            {
                selectedcolor = Color.Green;
            }
            if (comboBox1.SelectedIndex == 3)
            {
                selectedcolor = Color.Blue;
            }
            if (comboBox1.SelectedIndex == 4)
            {
                selectedcolor = Color.DarkBlue;
            }
            if (comboBox1.SelectedIndex == 5)
            {
                selectedcolor = Color.Violet;
            }
            if (comboBox1.SelectedIndex == 6)
            {
                selectedcolor = Color.Black;
            }

        }
    }
}


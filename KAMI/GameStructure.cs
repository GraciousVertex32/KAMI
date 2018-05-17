using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace KAMI
{
    class GameStructure//fuck my life,too hard for me
    {
        private int trials;
        private static int width = 16;
        private static int height = 10;
        Grid[,] imggrid = new Grid[width, height];//to grid result
        Color[,] returncolor = new Color[width, height];
        public GameStructure(Color[,] InputColor,int chance)//set up the grid array imggrid[,]
        {
            this.trials = chance;
            Array.Copy(InputColor, returncolor,InputColor.Length);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Grid grid = new Grid(x, y, InputColor[x, y]);
                    imggrid[x, y] = grid;
                }
            }
            foreach (Grid grid in imggrid) // set neighbour for each grid
            {
                if(grid.X > 0)
                {
                    grid.Left = GetGrid(grid.X - 1, grid.Y);
                }
                if (grid.Y > 0)
                {
                    grid.Up = GetGrid(grid.X, grid.Y - 1);
                }
                if (grid.X + 1 < width)
                {
                    grid.Right = GetGrid(grid.X + 1, grid.Y);
                }
                if (grid.Y + 1 < height)
                {
                    grid.Down = GetGrid(grid.X, grid.Y + 1);
                }
            }
        }
        public void Click(int x, int y, Color c) // for outer class to change color
        {
            Operation1(GetGrid(x, y), c);
            trials = trials - 1;
        }
        public Color[,] Updateimage() // new image information for visualization
        {
            return returncolor;
        }
        public int GetStep()
        {
            return trials;
        }
        public bool End()
        {
            Color c = imggrid[0, 0].Gridcolor;
            foreach (Grid grid in imggrid)
            {
                if (c != grid.Gridcolor)
                {
                    return false;
                }
            }
            return true;
        }
        private void Operation1(Grid grid,Color newcolor) // this change the color
        {
            foreach (Grid part in AllConnectedGrid(grid))
            {
                part.Gridcolor = newcolor;
                returncolor[part.X, part.Y] = newcolor;
            }
        }
        private IEnumerable<Grid> AllConnectedGrid(Grid origin) // return all connected block
        {
            IList<Grid> connected = new List<Grid>();
            Queue<Grid> pq = new Queue<Grid>();
            Grid temp;
            pq.Enqueue(origin);
            connected.Add(origin);
            bool[,] visited = new bool[width, height];
            while (pq.Count != 0)
            {
                if (visited[pq.First<Grid>().X, pq.First<Grid>().Y] == false)
                {
                    temp = pq.Dequeue();
                    // try to get temp drawed one by one
                    visited[temp.X, temp.Y] = true;
                    if (temp.Left != null && temp.CompareColor(temp.Left))
                    {
                        connected.Add(temp.Left);
                        pq.Enqueue(temp.Left);
                    }
                    if (temp.Right != null && temp.CompareColor(temp.Right))
                    {
                        connected.Add(temp.Right);
                        pq.Enqueue(temp.Right);
                    }
                    if (temp.Up != null && temp.CompareColor(temp.Up))
                    {
                        connected.Add(temp.Up);
                        pq.Enqueue(temp.Up);
                    }
                    if (temp.Down != null && temp.CompareColor(temp.Down))
                    {
                        connected.Add(temp.Down);
                        pq.Enqueue(temp.Down);
                    }
                }
                else
                {
                    pq.Dequeue();
                }
            }
            return connected;
        }
        private Grid GetGrid(int x, int y) // get grid from x,y
        {
            return imggrid[x, y];
        }
    }
    class Grid//single color grid
    {
        public Color Gridcolor { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Grid Left { get; set; }
        public Grid Right { get; set; }
        public Grid Up { get; set; }
        public Grid Down { get; set; }
        public Grid(int x1, int y1, Color c)
        {
            X = x1;
            Y = y1;
            Gridcolor = c;
        }
        public bool CompareColor(Grid that)
        {
            return this.Gridcolor == that.Gridcolor;
        }
    }
}

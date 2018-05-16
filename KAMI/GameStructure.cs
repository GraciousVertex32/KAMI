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
        private static int width = 16;
        private static int height = 10;
        Grid[,] imggrid = new Grid[width, height];//to grid result
        public GameStructure(Color[,] InputColor)//set up the grid array imggrid[,]
        {
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
                if(grid.X - 1 > 0)
                {
                    grid.Left = GetGrid(grid.X - 1, grid.Y);
                }
                if (grid.Y - 1 > 0)
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
        private void Operation(Grid grid,Color newcolor)
        {
            foreach (Grid part in AllConnectedGrid(grid))
            {
                part.Gridcolor = newcolor;
            }
        }
        private IEnumerable<Grid> AllConnectedGrid(Grid origin)
        {
            IList<Grid> connected = new List<Grid>();
            Queue<Grid> pq = new Queue<Grid>();
            Grid temp;
            pq.Enqueue(origin);
            connected.Add(origin);
            while (pq.Count != 0)
            {
                temp = pq.Dequeue();
                if (temp.CompareColor(temp.Left))
                {
                    connected.Add(temp.Left);
                    pq.Enqueue(temp.Left);
                }
                if (temp.CompareColor(temp.Right))
                {
                    connected.Add(temp.Right);
                    pq.Enqueue(temp.Right);
                }
                if (temp.CompareColor(temp.Up))
                {
                    connected.Add(temp.Up);
                    pq.Enqueue(temp.Up);
                }
                if (temp.CompareColor(temp.Down))
                {
                    connected.Add(temp.Down);
                    pq.Enqueue(temp.Down);
                }
            }
            return connected;
        }
        private Grid GetGrid(int x, int y)
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

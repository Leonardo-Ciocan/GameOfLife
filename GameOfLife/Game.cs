using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife
{
    public class Game
    {
        public static Cell[,] Universe = new Cell[150, 100];

        public delegate void RedrawHandler();
        public static event RedrawHandler Redraw;

        public static void InitializeUniverse()
        {
            for (int i = 0; i < 150; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    Universe[i,j]=new Cell();
                }
            }
        }

        public static int getAdjecentCells(int x, int y)
        {
            int adjecent = 0;
            if( x+1 < Universe.GetLength(0))
                if (Game.Universe[x + 1, y].Alive) adjecent++;

            if (y + 1 <  Universe.GetLength(1)) 
                if (Game.Universe[x, y + 1].Alive) adjecent++;

            if (x + 1 < Universe.GetLength(0) && y + 1 < Universe.GetLength(1))
                if (Game.Universe[x + 1, y + 1].Alive) adjecent++;

            if (x - 1 >0)
                if (Game.Universe[x - 1, y].Alive) adjecent++;

            if (y-1 >0) 
                if (Game.Universe[x, y - 1].Alive) adjecent++;

            if (y-1>0 && x-1>0)
                if (Game.Universe[x - 1, y - 1].Alive) adjecent++;

            if (x-1>0 && y+1 < Universe.GetLength(1))
                if (Game.Universe[x - 1, y + 1].Alive) adjecent++;

            if (y - 1 > 0 && x + 1 < Universe.GetLength(0))
                if (Game.Universe[x + 1, y - 1].Alive) adjecent++;
            return adjecent;
        }

        public static int TotalGenerations = 0;
        public static void CalculateNextGeneration()
        {
            Cell[,] NextUniverse = new Cell[150, 100];
            for (int i = 0; i < 150; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    NextUniverse[i,j]=new Cell();
                    int adjecent = getAdjecentCells(i, j);
                    if (Universe[i, j].Alive)
                    {
                        if (adjecent < 2 && adjecent > 3) NextUniverse[i, j].Alive = false;
                        if (adjecent > 1 && adjecent < 4)
                        {
                            NextUniverse[i, j].Alive = true;
                            NextUniverse[i, j].Generation = Universe[i, j].Generation + 1;
                        }
                    }
                    else
                    {
                        if (adjecent == 3) NextUniverse[i, j].Alive = true;
                    }

                }
            }
            TotalGenerations ++;
            Universe = NextUniverse;
            Redraw();
        }
    }
}

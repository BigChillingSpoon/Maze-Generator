using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MazeRecursiveBacktracking;


public class Maze
{
    static int cellSize;
    static char fillSymbol;
    readonly static char backgroundSymbol = ' ';
    static int mazeSize;
    static Cell[][]? maze;
    public Maze(int sizeOfMaze, char fillCharacter,int sizeOfOneCell) 
    { 
        mazeSize= sizeOfMaze;
        fillSymbol= fillCharacter;
        cellSize= sizeOfOneCell;
        maze = new Cell[mazeSize][];
        GenerateMazeEmpty();
        GenerateMaze();
    }
    public void Print()
    {
        
        for (int i = 0; i < mazeSize; i++)
        {
            for (int row = 0; row < cellSize; row++)
            {
                for (int k = 0; k < mazeSize; k++)
                {
                    maze![i][k].MazePrintRow(row);
                }
                Console.WriteLine();
            }
        }
        
        
    }
    static void GenerateMaze()
    {
        Random rnd = new();
        List <int[]> visitedCellsSorted = new();
        List<int[]> nextPossiblePositions = new();

        int[] coordinatesIJ = new int[4];
        int coI = rnd.Next(0, mazeSize);
        int coJ = rnd.Next(0, mazeSize);
        visitedCellsSorted.Add(new int[] { coI, coJ });
        int displacementI, displacementJ;

        Cell neighbourCell = new Cell(0,0);
        //Cell mainCell= new Cell(0,0);
        do
        {
            nextPossiblePositions.Clear();
            displacementI = 0; displacementJ = 0;
           
            coordinatesIJ[0] = coI;
            coordinatesIJ[1] = coJ;


            

            maze![coI][coJ].isVisited = true;
            for (int i = 0; i < 4; i++)
            {
                (displacementI, displacementJ) = DisplacementIJ(displacementI, displacementJ, i);
                neighbourCell.posI = coI+displacementI;
                neighbourCell.posJ = coJ + displacementJ;
                if (neighbourCell.posI < mazeSize && neighbourCell.posJ < mazeSize && neighbourCell.posI >= 0 && neighbourCell.posJ >= 0)
                {
                    if (!maze[neighbourCell.posI][neighbourCell.posJ].isVisited)
                    {
                        nextPossiblePositions.Add(new int[] { coI + displacementI,coJ+ displacementJ ,displacementI,displacementJ});
                    }
                }

            }
           
            if (nextPossiblePositions.Count > 0)
            {
                //tady se radeji nekoukat;)
                coordinatesIJ = nextPossiblePositions[rnd.Next(0, nextPossiblePositions.Count)];
                CellEmptyWall(0);
                coI = coordinatesIJ[0];
                coJ = coordinatesIJ[1];
                visitedCellsSorted.Add(new int[] { coI, coJ });
                CellEmptyWall(1);
            }
            else
            {
                coordinatesIJ = visitedCellsSorted[visitedCellsSorted.Count - 1];
                coI = coordinatesIJ[0];
                coJ = coordinatesIJ[1];
                //Console.WriteLine(coI + "." + coJ);
                visitedCellsSorted.RemoveAt(visitedCellsSorted.Count - 1);
            }
            
        } while (visitedCellsSorted.Count > 0);
        foreach (int[] i in visitedCellsSorted)
        {
            Console.WriteLine(i[0] + "," + i[1] + "kys");
        }
    void CellEmptyWall(int option)
    {
        switch (option)
        {
                //Current Main Cell
                case 0:
                    if (coordinatesIJ[2] == -1 && coordinatesIJ[3] == 0) maze![coI][coJ].CellChangeMark(0, 0);
                    else if (coordinatesIJ[2] == 0 && coordinatesIJ[3] == -1) maze![coI][coJ].CellChangeMark(0,1);
                    else if (coordinatesIJ[2] == 0 && coordinatesIJ[3] == 1) maze![coI][coJ].CellChangeMark(0, 2);
                    else if (coordinatesIJ[2] == 1 && coordinatesIJ[3] == 0) maze![coI][coJ].CellChangeMark(0,3);
                    break;  
                //Next Main Cell
                case 1:
                    if (coordinatesIJ[2] == -1 && coordinatesIJ[3] == 0) maze![coI][coJ].CellChangeMark(0, 3);
                    else if (coordinatesIJ[2] == 0 && coordinatesIJ[3] == -1) maze![coI][coJ].CellChangeMark(0, 2);
                    else if (coordinatesIJ[2] == 0 && coordinatesIJ[3] == 1) maze![coI][coJ].CellChangeMark(0, 1);
                    else if (coordinatesIJ[2] == 1 && coordinatesIJ[3] == 0) maze![coI][coJ].CellChangeMark(0, 0);
                    break;
        }
        
    }

    static (int, int) DisplacementIJ(int displacementI, int displacementJ, int pomI)
        {

            switch (pomI)
            {
                case 0:
                    {
                        displacementI--;
                    }
                    break;
                case 1:
                    {
                        displacementI++;
                        displacementJ--;
                    }
                    break;
                case 2:
                    {
                        displacementJ += 2;
                    }
                    break;
                case 3:
                    {
                        displacementI++;
                        displacementJ--;
                    }
                    break;
            }
            return (displacementI, displacementJ);
        }
    }


    static void GenerateMazeEmpty()
    {
            
        for (int i = 0; i < mazeSize; i++)
        {
            maze![i] = new Cell[mazeSize];
            for (int j = 0; j < mazeSize; j++)
            {
                maze[i][j] = new Cell(i, j);
            }
        }
           
    }
       
    
    
    internal class Cell
    {

        internal protected bool isVisited = false;
        int[] cellWallsMark = new int[] {1,1,1,1};
        char[,] cellAppearance = new char[cellSize, cellSize];
        char cellFillSymbol;
        internal protected int posI,posJ;
        readonly char cellBackgroundSymbol =' ';
        internal Cell(int posI,int posJ)
        {
            this.cellFillSymbol = fillSymbol;
            this.cellBackgroundSymbol = backgroundSymbol;
            this.posI = posI;
            this.posJ = posJ;
            CellGenerate();
        }

        internal void CellShowMark()
        {
            foreach (int i in cellWallsMark) Console.Write(i);
            Console.WriteLine();
        }
        internal void CellChangeMark(int replaceWith, int index)
        {
            cellWallsMark[index] = replaceWith;
            CellGenerate();
        }
        private void CellGenerate()
        {
            
            for (int i = 0, indexOfMark = 0; i < cellSize; i++)
            {
                for (int j = 0; j < cellSize; j++)
                {
                    //vsechny krajni sloupce a radky se vyplni
                    if (j == 0 || i == 0) cellAppearance[i, j] = fillSymbol;
                    //podminka pro detekci sten 
                    if (((i == 0 || i == cellSize - 1) && (j > 0 && j < cellSize - 1)) || ((j == 0 || j == cellSize - 1) && (i > 0 && i < cellSize - 1)))
                    {
                        cellAppearance[i, j] = (cellWallsMark[indexOfMark] == 1) ? cellFillSymbol : cellBackgroundSymbol;
                        //displacementuti indexu Marku
                        indexOfMark = CellChangeWall( i, j, indexOfMark);
                    }
                    else if (i > 0 && i < cellSize - 1 && j > 0 && j < cellSize - 1) cellAppearance[i, j] = cellBackgroundSymbol;
                    else cellAppearance[i, j] = fillSymbol;
                }
            }
            static int CellChangeWall(int i, int j, int index)
            {
                return index switch
                {
                    0 => (j == cellSize - 2) ? 1 : 0,
                    1 => (j == 0) ? 2 : 1,
                    2 => (j == cellSize - 1 && i < cellSize - 2) ? 1 : (i == cellSize - 2 && j == cellSize - 1) ? 3 : 2,
                    _ => 3
                };
            }
        }
        internal void MazePrintRow(int row)
        {
            for (int j = 0; j < cellSize; j++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write(" " + cellAppearance[row, j]);
            }
        }

    }
    
}

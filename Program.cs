using System;

namespace MazeRecursiveBacktracking;

internal class Program
{
    static void Main(string[] args)
    {
        //118,4
        //158,3
        const int mazeSize = 158;
        const int cellSize = 3;

        char mazeFillChar = '■';
        Maze maze=new Maze(mazeSize, mazeFillChar, cellSize);
        maze.Print();
       
    }
    

}


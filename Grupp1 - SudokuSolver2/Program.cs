using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupp1Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {

            //Sudoku game = new Sudoku("003020600900305001001806400" +
            //                         "008102900700000008006708200" +
            //                         "002609500800203009005010300");
            Sudoku game = new Sudoku("060001020970820400035004001604000018007000200820000605700900130002067094040500080");
            game.PrintSudoku();
            game.Solve(ref Sudoku.board);

            Console.ReadLine();
        }


    }
}

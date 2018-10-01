using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupp1Sudoku
{
    class Program
    {
        private const string easy3 = "060001020970820400035004001604000018007000200820000605700900130002067094040500080";
        private const string easy1 = "003020600900305001001806400008102900700000008006708200002609500800203009005010300";
        private const string easy2 = "619030040270061008000047621486302079000014580031009060005720806320106057160400030";
        private const string medium1 = "037060000205000800006908000000600024001503600650009000000302700009000402000050360";
        private const string diabolic1 = "000000000000003085001020000000507000004000100090000000500000073002010000000040009";
        private const string zen = "000000000000000000000000000000000000000010000000000000000000000000000000000000000";
        private const string unsolvable1 = "0090287008.6..4..5..3.....46.........2.71345.........23.....5..9..4..8.7..125.3..";

        static void Main(string[] args)
        {

            //Sudoku game = new Sudoku("003020600900305001001806400" +
            //                         "008102900700000008006708200" +
            //                         "002609500800203009005010300");
            Sudoku game = new Sudoku(medium1);
            game.PrintSudoku(Sudoku.board);
            game.Solve(Sudoku.board); //ref?

            Console.ReadLine();
        }

        
    }
}

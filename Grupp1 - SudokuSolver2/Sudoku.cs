using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grupp1Sudoku
{
    public class Sudoku
    {
        public static int[,] board;      //originalbrädet

        //int[,] boardCopy;       //kopia av brädet
        //List<int> emptyCellCoordinates = new List<int>();  //används när vi söker tom cell i Fas 2
        //List<int> PossibleNumbers = new List<int>();

        public Sudoku(string input)
        {
            board = TransformStringToArray(input);
        }

        //Metod som söker efter enda möjliga siffra för en cell
        public int FindOnlyPossibleNumber(int cellY, int cellX)
        {
            bool[] eliminatedNumbers = new bool[9];

            //Search the row
            for (int x = 0; x < 9; x++)
            {
                int cellVal = board[cellY, x];
                if (cellVal != 0)
                {
                    eliminatedNumbers[cellVal - 1] = true;
                }
            }

            //Search the column
            for (int y = 0; y < 9; y++)
            {
                int cellVal = board[y, cellX];
                if (cellVal != 0)
                {
                    eliminatedNumbers[cellVal - 1] = true;
                }
            }

            //Search the block
            int blockX = cellX / 3;
            int blockY = cellY / 3;

            //hitta block koordinater
            for (int y = blockY * 3; y < (blockY * 3) + 3; y++)
            {
                for (int x = blockX * 3; x < (blockX * 3) + 3; x++)
                {
                    int cellVal = board[y, x];
                    if (cellVal != 0)
                    {
                        eliminatedNumbers[cellVal - 1] = true;
                    }
                }
            }
            int trueCount = 0;
            foreach (bool b in eliminatedNumbers)
            {
                if (b == true)
                {
                    trueCount++;
                }
            }

            if (trueCount == 8)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (eliminatedNumbers[i] == false)
                    {
                        return i + 1;
                    }
                }
            }
            return 0;
        }

        public void Solve(int[,] board)  
        {                               

            int[,] boardCopy = (int[,])board.Clone(); // gör kopian av brädet
            bool filledAnyCell;
           
            do
            {
                filledAnyCell = false;

                for (int row = 0; row < 9; row++)
                {
                    for (int col = 0; col < 9; col++)
                    {
                        if (board[row, col] == 0)
                        {
                            int newCellValue = FindOnlyPossibleNumber(row, col);
                            if (newCellValue != 0)
                            {
                                board[row, col] = newCellValue;
                                filledAnyCell = true;
                            }
                        }
                    }
                }

            } while (filledAnyCell); 
            int numberOfEmptyCells = EmptyCells(board); // Kollar om Solve1 har avslutats pga inga tomma eller om det finns tomma kvar.

            if (numberOfEmptyCells == 0)
            {
                Console.WriteLine("Finished");
                PrintSudoku(board);
            }
            else
            {
                var result = SolveRecursive(boardCopy);  //Letar upp första bästa tomma cell. Returnerar brädet. Anropar sig själv. 
                
               
                if (result == null) //possiblenumber = 0 då vet vi att det första talet är fel
                {
                    Console.WriteLine("Olöslig!");
                }
                else
                {
                    PrintSudoku(result);
                }
            }

        }
        //anropa Metod: FindEmptyCell() när den hittat en tom cell,
        //anropa Metod: FindPossibleNumbersForCell() : spara i lista
        // Return True/false (if false = hela brädet olösligt, if true= fortsätt med nedan kod)
        // Clone() (Kopiera brädet)
        // ANropa metod: FirstPossibleNumber()
        //Skriv in första möjliga siffra på kopian
        //Kör FindOnlyPossibleNumber()
        //Return True eller False
        //True = vinst
        //False = Släng kopian, ta ny kopia och lägg in nästa möjliga siffra i cell
        //När alla möjliga siffror testats i cellen (for-loop)
        //If False = olösligt bräde       

        ///////////////////////////////////////////////////////////////////////////////////////////////////

        public List<int> FindPossibleNumbers(int[,] boardCopy, int cellY, int cellX)
        {
            bool[] eliminatedNumbers = new bool[9];
            List<int> PossibleNumbers = new List<int>();

            //Search the row
            for (int x = 0; x < 9; x++)
            {
                int cellVal = boardCopy[cellY, x]; //board[cellY, x]
                if (cellVal != 0)
                {
                    eliminatedNumbers[cellVal - 1] = true;
                }
            }

            //Search the column
            for (int y = 0; y < 9; y++)
            {
                int cellVal = boardCopy[y, cellX]; //board[y,cellX]
                if (cellVal != 0)
                {
                    eliminatedNumbers[cellVal - 1] = true;
                }
            }
            //Search the block
            int blockX = cellX / 3; //cellX
            int blockY = cellY / 3; //cellY

            //hitta block koordinater
            for (int y = blockY * 3; y < (blockY * 3) + 3; y++)
            {
                for (int x = blockX * 3; x < (blockX * 3) + 3; x++)
                {
                    int cellVal = boardCopy[y, x]; // board
                    if (cellVal != 0)
                    {
                        eliminatedNumbers[cellVal - 1] = true;
                    }
                }
            }
            int trueCount = 0;

            foreach (bool b in eliminatedNumbers)
            {
                if (b == true)
                {
                    trueCount++;
                }
            }

            for (int i = 0; i < 9; i++)
            {
                if (eliminatedNumbers[i] == false)
                {
                    PossibleNumbers.Add(i + 1);  // listan med möjliga värden för cellen
                }          
            }           
            return PossibleNumbers; // vi returnerar den aktuella listan så att den kan användas av rek. metoden. 
        }

        public int[,] SolveRecursive(int[,] board) //omdöpt FindfirstEmtyCell
        {
            if (board == null) // om den kommer in med null, brädet är klart, förlust. 
            {
                return null;
            }
            if (EmptyCells(board) == 0) //klart! Det finns inga tomma celler. 
            {              
                return board;
            }

            int[,] boardCopy = (int[,])board.Clone(); // kopierar brädet.

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (boardCopy[y, x] == 0)
                    {
                        List<int> possibleNumbers = new List<int>();
                        possibleNumbers = FindPossibleNumbers(boardCopy, y, x);  
                        if (possibleNumbers.Count == 0) // det aktuellla cellen har inga alernativ
                            return null;
                        if(possibleNumbers.Count == 1) // när brädet testas kan nästkommande celler få endast ett möjligt värde/siffra
                        {
                            boardCopy[y, x] = possibleNumbers.FirstOrDefault();
                            return SolveRecursive(boardCopy);
                        }
                            
                        for (int i = 0; i < possibleNumbers.Count; i++)
                        {
                            boardCopy[y, x] = possibleNumbers[i]; //uppdaterar kopian. 
                            var next = SolveRecursive(boardCopy);
                            if (next == null) // första siffran  testas, om null, siffran var fel då brädet inte gick att lösa, fortsätt med nästa siffra
                            {
                                continue;
                            }else
                            {
                                return SolveRecursive(boardCopy);
                            }
                            //Solve ska returnera true eller false, för varje siffra i PossibleNumbers
                            //När hela loopen körts: vinst eller olösbart
                        }                   
                    }                  
                } 
            }
            return (board);
        }
        private int[,] TransformStringToArray(string input)
        {
            int[,] newboard = new int[9, 9];  //Skapa en array[9, 9]
            for (int row = 0; row < newboard.GetLength(1); row++) // For loop Y
            {
                for (int col = 0; col < newboard.GetLength(0); col++)//For loop X
                {
                    newboard[row, col] = int.Parse(input[col + (row * 9)].ToString());
                }
            }
            return newboard;
        }

        public void PrintSudoku(int[,] board)
        {
            for (int y = 0; y < board.GetLength(1); y++) //For-loop för kolumnen
            {
                if (y % 3 == 0)
                {
                    Console.WriteLine("-------------------------"); //Skriv ut strecksrad
                }

                for (int x = 0; x < board.GetLength(0); x++) //For-loop för raden
                {
                    if (x % 3 == 0)
                    {
                        Console.Write("| ");
                    }
                    if (board[y, x] == 0)
                    {
                        Console.Write("  ");
                    }
                    else
                    {
                        Console.Write(board[y, x] + " ");    //Annars: Skriv ut siffran
                    }
                }
                Console.Write("|\n");
            }
            Console.WriteLine("-------------------------"); //Skriv ut understrecksrad
        }


        public int EmptyCells(int[,] board)
        {
            int count = 0;

            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (board[y, x] == 0)
                    {
                        count++;
                    }

                }
            }
            return count;

        }
    }
}

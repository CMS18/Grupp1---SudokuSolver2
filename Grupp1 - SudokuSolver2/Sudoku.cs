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
        int[,] boardCopy;       //kopia av brädet
        List<int> emptyCellCoordinates = new List<int>();  //används när vi söker tom cell i Fas 2
        List<int> PossibleNumbers = new List<int>();



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


        public void Solve(ref int[,] board)  //det är väl Solve-metoden vi ska anropa rekursivt??
        {                                //skickar in board som argument
            bool filledAnyCell;
            bool manyGuess;
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
                                                                                                                     
            } while (filledAnyCell); //eller ska denna loop sluta ngn annnstans??   

            FindFirstEmptyCell(); //Letar upp första bästa tomma cell

            // Om FindFirstEmtyCell inte hittar någon tom cell == förlust! Ngn if sats här som går till Print() else den rekursiva delen
            //om inga fyllda celler under ett varv; OCH det finns tomma celler kvar gå vidare med rekursiv lösning
            boardCopy = (int[,])board.Clone();
            do
            {
                manyGuess = false;                
                int cellvalue = FindPossibleNumbers(emptyCellCoordinates[0], emptyCellCoordinates[1]);//Argument: y och x-koordinater från metoden FindFirstEmptyCell

                    if (cellvalue != 0)
                    {
                        TryCellValue();
                    }
                    else
                    {
                        manyGuess = true; 
                    }
                
                
            } while (manyGuess);
            PrintSudoku();

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
        
        public int FindPossibleNumbers(int cellY, int cellX) 
        {
            bool[] eliminatedNumbers = new bool[9];

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
            if (trueCount == 9)
            {
                Console.WriteLine("Brädet är olösligt.");
                return 0; //returnerar noll om det inte finns någon siffra att sätta in. 
               // PrintSudoku();//Avbryt spelet på något sätt (break funkar ej??)
            }
            else
            {
                for (int i = 0; i < 9; i++)
                {
                    if (eliminatedNumbers[i] == false)
                    {
                       PossibleNumbers.Add(i + 1);  // listan med möjliga värden för cellen
                    }
                }
                return 1; //returnerar 1. Vi behöver inte returnera listans värde här eftersom vi hanterar det sen i Trymetoden.
            }
        }

        public void FindFirstEmptyCell()
        {
            bool goAhead = true;
            while (goAhead)
            {
                for (int y = 0; y < 9; y++)
                {
                    for (int x = 0; x < 9; x++)
                    {
                        if (boardCopy[y, x] == 0)
                        {
                            emptyCellCoordinates.Add(y);
                            emptyCellCoordinates.Add(x);
                            goAhead = false;
                            y = 9;
                            break;          //varför fortsätter loopen fast den hittat en tom cell? break stoppar närmaste loopen 
                                            // satte därför y till 9 för att avbryta helt. Ändrade även boolvillkoret för att tydligare avbryta loopen.
                                            // för vi vill väl bara kolla en tom cell?
                        }

                    }
                }
            }
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
        //Metod med loop för att testa de möjliga numren i den cellen.
        public void TryCellValue()// int row, int col
        {
            int y = emptyCellCoordinates[0];
            int x = emptyCellCoordinates[1];

            for (int i = 0; i < PossibleNumbers.Count; i++)
            {
                boardCopy[y, x] = PossibleNumbers[i]; // y,x //uppdaterar kopian. 
                Solve(ref boardCopy);               //lägger till ref i argument, tror det ska vara det??
                //Solve ska returnera true eller false, för varje siffra i PossibleNumbers
                //När hela loopen körts: vinst eller olösbart
            }
            // Behöver vi någonstans kasta vårt bräde och göra en ny kopia? Ja! men var?
        }
    }
}

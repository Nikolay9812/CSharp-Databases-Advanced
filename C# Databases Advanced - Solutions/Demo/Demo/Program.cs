using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            int size = int.Parse(Console.ReadLine());
            int countOfCommands = int.Parse(Console.ReadLine());

            char[,] field = new char[size, size];

            int fRow = 0;
            int fCol = 0;

            for (int row = 0; row < size; row++)
            {
                char[] currRow = Console.ReadLine().ToCharArray();

                for (int col = 0; col < size; col++)
                {
                    field[row, col] = currRow[col];

                    if (currRow[col] == 'f')
                    {
                        fRow = row;
                        fCol = col;
                    }
                }
            }
            int count = 0;

            field[fRow, fCol] = '-';

            while (true)
            {
                string command = Console.ReadLine();

                if (command == "up")
                {
                    fRow--;
                    if (fRow == 0)
                    {
                        fRow = field.GetLength(0) - 1;
                    }
                }
                else if (command == "down")
                {
                    fRow++;
                    if (fRow == field.GetLength(0) - 1)
                    {
                        fRow = 0;
                    }
                }
                else if (command == "left")
                {
                    fCol--;
                    if (fCol == 0)
                    {
                        fCol = field.GetLength(1);
                    }

                }
                else if (command == "right")
                {
                    fCol++;
                    if (fCol == field.GetLength(1))
                    {
                        fCol = 0;
                    }
                }

                if (field[fRow, fCol] == 'B')
                {

                }

                if (field[fRow, fCol] == 'T')
                {

                }

                if (field[fRow, fCol] == 'F')
                {
                    Console.WriteLine("Player won!");
                    break;
                }

                Print(field);

                count++;

                if (count == countOfCommands)
                {
                    Console.WriteLine("Player lost!");
                    break;
                }
            }

            Print(field);
        }

        private static void Print(char[,] field)
        {
            for (int row = 0; row < field.GetLength(0); row++)
            {
                for (int col = 0; col < field.GetLength(1); col++)
                {
                    Console.Write(field[row, col]);
                }

                Console.WriteLine();
            }
        }
    }
}

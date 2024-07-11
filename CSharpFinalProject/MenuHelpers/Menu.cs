using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFinalProject.MenuHelpers
{
    internal static class Menu
    {
        public static void PrintTitle(string title)
        {
            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine();
        }

        public static string ShowMenu(string? title, List<string> args, string? menuInformation = null, bool clearConsole = true)
        {
            int currentIndex = 0;
            if (clearConsole)
                Console.Clear();

            int startLine = Console.CursorTop;

            while (true)
            {
                Console.SetCursorPosition(0, startLine);

                for (int i = 0; i < args.Count + (menuInformation is not null ? 3 : 2); i++)
                {
                    Console.WriteLine(new string(' ', Console.WindowWidth));
                }

                Console.SetCursorPosition(0, startLine);

                Console.BackgroundColor = ConsoleColor.Black;
                if (title is not null)
                {
                    Console.WriteLine(title);
                    Console.WriteLine();
                }

                if (menuInformation is not null)
                {
                    Console.WriteLine(menuInformation);
                    Console.WriteLine();
                }

                for (int i = 0; i < args.Count; i++)
                {
                    if (currentIndex == i)
                        Console.BackgroundColor = ConsoleColor.Green;
                    else
                        Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine($"{i + 1}. {args[i]}");
                }
                Console.BackgroundColor = ConsoleColor.Black;
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.DownArrow)
                    currentIndex++;
                else if (key.Key == ConsoleKey.UpArrow)
                    currentIndex--;
                else if (key.Key == ConsoleKey.Enter)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                    return args[currentIndex];
                }

                if (currentIndex == -1)
                    currentIndex = args.Count - 1;
                else if (currentIndex == args.Count)
                    currentIndex = 0;
            }
        }

        private static void ClearConsoleLine()
        {
            int currentLine = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLine);
        }

        public static double ShowQuantitySelector()
        {
            double quantity = 1;
            Console.WriteLine("Select -1 to start default quantity chooser.");
            int startLine = Console.CursorTop;
            while (true)
            {
                Console.SetCursorPosition(0, startLine);
                ClearConsoleLine();
                Console.WriteLine("Quantity: "+quantity);

                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.DownArrow)
                    quantity--;
                else if (key.Key == ConsoleKey.UpArrow)
                    quantity++;
                else if (key.Key == ConsoleKey.Enter)
                {
                    if (quantity == -1)
                    {
                        while (true)
                        {
                            Console.Write("Default quantity: ");
                            double qr;
                            if (double.TryParse(Console.ReadLine()?.Replace(".",","), out qr))
                            {
                                return qr;
                            }
                            else
                            {
                                Console.WriteLine("Wrong input!");
                            }
                        }
                    }
                    return quantity;
                }
                if (quantity <= -2)
                    quantity = -1;
            }
        }
    }
}

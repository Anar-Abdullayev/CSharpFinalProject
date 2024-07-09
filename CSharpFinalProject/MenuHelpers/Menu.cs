using System;
using System.Collections.Generic;
using System.Linq;
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

        //public static string ShowMenu2(string title, List<string> args, string? menuInformation)
        //{
        //    int currentIndex = 0;
        //    while (true)
        //    {
        //        Console.BackgroundColor = ConsoleColor.Black;
        //        Console.Clear();
        //        Console.WriteLine(title);
        //        Console.WriteLine();
        //        if (menuInformation is not null)
        //        {
        //            Console.WriteLine(menuInformation);
        //            Console.WriteLine();
        //        }
        //        for (int i = 0; i < args.Count; i++)
        //        {
        //            if (currentIndex == i)
        //                Console.BackgroundColor = ConsoleColor.Green;
        //            else
        //                Console.BackgroundColor = ConsoleColor.Black;
        //            Console.WriteLine($"{i + 1}. {args[i]}");
        //        }
        //        ConsoleKeyInfo key = Console.ReadKey();
        //        if (key.Key == ConsoleKey.DownArrow)
        //            currentIndex++;
        //        else if (key.Key == ConsoleKey.UpArrow)
        //            currentIndex--;
        //        else if (key.Key == ConsoleKey.Enter)
        //        {
        //            Console.BackgroundColor = ConsoleColor.Black;
        //            Console.ForegroundColor = ConsoleColor.White;
        //            return args[currentIndex];
        //        }

        //        if (currentIndex == -1)
        //            currentIndex = args.Count - 1;
        //        else if (currentIndex == args.Count)
        //            currentIndex = 0;
        //    }
        //}

        public static string ShowMenu(string title, List<string> args, string? menuInformation, bool clearConsole = true)
        {
            int currentIndex = 0;
            if (clearConsole)
                Console.Clear();
            // Capture the current cursor top position as the starting line
            int startLine = Console.CursorTop;

            while (true)
            {
                // Set cursor position to the start line for the menu
                Console.SetCursorPosition(0, startLine);

                // Clear the menu area
                for (int i = 0; i < args.Count + (menuInformation is not null ? 3 : 2); i++)
                {
                    Console.WriteLine(new string(' ', Console.WindowWidth));
                }

                // Reset the cursor position to start drawing the menu
                Console.SetCursorPosition(0, startLine);

                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine(title);
                Console.WriteLine();
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
    }
}

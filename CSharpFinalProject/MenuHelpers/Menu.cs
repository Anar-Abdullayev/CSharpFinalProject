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
        public static string ShowMenu(string title, List<string> args, string? menuInformation)
        {
            int currentIndex = 0;
            while (true)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
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
                ConsoleKeyInfo key = Console.ReadKey();
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

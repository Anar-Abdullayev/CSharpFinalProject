using CSharpFinalProject.Controllers;
using CSharpFinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFinalProject.MenuHelpers
{
    internal static class AdminMenu
    {
        public static void StartLogin()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine();
            usrName:
            Console.Write("Username: ");
            string? username = Console.ReadLine();
            if (string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(username))
                goto usrName;
            pswd:
            Console.Write("Password: ");
            string? password = Console.ReadLine();
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
                goto pswd;

            AdministratorController.CurrentUser = LoginController.Login(username, password);
            if (AdministratorController.CurrentUser is null)
            {
                Console.WriteLine("Username or password is incorrect! Press any key to continue...");
                Console.ReadKey();
                return;
            }

            while (true)
            {

            }
        }

        private static void StartMainMenu()
        {

        }
    }
}

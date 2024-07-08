using CSharpFinalProject.Controllers;
using CSharpFinalProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFinalProject.MenuHelpers
{
    internal static class AdminMenu
    {
        public static string Title;
        public static void StartLogin()
        {
            Title = ConfigurationManager.AppSettings["titlePrototype"]!.Replace("REPLACED", "Administrator Section");
            Menu.PrintTitle(Title);
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

            StartMainMenu();
        }

        private static void StartMainMenu()
        {
            while (true)
            {
                string choice = Menu.ShowMenu(Title, new List<string>() { "Show stock", "Add new category", "Show sell history", "Back" }, null);

                switch (choice)
                {
                    case "Show stock":
                        StartShowCategories();
                        break;
                    case "Show sell history":
                        break;
                    case "Back":
                        return;
                }
            }
        }

        private static void StartShowCategories()
        {
            while (true)
            {
                List<string> categories = Database.Categories!.Select(x => x.Name).ToList();
                categories.Add("Add new category");
                categories.Add("Delete category");
                categories.Add("Back");

                string choice = Menu.ShowMenu(Title, categories, "Select category to see sub-products. Alternatively you can add new category.");

                switch (choice)
                {
                    case "Add new category":
                        StartAddNewCategory();
                        break;
                    case "Back":
                        return;
                    default:
                        StartShowProducts(choice);
                        break;
                }
            }
        }

        private static void StartAddNewCategory()
        {
            Menu.PrintTitle(Title);
        retryCategoryName:
            Console.Write("Enter new category name: ");
            string? categoryName = Console.ReadLine();
            if (categoryName is null)
                goto retryCategoryName;

            try
            {
                if (AdministratorController.AddCategory(categoryName))
                    Console.Write("New category added successfully! ");
                else
                    Console.Write("Something has gone wrong!");

                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message + " Press any key to continue...");
                Console.ReadKey();
                return;
            }
        }

        private static void StartShowProducts(string categoryName)
        {
            Category category = Database.Categories!.First(x => x.Name == categoryName);
            while (true)
            {
                List<string> productList = category.Products is not null ? category.Products.Select(x=>x.Name).ToList() : new List<string>();
                productList.Add("Add new product");
                productList.Add("Back");

                string choice = Menu.ShowMenu(Title, productList, "You can select a product to show menu for product, add a new product or delete existing product");

                switch (choice)
                {
                    case "Back":
                        return;
                    case "Add new product":
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

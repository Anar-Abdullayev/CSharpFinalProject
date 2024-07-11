using CSharpFinalProject.Controllers;
using CSharpFinalProject.Extention_Methods;
using CSharpFinalProject.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CSharpFinalProject.MenuHelpers
{
    internal static class UserMenu
    {
        private static string Title;
        public static void StartLogin()
        {
            Menu.PrintTitle(ConfigurationManager.AppSettings["marketName"]);
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

            UserController.CurrentUser = LoginController.Login(username, password);
            if (UserController.CurrentUser is null)
            {
                Console.WriteLine("Username or password is incorrect! Press any key to continue...");
                Console.ReadKey();
                return;
            }
            Title = UserController.CurrentUser.Name + " " + UserController.CurrentUser.Surname;

            StartMainMenu();
        }

        private static void StartMainMenu()
        {
            while (true)
            {
                string choice = Menu.ShowMenu(Title, new List<string>()
                {
                    "Start shopping",
                    "Show my basket",
                    "My profile",
                    "Show my purchase history",
                    "Log out"
                });

                switch (choice)
                {
                    case "Start shopping":
                        StartShopping();
                        break;
                    case "Show my basket":
                        StartShowMyBasket();
                        break;
                    case "My profile":
                        StartMyProfile();
                        break;
                    case "Show my purchase history":
                        StartMyHistory();
                        break;
                    case "Log out":
                        return;
                }
            }
        }

        private static void StartShopping()
        {
            var categories = Database.Categories.Select(x => x.Name).ToList();
            categories.Add("Back");
            while (true)
            {
                string categoryChoice = Menu.ShowMenu(Title, categories);

                if (categoryChoice == "Back")
                    return;


                void StartProducts()
                {
                    var category = Database.Categories.First(x => x.Name == categoryChoice);
                    var productList = category.Products!.Where(x => x.StockAmount != 0).Select(x => x.Name).ToList();
                    productList.Add("Back");


                    while (true)
                    {
                        var productChoice = Menu.ShowMenu(Title, productList);

                        if (productChoice == "Back")
                            break;

                        var product = category.Products!.First(x => x.Name == productChoice);

                        Console.Clear();
                        Menu.PrintTitle(Title);
                        product.PrintProductInfo();

                        string result = Menu.ShowMenu("", new List<string>() { "Buy", "Back" }, clearConsole: false);

                        if (result == "Back")
                            continue;

                        retryQuantitySelector:
                        double productQuantity = Menu.ShowQuantitySelector();
                        if (product.Measurement == Enums.Measurement.Quantify && productQuantity % 1 != 0)
                        {
                            Console.WriteLine("Quantity must be whole number for " + Enums.Measurement.Quantify.ToString() + " measurements.");
                            goto retryQuantitySelector;
                        }

                        if (productQuantity == 0)
                            continue;

                        try
                        {
                            UserController.AddProductToBasket(product, productQuantity);
                            Console.WriteLine("Added to basket successfully! Press any key to continue...");
                        }
                        catch (ArgumentOutOfRangeException ex)
                        {
                            Console.WriteLine(ex.Message + " Press any key to continue...");
                        }
                        Console.ReadKey();
                    }
                }
                StartProducts();
            }

        }
        private static void StartShowMyBasket()
        {
            if (UserController.CurrentUser!.Basket is null || UserController.CurrentUser.Basket.Count == 0)
            {
                Console.WriteLine("Your basket is empty!");
                Thread.Sleep(2000);
                return;
            }
            Console.Clear();
            Menu.PrintTitle(Title);
            for (int i = 0; i < UserController.CurrentUser!.Basket?.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {UserController.CurrentUser!.Basket![i].Name} - {UserController.CurrentUser!.Basket![i].StockAmount} - {UserController.CurrentUser!.Basket![i].Price} AZN - Total Price: {Math.Round(UserController.CurrentUser!.Basket![i].Price * UserController.CurrentUser!.Basket![i].StockAmount, 2)}");
            }
            Console.WriteLine("Total Price: " + UserController.CurrentUser.TotalBasketCost);
            Console.WriteLine();
            string selector = Menu.ShowMenu(null, new List<string>() { "Delete product from Basket", "Purchase all", "Back" }, clearConsole: false);

            switch (selector)
            {
                case "Delete product from Basket":
                    var myBasket = UserController.CurrentUser.Basket!.Select(x => x.Name).ToList();
                    myBasket.Add("Cancel");
                    string choice = Menu.ShowMenu(Title, myBasket, "Select product you want to delete from basket.", true);
                    if (choice == "Cancel")
                        break;
                    var deletedProduct = UserController.CurrentUser.Basket!.First(x => x.Name == choice);
                    if (UserController.DeleteProductFromBasket(deletedProduct))
                        Console.WriteLine("Product has been deleted from basket successfully!");
                    else
                        Console.WriteLine("Product either not found or an exception occured! Failed to remove!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    break;
                case "Purchase all":
                    Console.Clear();
                    Menu.PrintTitle(Title);
                    Console.WriteLine("Total cost is: " + UserController.CurrentUser.TotalBasketCost + " AZN");
                    retryPayment:
                    Console.Write("Enter money: ");
                    double paid;
                    if (double.TryParse(Console.ReadLine()?.Replace(".", ","), out paid))
                    {
                        if (paid < UserController.CurrentUser.TotalBasketCost)
                        {
                            Console.WriteLine("Not enough balance to pay for your basket! Try again later... Press any key to continue...");
                            Console.ReadKey();
                            break;
                        }
                        try
                        {
                            if (UserController.ConfirmBasket())
                                Console.WriteLine($"Purchase confirmed! Change: {Math.Round(paid-UserController.CurrentUser.TotalBasketCost,2)} Press any key to continue...");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Wrong input!");
                        goto retryPayment;
                    }
                    break;
                case "Back":
                    return;
            }
        }

        private static void StartMyProfile()
        {
            throw new NotImplementedException();
        }
        private static void StartMyHistory()
        {
            Console.Clear();
            Menu.PrintTitle(Title);

            List<SellHistory> myHistory = Database.SellHistories.Where(x => x.UserID == UserController.CurrentUser!.ID).ToList();
            foreach (var history in myHistory)
            {

            }
        }

    }
}

using CSharpFinalProject.Controllers;
using CSharpFinalProject.Extention_Methods;
using CSharpFinalProject.Models;
using System.Configuration;

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
            while (true)
            {
                Console.Clear();
                Menu.PrintTitle(Title);
                for (int i = 0; i < UserController.CurrentUser!.Basket?.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {UserController.CurrentUser!.Basket![i].Name} - {UserController.CurrentUser!.Basket![i].StockAmount} - {UserController.CurrentUser!.Basket![i].Price} AZN - Total Price: {Math.Round(UserController.CurrentUser!.Basket![i].Price * UserController.CurrentUser!.Basket![i].StockAmount, 2)}");
                }
                Console.WriteLine("Total Price: " + UserController.CurrentUser.TotalBasketCost);
                Console.WriteLine();
                string selector = Menu.ShowMenu(null, new List<string>() { "Delete product from Basket", "Clear all", "Purchase all", "Back" }, clearConsole: false);

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
                    case "Clear all":
                        Console.WriteLine("Are you sure you want to clear your basket? (Yes/No)");
                        string yesNo = Console.ReadLine();
                        if (yesNo == "Yes")
                        {
                            UserController.CurrentUser.Basket!.Clear();
                            return;
                        }
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
                                    Console.WriteLine($"Purchase confirmed! Change: {Math.Round(paid - UserController.CurrentUser.TotalBasketCost, 2)} Press any key to continue...");
                                Console.ReadKey();
                                return;
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
        }


        private static void StartMyProfile()
        {
            while (true)
            {
                startFromBeginning:
                Console.Clear();
                Menu.PrintTitle(Title);
                User myUser = UserController.CurrentUser!;
                myUser.PrintUserInfo();
                Console.WriteLine();
                string choice = Menu.ShowMenu(null, new List<string>() { "Change my name", "Change my surname", "Set new password", "Save changes", "Back" }, clearConsole: false);

                switch (choice)
                {
                    case "Change my name":
                        Console.Write("New name: ");
                        string name = Console.ReadLine();
                        myUser.Name = name;
                        goto startFromBeginning;
                    case "Change my surname":
                        Console.Write("New surname: ");
                        string surname = Console.ReadLine();
                        myUser.Surname = surname;
                        goto startFromBeginning;
                    case "Set new password":
                        Console.Write("Current password: ");
                        string curPass = Console.ReadLine();
                        if (curPass != myUser.Password)
                        {
                            Console.WriteLine("Wrong password!");
                            break;
                        }
                        Console.Write("New password: ");
                        string newPass1 = Console.ReadLine();
                        Console.Write("Repeat new password: ");
                        string newPass2 = Console.ReadLine();
                        if (newPass1 != newPass2)
                        {
                            Console.WriteLine("Passwords are different!");
                            break;
                        }
                        if (!LoginController.PasswordValidation(newPass1))
                        {
                            Console.WriteLine("Password must contain at least one upper case, one numeric, and the length must be higher than 8 symbols");
                            break;
                        }
                        myUser.Password = newPass1;
                        Console.WriteLine("Changed successfully!");
                        break;
                    case "Save changes":
                        Database.SaveJson(Database.Users, ConfigurationManager.AppSettings["dbUsersPath"]!);
                        Console.WriteLine("Saved successfully!");
                        break;
                    case "Back":
                        return;
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
        }
        private static void StartMyHistory()
        {
            Console.Clear();
            Menu.PrintTitle(Title);

            List<SellHistory> myHistory = Database.SellHistories.Where(x => x.UserID == UserController.CurrentUser!.ID).ToList();
            foreach (var history in myHistory)
            {
                Console.WriteLine($"Time: {history.PurchaseTime.ToString("dd.MM.yyyy HH:mm")}\nProducts:");
                foreach (var item in history.ProductList!)
                    Console.WriteLine(item);
                Console.WriteLine();
            }
            Console.WriteLine("Press any key to continue");
            Console.ReadKey();
        }

        public static void Register()
        {
            Console.Clear();
            Menu.PrintTitle("Registration");

            Console.Write("Username: ");
            string username = Console.ReadLine()!;
            Console.Write("Password: ");
            string password = Console.ReadLine()!;
            Console.Write("Name: ");
            string name = Console.ReadLine()!;
            Console.Write("Surname: ");
            string surname = Console.ReadLine()!;
            User user = new User() { Username = username, Name = name, Surname = surname, Password = password };
            string registerResultMessage;
            try
            {
                LoginController.Register(user, out registerResultMessage);
                Console.WriteLine(registerResultMessage);
                Console.WriteLine("Press any key to continue...");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadKey();
        }
    }
}

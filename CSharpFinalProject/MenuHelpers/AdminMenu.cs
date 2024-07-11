using CSharpFinalProject.Controllers;
using CSharpFinalProject.Enums;
using CSharpFinalProject.Extention_Methods;
using CSharpFinalProject.Models;
using System.Configuration;

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

            if (!AdministratorController.CurrentUser.IsAdmin)
            {
                Console.WriteLine("You have no administrative privileges! Press any key to continue...");
                Console.ReadKey();
                return;
            }

            StartMainMenu();
        }

        private static void StartMainMenu()
        {
            while (true)
            {
                string choice = Menu.ShowMenu(Title, new List<string>() { "Show stock", "Show sell history", "Save changes", "Log out" }, null);

                switch (choice)
                {
                    case "Show stock":
                        StartShowCategories();
                        break;
                    case "Show sell history":
                        break;
                    case "Save changes":
                        Database.SaveJson(Database.Categories, ConfigurationManager.AppSettings["dbCategoryPath"]);
                        Console.Clear();
                        Console.WriteLine("Changes has been saved! Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case "Log out":
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
                List<string> productList = category.Products is not null ? category.Products.Select(x => x.Name).ToList() : new List<string>();
                productList.Add("Add new product");
                productList.Add("Delete current category");
                productList.Add("Change name of category");
                productList.Add("Back");

                string choice = Menu.ShowMenu(Title, productList, "You can select a product to show menu for product, add a new product or delete existing product");

                switch (choice)
                {
                    case "Add new product":
                        StartAddNewProduct(category);
                        break;
                    case "Delete current category":
                        Console.Clear();
                        Console.WriteLine($"Do you really want to delete entire category? If yes confirm the category name ({category.Name}): ");
                        string? catNameToBeDeleted = Console.ReadLine();
                        Console.Clear();
                        if (category.Name == catNameToBeDeleted)
                        {
                            if (AdministratorController.DeleteCategory(catNameToBeDeleted))
                                Console.WriteLine("Category has been deleted successfully! Press any key to continue;");
                            Console.ReadKey();
                            return;
                        }
                        Console.WriteLine("Deletion of category has been terminated! Try again later...");
                        Console.ReadKey();
                        break;
                    case "Change name of category":
                        Console.Write("Enter new name: ");
                        string newName = Console.ReadLine();
                        if (string.IsNullOrEmpty(newName) || string.IsNullOrWhiteSpace(newName))
                        {
                            Console.WriteLine("Name can't be empty! Press any key to continue...");
                        }
                        else
                        {
                            var newCategory = Database.Categories.FirstOrDefault(x => x.Name == newName);
                            if (newCategory is not null)
                                Console.WriteLine("Category name already exists! Press any key to continue...");
                            else
                            {
                                category.Name = newName;
                                Console.WriteLine("Category name has been changed successfully! Press any key to continue...");
                            }
                        }
                        Console.ReadKey();
                        break;
                    case "Back":
                        return;
                    default:
                        Product product = category.Products!.FirstOrDefault(x => x.Name == choice)!;
                        StartProductInfo(category, product);
                        break;
                }
            }
        }

        private static void StartAddNewProduct(Category category)
        {
            Console.Clear();
            Menu.PrintTitle(Title);

            retryName:
            Console.Write("Enter product name: ");
            string? productName = Console.ReadLine();
            if (string.IsNullOrEmpty(productName) || string.IsNullOrWhiteSpace(productName))
            {
                Console.WriteLine("Product name can't be empty!");
                goto retryName;
            }

            retryPrice:
            Console.Write("Enter price: ");
            string? productPrice = Console.ReadLine();
            double prPrice;
            if (!double.TryParse(productPrice?.Replace(".", ","), out prPrice))
            {
                Console.WriteLine("Product price can't be empty!");
                goto retryPrice;
            }

            string measurementChoice = Menu.ShowMenu("", new List<string>() { Measurement.Quantify.ToString(), Measurement.Mass.ToString() }, "Measurement type:", false);

            retryStock:
            Console.Write("Enter stock quantity: ");
            string? productStock = Console.ReadLine();
            double prStock;
            if (!double.TryParse(productStock?.Replace(".", ","), out prStock))
            {
                Console.WriteLine("Product stock can't be empty!");
                goto retryStock;
            }
            else
            {
                if (measurementChoice == Measurement.Quantify.ToString())
                {
                    if (prStock % 1 != 0)
                    {
                        Console.WriteLine("Product stock quantity must be whole number!");
                        goto retryStock;
                    }
                }
            }
            Measurement measurement = Measurement.Quantify.ToString() == measurementChoice ? Measurement.Quantify : Measurement.Mass;
            try
            {
                AdministratorController.AddProduct(category, productName, prPrice, prStock, measurement);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Console.WriteLine(" Press any key to continue...");
                Console.ReadKey();
            }
        }

        private static void StartProductInfo(Category currentCategory, Product product)
        {
            while (true)
            {
                Console.Clear();
                Menu.PrintTitle(Title);

                product.PrintProductInfo(true);
                string choice = Menu.ShowMenu("", new List<string>() { "Change name", "Set price", "Set stock amount", "Set measurement", "Delete current product", "Back" }, null, false);

                switch (choice)
                {
                    case "Change name":
                        Console.Write("Enter new name: ");
                        string? productName = Console.ReadLine();
                        if (string.IsNullOrEmpty(productName) || string.IsNullOrWhiteSpace(productName))
                        {
                            Console.WriteLine("Product name can't be empty. Press any key to continue...");
                        }
                        else
                        {
                            var productState = currentCategory.Products!.FirstOrDefault(p => p.Name == productName);
                            if (productState is not null)
                                Console.WriteLine("Product name already exists! Press any key to continue...");
                            else
                            {
                                product.Name = productName;
                                Console.WriteLine("Product name has been changed. Press any key to continue...");
                            }
                        }
                        Console.ReadKey();
                        break;
                    case "Set price":
                        Console.Write("New price: ");
                        double newPrice;
                        if (double.TryParse(Console.ReadLine()?.Replace(".", ","), out newPrice))
                        {
                            try
                            {
                                product.Price = newPrice;
                                Console.Write("Price has been changed successfully!");
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }
                        else
                        {
                            Console.Write("Wrong input!");
                        }
                        Console.WriteLine(" Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case "Set stock amount":
                        Console.Write("New stock: ");
                        double newStock;
                        if (double.TryParse(Console.ReadLine()?.Replace(".", ","), out newStock))
                        {
                            try
                            {
                                product.StockAmount = newStock;
                                Console.Write("Stock has been changed successfully!");
                            }
                            catch (Exception ex)
                            {
                                Console.Write(ex.Message);
                            }
                        }
                        else
                        {
                            Console.Write("Wrong input!");
                        }
                        Console.WriteLine(" Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case "Set measurement":
                        string measurementChoice = Menu.ShowMenu("", new List<string>() { Measurement.Quantify.ToString(), Measurement.Mass.ToString() }, "Measurement type:", false);
                        product.Measurement = measurementChoice == Measurement.Quantify.ToString() ? Measurement.Quantify : Measurement.Mass;
                        Console.WriteLine("Measurement has been changed successfully! Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case "Delete current product":
                        Console.Write($"Do you want to delete {product.Name} fully? To confirm write its name ({product.Name}): ");
                        string confirm = Console.ReadLine();
                        if (confirm == product.Name)
                        {
                            AdministratorController.DeleteProduct(currentCategory, confirm);
                            Console.WriteLine("Product has been deleted successfully! Press any key to continue...");
                            Console.ReadKey();
                            return;
                        }
                        Console.WriteLine("Product deletion has been terminated! Try again later. Press any key to continue...");
                        Console.ReadKey();
                        break;
                    case "Back":
                        return;
                }
            }
        }

    }
}

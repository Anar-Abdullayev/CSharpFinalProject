using CSharpFinalProject.Data;
using CSharpFinalProject.MenuHelpers;
using CSharpFinalProject.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace CSharpFinalProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Add comment the following 3 lines if you have run the project at least one time.
            // Following 3 lines creates dump data.
            // Admin username is: StepIT and password is: StepIT!123
            //Database.Context.Database.EnsureDeleted();
            //Database.Context.Database.EnsureCreated();
            //CreateData();

            Database.Categories = Database.Context.Categories.Include(c => c.Products).ToList();
            Database.Users = Database.Context.Users.ToList();
            Database.SellHistories = Database.Context.SellHistories.ToList();

            string? marketName = ConfigurationManager.AppSettings["marketName"];

            Market myMarket = new Market(marketName!, Database.Context.SellHistories.OrderByDescending(sh => sh.HistoryId).Select(sh => sh.CurrentCashier).FirstOrDefault());

            while (true)
            {
                string choice = Menu.ShowMenu(myMarket.Name, new List<string>() { "Login as Administrator", "Login as User", "Register as User", "Exit" }, "Press enter when you want to choose selection.");
                switch (choice)
                {
                    case "Login as Administrator":
                        AdminMenu.StartLogin();
                        break;
                    case "Login as User":
                        UserMenu.StartLogin();
                        break;
                    case "Register as User":
                        UserMenu.Register();
                        break;
                    case "Exit":
                        Database.Context.SaveChanges();
                        return;
                }
            }
        }

        private static void CreateData()
        {
            List<User> users = new List<User>
            {
                new User { Name = "Anar", Surname = "Abdullayev", Username = "Anar98", Password = "Anar!123", IsAdmin = false },
                new User { Name = "Step", Surname = "IT", Username = "StepIT", Password = "StepIT!123", IsAdmin = true },
                new User { Name = "Step", Surname = "IT", Username = "s", Password = "s", IsAdmin = true },
            };

            Category cat1 = new Category { Name = "Drinks" };
            Category cat2 = new Category { Name = "Snacks" };
            Category cat3 = new Category { Name = "Personal Care" };
            Category cat4 = new Category { Name = "Electronic Accessories" };
            Category cat5 = new Category { Name = "Toys" };
            List<Category> categories = new List<Category>
            {
                cat1, cat2, cat3, cat4, cat5
            };

            List<Product> products = new List<Product>
            {
                new Product { Name = "Coca Cola 1L", Price = 1.5, Measurement = Enums.Measurement.Quantify,  Category = cat1, SellCount = 0, StockAmount = 15 },
                new Product { Name = "Fanta 1L", Price = 1.5, Measurement = Enums.Measurement.Quantify,  Category = cat1, SellCount = 0, StockAmount = 15 },
                new Product { Name = "Sprite 1L", Price = 1.5, Measurement = Enums.Measurement.Quantify,  Category = cat1, SellCount = 0, StockAmount = 15 },
                new Product { Name = "Snikers", Price = 1.5, Measurement = Enums.Measurement.Quantify,  Category = cat2, SellCount = 0, StockAmount = 15 },
                new Product { Name = "Napkins", Price = 1.5, Measurement = Enums.Measurement.Quantify,  Category = cat3, SellCount = 0, StockAmount = 15 },
                new Product { Name = "Phone keeper", Price = 1.5, Measurement = Enums.Measurement.Quantify,  Category = cat4, SellCount = 0, StockAmount = 15 },
                new Product { Name = "Ninja Turtles", Price = 1.5, Measurement = Enums.Measurement.Quantify,  Category = cat5, SellCount = 0, StockAmount = 15 },
            };

            Database.Context.Users.AddRange(users);
            Database.Context.Categories.AddRange(categories);
            Database.Context.Products.AddRange(products);  
            Database.Context.SaveChanges();
        }
    }

}

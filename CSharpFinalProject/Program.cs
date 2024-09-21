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
            //    Database.Context.Database.EnsureDeleted();
            //    Database.Context.Database.EnsureCreated();
            ;
            Database.Categories = Database.Context.Categories.Include(c => c.Products).ToList();
            Database.Users = Database.Context.Users.ToList();
            Database.SellHistories = Database.Context.SellHistories.ToList();
           
            string? marketName = ConfigurationManager.AppSettings["marketName"];

            Market myMarket = new Market(marketName!, Database.Context.SellHistories.OrderByDescending(sh=>sh.HistoryId).Select(sh=>sh.CurrentCashier).FirstOrDefault());

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
    }
}

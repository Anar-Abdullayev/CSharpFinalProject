using CSharpFinalProject.MenuHelpers;
using CSharpFinalProject.Models;
using System.Configuration;

namespace CSharpFinalProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Database.Categories = Database.ReadJson<Category>(ConfigurationManager.AppSettings["dbCategoryPath"]);
            Database.Users = Database.ReadJson<User>(ConfigurationManager.AppSettings["dbUsersPath"]);
            
            string? marketName = ConfigurationManager.AppSettings["marketName"];
            Market myMarket = new Market(marketName);
            while (true)
            {
                string choice = Menu.ShowMenu(myMarket.Name, new List<string>() { "Login as Administrator", "Login as User", "Register as User", "Exit" }, "Press enter when you want to choose selection.");
                switch (choice)
                {
                    case "Login as Administrator":
                        AdminMenu.StartLogin();
                        break;
                    case "Login as User":
                        break;
                    case "Register as User":
                        break;
                    case "Exit":
                        return;
                }
            }
        }
    }
}

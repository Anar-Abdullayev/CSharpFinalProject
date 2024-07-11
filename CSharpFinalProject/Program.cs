using CSharpFinalProject.MenuHelpers;
using CSharpFinalProject.Models;
using System.Configuration;

namespace CSharpFinalProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
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
                        UserMenu.StartLogin();
                        break;
                    case "Register as User":
                        break;
                    case "Exit":
                        Database.SaveAll();
                        return;
                }
            }
        }
    }
}

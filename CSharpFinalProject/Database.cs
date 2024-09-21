using CSharpFinalProject.Data;
using CSharpFinalProject.Models;
using System.Configuration;
using System.Text.Json;

namespace CSharpFinalProject
{
    internal static class Database
    {
        public static MarketDbContext Context = new MarketDbContext();
        public static List<Category> Categories { get; set; }
        public static List<Product> Products { get; set; }
        public static List<User> Users { get; set; }
        public static List<SellHistory> SellHistories { get; set; }
    }
}

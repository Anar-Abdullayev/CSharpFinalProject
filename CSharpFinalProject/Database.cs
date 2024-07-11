using CSharpFinalProject.Models;
using System.Configuration;
using System.Text.Json;

namespace CSharpFinalProject
{
    internal static class Database
    {
        public static List<Category> Categories { get; set; }
        public static List<User> Users { get; set; }
        public static List<SellHistory> SellHistories { get; set; }

        private static int _userID;
        private static double _totalSellCount;
        static Database()
        {
            Categories = ReadJson<Category>(ConfigurationManager.AppSettings["dbCategoryPath"]) ?? new List<Category>();
            Users = ReadJson<User>(ConfigurationManager.AppSettings["dbUsersPath"]) ?? new List<User>();
            SellHistories = ReadJson<SellHistory>(ConfigurationManager.AppSettings["dbSellHistoryPath"]) ?? new List<SellHistory>();
            _userID = int.Parse(File.ReadAllText(ConfigurationManager.AppSettings["dbCurrentUserId"] ?? "0"));
            _totalSellCount = int.Parse(File.ReadAllText(ConfigurationManager.AppSettings["dbTotalSellCount"] ?? "0"));
        }

        public static bool SaveJson<T>(List<T> list, string path) where T : class
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrWhiteSpace(path))
                return false;

            try
            {
                var json = JsonSerializer.Serialize(list);
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                File.WriteAllText(path, json);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static List<T>? ReadJson<T>(string? path) where T : class
        {
            if (string.IsNullOrEmpty(path))
                return null;
            if (!File.Exists(path))
                return new List<T>();

            List<T>? list;
            var json = File.ReadAllText(path);

            try
            {
                list = JsonSerializer.Deserialize<List<T>>(json);
                return list ?? new List<T>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static void SaveAll()
        {
            SaveJson(Categories, ConfigurationManager.AppSettings["dbCategoryPath"]!);
            SaveJson(Users, ConfigurationManager.AppSettings["dbUsersPath"]!);
            SaveJson(SellHistories, ConfigurationManager.AppSettings["dbSellHistoryPath"]!);
            File.WriteAllText(ConfigurationManager.AppSettings["dbTotalSellCount"]!, Math.Round(_totalSellCount,2).ToString());
            File.WriteAllText(ConfigurationManager.AppSettings["dbCurrentUserId"]!, _userID.ToString());
        }

        public static int GetNextUserID()
        {
            return _userID++;
        }

        public static void IncreaseTotalCount(double count)
        {
            _totalSellCount += count;
        }
    }
}

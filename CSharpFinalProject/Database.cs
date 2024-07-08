using CSharpFinalProject.Models;
using System.Configuration;
using System.Text.Json;

namespace CSharpFinalProject
{
    internal static class Database
    {
        public static List<Category> Categories { get; set; }
        public static List<User> Users { get; set; }

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
    }
}

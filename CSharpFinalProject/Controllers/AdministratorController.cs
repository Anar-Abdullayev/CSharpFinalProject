using CSharpFinalProject.Models;
using System.Configuration;

namespace CSharpFinalProject.Controllers
{
    internal static class AdministratorController
    {
        public static User? CurrentUser { get; set; }

        public static bool AddCategory(string newCategoryName)
        {
            if (string.IsNullOrEmpty(newCategoryName) || string.IsNullOrWhiteSpace(newCategoryName)) 
                throw new ArgumentException("The category name can't be blank or white space!");
            if (Database.Categories!.FirstOrDefault(x => x.Name == newCategoryName) is not null)
                throw new ArgumentException("The category already exists!");
            try
            {
                Category newCat = new Category() { Name = newCategoryName, Products = null };
                Database.Categories!.Add(newCat);
                Database.SaveJson(Database.Categories, ConfigurationManager.AppSettings["dbCategoryPath"]);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

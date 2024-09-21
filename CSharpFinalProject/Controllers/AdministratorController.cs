using CSharpFinalProject.Enums;
using CSharpFinalProject.Models;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;

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
                Category newCat = new Category() { Name = newCategoryName };
                Database.Categories!.Add(newCat);
                Database.Context.Categories.Add(newCat);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public static bool DeleteCategory(string categoryName)
        {
            var category = Database.Categories.FirstOrDefault(x => x.Name == categoryName);
            if (category is not null)
            {
                Database.Categories.Remove(category);
                Database.Context.Categories.Remove(category);
            }
            else
                return false;
            return true;
        }
        public static bool AddProduct(Category category, string name, double price, double stockAmount, Measurement measurement)
        {
            try
            {
                Product newProduct = new Product() { Name = name, Price = price, StockAmount = stockAmount, Measurement = measurement, Category = category };

                if (category.Products is null)
                    category.Products = new List<Product>();

                if (category.Products.FirstOrDefault(x => x.Name == name) is not null)
                    throw new ArgumentException($"The product [ {name} ] already exists!");

                Database.Context.Products.Add(newProduct);
                return true;
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }
        }
        public static bool DeleteProduct(Category fromCategory, string productName)
        {
            try
            {
                Product? productToBeDeleted = fromCategory.Products?.FirstOrDefault(x => x.Name == productName);
                fromCategory.Products?.Remove(productToBeDeleted!);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

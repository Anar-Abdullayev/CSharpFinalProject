using CSharpFinalProject.Models;

namespace CSharpFinalProject.Extention_Methods
{
    public static class Extentions
    {
        internal static void PrintProductInfo(this Product product, bool isFull = false)
        {
            Console.WriteLine("Product name: " + product.Name);
            Console.WriteLine("Price: " + product.Price);
            if (!isFull)
                return;
            Console.WriteLine("Measurement: " + product.Measurement.ToString());
            Console.WriteLine("In stock: " + product.StockAmount);
        }

        internal static void PrintUserInfo(this User user)
        {
            Console.WriteLine("ID: " + user.UserId);
            Console.WriteLine("Username: " + user.Username);
            Console.WriteLine("Name: " + user.Name);
            Console.WriteLine("Surname: " + user.Surname);   
        }

    }
}

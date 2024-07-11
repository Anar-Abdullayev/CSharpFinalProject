using CSharpFinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}

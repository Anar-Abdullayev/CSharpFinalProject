using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpFinalProject.Models
{
    internal class Product
    {
        private double _price;
        private double _stockAmount;
        public string Name { get; set; }
        public double Price { get => _price; set { if (value < 0) throw new ArgumentException("Price can't be lower than 0."); _price = value; } }
        public double StockAmount { get => _stockAmount; set { if (value < 0) throw new ArgumentException("Stock can't be lower than 0."); _stockAmount = value; } }
    }
}

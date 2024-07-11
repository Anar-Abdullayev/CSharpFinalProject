using CSharpFinalProject.Enums;

namespace CSharpFinalProject.Models
{
    internal class Product : ICloneable
    {
        private double _price;
        private double _stockAmount;
        public string Name { get; set; }
        public double Price { get => _price; set { if (value < 0) throw new ArgumentException("Price can't be lower than 0."); _price = value; } }
        public double StockAmount { get => _stockAmount; set { if (value < 0) throw new ArgumentException("Stock can't be lower than 0."); _stockAmount = value; } }
        public Measurement Measurement { get; set; }
        public double SellCount { get; set; }

        public object Clone()
        {
            Product product = new Product();
            product.Name = Name;
            product.Price = Price;
            product.StockAmount = StockAmount;
            product.Measurement = Measurement;
            return product;
        }
    }
}

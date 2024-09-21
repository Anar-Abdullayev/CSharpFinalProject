using CSharpFinalProject.Enums;

namespace CSharpFinalProject.Models
{
    internal class Product : ICloneable
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double StockAmount { get; set; }
        public Measurement Measurement { get; set; }
        public double SellCount { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<SellHistory> SellHistory { get; set; }

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

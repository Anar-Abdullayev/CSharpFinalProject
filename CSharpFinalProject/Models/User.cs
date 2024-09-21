using System.Text.Json.Serialization;

namespace CSharpFinalProject.Models
{
    internal class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public ICollection<SellHistory> BuyHistory { get; set; }


        public List<Product>? Basket { get; set; }
        public double TotalBasketCost { get; set; } = 0;
    }
}

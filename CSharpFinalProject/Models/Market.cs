using System.Configuration;

namespace CSharpFinalProject.Models
{
    internal class Market
    {
        public string Name { get; set; }
        public decimal Cashier { get; set; } = 0;

        public Market(string marketName, decimal? currentCashier)
        {

            this.Name = marketName;
            this.Cashier = currentCashier ?? 0;
        }
    }
}

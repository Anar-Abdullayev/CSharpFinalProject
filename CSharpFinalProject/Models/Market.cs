using System.Configuration;

namespace CSharpFinalProject.Models
{
    internal class Market
    {
        public string Name { get; set; }

        public Market(string marketName, decimal? currentCashier)
        {

            this.Name = marketName;
            Cashier = currentCashier ?? 0;
        }

        public static decimal Cashier { get; set; } = 0;
    }
}

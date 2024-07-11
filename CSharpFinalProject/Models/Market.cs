using System.Configuration;

namespace CSharpFinalProject.Models
{
    internal class Market
    {
        public string Name { get; set; }
        public double Cashier { get; set; } = 0;

        public Market(string? name)
        {
            Name = name ?? "Nameless";
            double cash;
            if (File.Exists(ConfigurationManager.AppSettings["dbCashierPath"]))
                double.TryParse(File.ReadAllText(ConfigurationManager.AppSettings["dbCashierPath"]!), out cash);
            else
                cash = 0;
            Cashier = cash;
        }
    }
}

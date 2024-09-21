namespace CSharpFinalProject.Models
{
    internal class SellHistory
    {
        public int HistoryId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public double ProductAmount { get; set; }
        public decimal CurrentCashier { get; set; }
        public DateTime PurchaseTime { get; set; }


        public Product Product { get; set; }
        public User User { get; set; }
    }
}

namespace CSharpFinalProject.Models
{
    internal class SellHistory
    {
        public required int UserID { get; init; }
        public required string Username { get; init; }
        public DateTime PurchaseTime { get; init; }
        public List<string>? ProductList { get; set; }
        public double TotalAmount { get; init; }
    }
}

namespace CSharpFinalProject.Models
{
    internal class PurchaseHistory
    {
        public required int userID { get; set; }
        public required DateTime PurchaseTime { get; set; }
        public required List<string> PurchaseBasket { get; set; }
        public required double PurchaseCost { get; set; }
    }
}

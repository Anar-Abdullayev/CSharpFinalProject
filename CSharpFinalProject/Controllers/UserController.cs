using CSharpFinalProject.Models;

namespace CSharpFinalProject.Controllers
{
    internal static class UserController
    {
        public static User? CurrentUser { get; set; }

        public static bool AddProductToBasket(Product realProduct, double quantity)
        {

            Product? productForBasket = (realProduct.Clone() as Product);
            productForBasket.ProductId = realProduct.ProductId;
            if (productForBasket is null)
                throw new Exception("Null product can't be added to basket!");

            if (CurrentUser!.Basket is null)
                CurrentUser.Basket = new List<Product>();

            var existingProduct = CurrentUser.Basket.FirstOrDefault(x => x.Name == productForBasket.Name);

            if (existingProduct is null)
                productForBasket.StockAmount = quantity;
            else
            {
                productForBasket = existingProduct;
                productForBasket.StockAmount += quantity;
            }

            if (realProduct.StockAmount < productForBasket.StockAmount)
            {
                throw new ArgumentOutOfRangeException("There is/are not enough amount in stock for this product!");
            }

            if (existingProduct is null)
                CurrentUser.Basket.Add(productForBasket);
            CurrentUser.TotalBasketCost += (productForBasket.Price * quantity);
            CurrentUser.TotalBasketCost = Math.Round(CurrentUser.TotalBasketCost, 2);
            return true;
        }

        public static bool DeleteProductFromBasket(Product pr)
        {
            if (CurrentUser!.Basket is null)
                throw new Exception("Basket is empty!");
            var result = CurrentUser.Basket.Remove(pr);
            if (result == true)
                CurrentUser.TotalBasketCost -= pr.Price * pr.StockAmount;
            CurrentUser.TotalBasketCost = Math.Round(CurrentUser.TotalBasketCost, 2);
            return result;
        }

        public static bool ConfirmBasket()
        {
            if (CurrentUser is null)
                throw new ArgumentNullException(nameof(CurrentUser) + " can't be null");
            List<SellHistory> sellHistories = new List<SellHistory>(CurrentUser.Basket!.Count);

            foreach (var item in CurrentUser.Basket!)
            {
                Product foundProduct = Database.Context.Products.FirstOrDefault(x => x.ProductId == item.ProductId);
                if (foundProduct is null)
                    throw new NullReferenceException();

                foundProduct.SellCount += item.StockAmount;
                foundProduct.StockAmount -= item.StockAmount;
                Database.Context.Products.Update(foundProduct);

                SellHistory history = new SellHistory() { UserId = CurrentUser.UserId, ProductAmount = item.StockAmount, CurrentCashier = Market.Cashier + (decimal)(item.Price * item.StockAmount), PurchaseTime = DateTime.Now, ProductId = item.ProductId };
                sellHistories.Add(history);
            }

            CurrentUser.Basket.Clear();
            Database.Context.SellHistories.AddRange(sellHistories);

            Database.Context.SaveChanges();
            return true;
        }

    }
}

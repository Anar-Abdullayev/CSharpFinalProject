using CSharpFinalProject.Models;

namespace CSharpFinalProject.Controllers
{
    internal static class UserController
    {
        public static User? CurrentUser { get; set; }

        public static bool AddProductToBasket(Product realProduct, double quantity)
        {

            Product? productForBasket = (realProduct.Clone() as Product);

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
                productForBasket.StockAmount -= quantity;
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

            SellHistory history = new SellHistory() { UserID = CurrentUser!.ID, Username = CurrentUser.Username, PurchaseTime = DateTime.Now, TotalAmount = CurrentUser.TotalBasketCost, ProductList = new List<string>() };

            foreach (var myProduct in CurrentUser.Basket!)
            {
                foreach (var category in Database.Categories)
                {
                    if (category.Products is not null)
                    {
                        var foundProduct = category.Products.FirstOrDefault(x => x.Name == myProduct.Name);
                        if (foundProduct is not null)
                        {
                            Database.IncreaseTotalCount(myProduct.StockAmount);
                            foundProduct.SellCount += myProduct.StockAmount;
                            foundProduct.StockAmount -= myProduct.StockAmount;
                            string productString = myProduct.Name + " - " + myProduct.StockAmount + " quantify/mass - Total " + Math.Round(myProduct.StockAmount * myProduct.Price, 2) + " AZN";
                            history.ProductList.Add(productString);
                            break;
                        }
                    }
                }
            }

            CurrentUser.Basket.Clear();
            Database.SellHistories.Add(history);

            Database.SaveAll();

            return true;
        }
    }
}

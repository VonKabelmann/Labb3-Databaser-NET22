using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Models;

public class Shop
{
    public Shop(string shopName, IEnumerable<ProductContainer> productStock)
    {
        ShopName = shopName;
        ProductStock = productStock;
    }

    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement]
    public string ShopName { get; set; }

    [BsonElement]
    public IEnumerable<ProductContainer> ProductStock { get; set; }

    public void UpsertStockItem(ProductContainer productContainer)
    {
        var newStock = ProductStock.ToList();
        if (newStock.Any(s => s.Product.ProductName
                .Equals(productContainer.Product.ProductName)))
        {
            productContainer.Amount +=
                newStock.First(s => s.Product.ProductName == productContainer.Product.ProductName).Amount;
            newStock.Remove(newStock.First(s => s.Product.ProductName == productContainer.Product.ProductName));
            newStock.Add(productContainer);
        }
        else
        {
            newStock.Add(productContainer);
        }
        ProductStock = newStock;
    }

    public void UpdateOrDeleteStockItem(ProductContainer productContainer)
    {
        var newStock = ProductStock.ToList();
        productContainer.Amount =
            newStock.First(s => s.Product.ProductName == productContainer.Product.ProductName).Amount
            - productContainer.Amount;
        newStock.Remove(newStock.First(s => s.Product.ProductName == productContainer.Product.ProductName));
        if (productContainer.Amount == 0)
        {
            ProductStock = newStock;
            return;
        }
        newStock.Add(productContainer);
        ProductStock = newStock;
    }
}
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DataAccess.Models;

public class User
{
    public User(string userName, string password)
    {
        UserName = userName;
        Password = password;
        ShoppingCart = new List<ProductContainer>();
    }

    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement]
    public string UserName { get; set; }

    [BsonElement]
    public string Password { get; set; }

    [BsonElement]
    public IEnumerable<ProductContainer> ShoppingCart { get; set; }

    public void UpsertStockItem(ProductContainer productContainer)
    {
        var newCart = ShoppingCart.ToList();
        if (newCart.Any(s => s.Product.ProductName
                .Equals(productContainer.Product.ProductName)))
        {
            productContainer.Amount +=
                newCart.First(s => s.Product.ProductName == productContainer.Product.ProductName).Amount;
            newCart.Remove(newCart.First(s => s.Product.ProductName == productContainer.Product.ProductName));
            newCart.Add(productContainer);
        }
        else
        {
            newCart.Add(productContainer);
        }
        ShoppingCart = newCart;
    }

    public void UpdateOrDeleteStockItem(ProductContainer productContainer)
    {
        var newCart = ShoppingCart.ToList();
        productContainer.Amount =
            newCart.First(s => s.Product.ProductName == productContainer.Product.ProductName).Amount
            - productContainer.Amount;
        newCart.Remove(newCart.First(s => s.Product.ProductName == productContainer.Product.ProductName));
        if (productContainer.Amount == 0)
        {
            ShoppingCart = newCart;
            return;
        }
        newCart.Add(productContainer);
        ShoppingCart = newCart;
    }
}
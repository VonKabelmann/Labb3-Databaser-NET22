using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Models;

public class ProductContainer
{
    public ProductContainer(Product product, int amount)
    {
        Product = product;
        Amount = amount;
    }

    [BsonElement]
    public Product Product { get; set; }

    [BsonElement]
    public int Amount { get; set; }
}
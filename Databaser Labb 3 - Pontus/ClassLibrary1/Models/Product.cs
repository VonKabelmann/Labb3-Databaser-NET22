using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace DataAccess.Models;

public class Product
{
    public Product(string productName, double price)
    {
        ProductName = productName;
        Price = price;
    }

    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement]
    public string ProductName { get; set; }

    [BsonElement]
    public double Price { get; set; }
}
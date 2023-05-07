using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataAccess.Models;

public class Order
{
    public Order(ObjectId shopId, ObjectId userId, IEnumerable<ProductContainer> listOfProducts)
    {
        ShopId = shopId;
        UserId = userId;
        ListOfProducts = listOfProducts;
    }

    [BsonId]
    public ObjectId Id { get; set; }

    [BsonElement]
    public ObjectId ShopId { get; set; }

    [BsonElement]
    public ObjectId UserId { get; set; }

    [BsonElement]
    public IEnumerable<ProductContainer> ListOfProducts { get; set; }
}
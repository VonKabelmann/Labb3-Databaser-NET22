using DataAccess.Models;
using MongoDB.Driver;
using System;

namespace DataAccess.Managers;

public class OrderManager : IRepository<Order>
{
    private readonly IMongoCollection<Order> _collection;
    public OrderManager()
    {
        var hostname = "localhost";
        var databaseName = "Db-Labb-3-Pontus";
        var connectionString = $"mongodb://{hostname}:27017";

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<Order>("Orders", new MongoCollectionSettings() { AssignIdOnInsert = true });
    }
    public void Add(Order item)
    {
        _collection.InsertOne(item);
    }

    public IEnumerable<Order> GetAll()
    {
        return _collection.Find(_ => true).ToEnumerable();
    }

    public void Replace(object id, Order item)
    {
        var filter = Builders<Order>.Filter.Eq("Id", id);
        var update = Builders<Order>
            .Update
            .Set("ShopId", item.ShopId)
            .Set("UserId", item.UserId)
            .Set("Order", item.ListOfProducts);

        _collection
            .FindOneAndUpdate(
                filter,
                update,
        new FindOneAndUpdateOptions<Order, Order>()
        {
            IsUpsert = true
        });
    }

    public void Delete(object id)
    {
        var filter = Builders<Order>.Filter.Eq("Id", id);
        _collection.FindOneAndDelete(filter);
    }
}
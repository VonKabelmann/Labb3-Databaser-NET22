using DataAccess.Models;
using MongoDB.Driver;

namespace DataAccess.Managers;

public class ProductManager : IRepository<Product>
{
    private readonly IMongoCollection<Product> _collection;
    public Product? CurrentProduct { get; set; }
    public ProductManager()
    {
        var hostname = "localhost";
        var databaseName = "Db-Labb-3-Pontus";
        var connectionString = $"mongodb://{hostname}:27017";

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<Product>("Products", new MongoCollectionSettings() { AssignIdOnInsert = true });
    }
    public void Add(Product item)
    {
        _collection.InsertOne(item);
    }

    public IEnumerable<Product> GetAll()
    {
        return _collection.Find(_ => true).ToEnumerable();
    }

    public void Replace(object id, Product item)
    {
        var filter = Builders<Product>.Filter.Eq("Id", id);
        var update = Builders<Product>
            .Update
            .Set("Price", item.Price)
            .Set("ProductName", item.ProductName);

        _collection
            .FindOneAndUpdate(
                filter,
                update,
                new FindOneAndUpdateOptions<Product, Product>()
                {
                    IsUpsert = true
                });
    }

    public void Delete(object id)
    {
        var filter = Builders<Product>.Filter.Eq("Id", id);
        _collection.FindOneAndDelete(filter);
    }

    public Product? GetProductByProductName(string productName)
    {
        return _collection.Find(p => p.ProductName == productName).FirstOrDefault();
    }
}
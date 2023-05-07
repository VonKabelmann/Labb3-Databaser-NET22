using DataAccess.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Managers;

public class ShopManager : IRepository<Shop>
{
    private readonly IMongoCollection<Shop> _collection;
    public Shop? CurrentShop { get; set; }

    public ShopManager()
    {
        var hostname = "localhost";
        var databaseName = "Db-Labb-3-Pontus";
        var connectionString = $"mongodb://{hostname}:27017";

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<Shop>("Shops", new MongoCollectionSettings() { AssignIdOnInsert = true });
    }
    public void Add(Shop item)
    {
        _collection.InsertOne(item);
    }

    public IEnumerable<Shop> GetAll()
    {
        return _collection.Find(_ => true).ToEnumerable();
    }

    public void Replace(object id, Shop item)
    {
        var filter = Builders<Shop>.Filter.Eq("Id", id);
        var update = Builders<Shop>
            .Update
            .Set("ShopName", item.ShopName)
            .Set("ProductStock", item.ProductStock);

        _collection
            .FindOneAndUpdate(
                filter,
                update,
                new FindOneAndUpdateOptions<Shop, Shop>()
                {
                    IsUpsert = true
                });
    }

    public void Delete(object id)
    {
        var filter = Builders<Shop>.Filter.Eq("Id", id);
        _collection.FindOneAndDelete(filter);
    }

    public void EditProductInAllShops(Product product)
    {
        var allShops = GetAll();

        foreach (var shop in allShops)
        {
            if (shop.ProductStock.Any(p => p.Product.Id == product.Id))
            {
                var stock = shop.ProductStock.ToList();
                var productContainerToEdit = stock.First(p => p.Product.Id == product.Id);
                stock.Remove(productContainerToEdit);
                productContainerToEdit.Product = product;
                stock.Add(productContainerToEdit);
                shop.ProductStock = stock;
                Replace(shop.Id, shop);
            }
        }
    }

    public Shop? GetShopById(ObjectId id)
    {
        return _collection.Find(s => s.Id == id).ToList().FirstOrDefault();
    }
}
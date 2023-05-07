using DataAccess.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Managers;

public class UserManager : IRepository<User>
{
    private readonly IMongoCollection<User> _collection;

    public User? CurrentUser { get; set; }
    public UserManager()
    {
        var hostname = "localhost";
        var databaseName = "Db-Labb-3-Pontus";
        var connectionString = $"mongodb://{hostname}:27017";

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        _collection = database.GetCollection<User>("Users", new MongoCollectionSettings() { AssignIdOnInsert = true });
    }
    public void Add(User item)
    {
        _collection.InsertOne(item);
    }

    public IEnumerable<User> GetAll()
    {
        return _collection.Find(_ => true).ToEnumerable();
    }

    public void Replace(object id, User item)
    {
        var filter = Builders<User>.Filter.Eq("Id", id);
        var update = Builders<User>
            .Update
            .Set("UserName", item.UserName)
            .Set("Password", item.Password)
            .Set("ShoppingCart", item.ShoppingCart);

        _collection
            .FindOneAndUpdate(
                filter,
                update,
                new FindOneAndUpdateOptions<User, User>()
                {
                    IsUpsert = true
                });
    }

    public void Delete(object id)
    {
        var filter = Builders<User>.Filter.Eq("Id", id);
        _collection.FindOneAndDelete(filter);
    }

    public User? GetUserByUserName(string userName)
    {
        return _collection.Find(u => u.UserName == userName).FirstOrDefault();
    }
    public User? GetUserById(ObjectId id)
    {
        return _collection.Find(s => s.Id == id).ToList().FirstOrDefault();
    }
}
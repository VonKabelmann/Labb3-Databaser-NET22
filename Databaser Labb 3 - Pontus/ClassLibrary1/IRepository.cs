namespace DataAccess;

public interface IRepository<T>
{
    void Add(T item);
    IEnumerable<T> GetAll();
    void Replace(object id, T item);
    void Delete(object id);
}
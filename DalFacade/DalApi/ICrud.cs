using DO;

namespace DalApi
{
    public interface ICrud<T> where T : class
    {
        int Create(T item);   // Creates new entity object in DAL
        void Delete(int id);  // Reads entity object by its ID 
        T? Read(int id);      // Reads all entity objects
        T? Read(Func<T, bool> filter);     // Reads all entity objects that match a given predicate
        IEnumerable<T?> ReadAll(Func<T, bool>? filter = null);    // Updates entity object
        void Update(T item);  // Deletes an object by its Id
    }
}
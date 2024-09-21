namespace DalApi;

/// <summary>
/// creates an interface to all the data operation
/// </summary>
/// <typeparam name="T">the type of the data</typeparam>
/// <typeparam name="K">the type of the key</typeparam>
public interface ICrud<T, K> where T : class
{
    K Create(T item);                                    // Creates new entity object in DAL
    T? Read(K id);                                       // Reads the entity object that matches a given ID
    T? Read(Func<T, bool> filter);                         // Reads the first entity object that matches a given predicate
    void Update(T item);                                   // Updates an object by its Id
    IEnumerable<T> ReadAll(Func<T, bool>? filter = null); // Reads all entity objects that match a given predicate
    void Delete(K id);                                   // Delete entity object by its ID 
    void DeleteAll();                                      // Deletes all entity objects
}
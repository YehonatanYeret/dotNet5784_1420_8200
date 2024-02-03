namespace DalApi;

/// <summary>
/// creates an interface to all the data operation
/// </summary>
/// <typeparam name="T">the type of the data</typeparam>
public interface ICrud<T> where T : class
{
    int Create(T item);                                    // Creates new entity object in DAL
    T? Read(int id);                                       // Reads all entity objects
    T? Read(Func<T, bool> filter);                         // Reads all entity objects that match a given predicate
    void Update(T item);                                   // Deletes an object by its Id
    IEnumerable<T> ReadAll(Func<T, bool>? filter = null); // Updates entity object
    void Delete(int id);                                   // Reads entity object by its ID 
    void DeleteAll();                                      // Deletes all entity objects
}
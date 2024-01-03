namespace DalApi;
using DO;

public interface IMilestone
{
    int Create(Milestone item); //Creates new entity object in DAL
    Milestone? Read(int id); //Reads entity object by its ID 
    List<Milestone> ReadAll(); //stage 1 only, Reads all entity objects
    void Update(Milestone item); //Updates entity object
    void Delete(int id); //Deletes an object by its Id
}

namespace Dal;
using DalApi;
using DO;
using System.Linq;

internal class EngineerImplementation : IEngineer
{
    //create a new engineer
    public int Create(Engineer item)
    {
        Engineer? engineer = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == item.Id);
        if (engineer != null)//if the engineer already exists
            throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");

        DataSource.Engineers.Add(item);//add the task to the list
        return item.Id;//return the id
    }

    //read a engineer and return it. if not found return null
    public Engineer? Read(int id)
    {
        Engineer? engineer = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == id);//find the engineer with the id and if not found return null
        if (engineer == null)//if not found
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");//throw exception
        return engineer;
    }

    public Engineer? Read(Func<Engineer, bool> filter)
    {
        // return the first element that match the filter condition, else return null
        return DataSource.Engineers.FirstOrDefault(filter); 
    }

    //return a copy of the list of engineers
    public IEnumerable<Engineer> ReadAll(Func<Engineer, bool>? filter = null)
    {
        if (filter != null)
        {
            return DataSource.Engineers.Where(filter);
        }
        return DataSource.Engineers.Select(engineer => engineer);
    }

    //update a engineer by removing the old one and adding the new one
    public void Update(Engineer item)
    {
        Engineer? engineer = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == item.Id);//find the index of the engineer with the same id
        if (engineer == null)//if not found
            throw new DalDoesNotExistException($"Engineer with ID={item.Id} does not exist");//throw exception

        DataSource.Engineers.RemoveAll(temp => temp.Id == engineer.Id);//remove the engineer
        DataSource.Engineers.Add(item);//add the new engineer
    }

    //we dont need to check if there is no tasks with the engineer id because we will check it in the logic layer
    public void Delete(int id)
    {
        //we dont need to check if there is no tasks with the engineer id because we will check it in the logic layer

        Engineer? engineer = DataSource.Engineers.FirstOrDefault(engineer => engineer.Id == id);//find the index of the engineer with the same id
        if (engineer == null)//if not found
            throw new DalDoesNotExistException($"Engineer with ID={id} does not exist");//throw exception

        DataSource.Engineers.RemoveAll(temp => temp.Id == id);//remove the engineer
    }

}

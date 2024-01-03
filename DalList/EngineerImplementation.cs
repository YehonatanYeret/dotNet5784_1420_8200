namespace Dal;
using DalApi;
using DO;

public class EngineerImplementation : IEngineer
{
    public int Create(Engineer item)
    {
        Engineer? engineer = DataSource.Engineers.Find(engineer => engineer.Id == item.Id);
        if(engineer != null)//if the engineer already exists
            throw new Exception($"Engineer with ID={item.Id} already exists");

        DataSource.Engineers.Add(item);//add the task to the list
        return item.Id;//return the id
    }

    public Engineer? Read(int id)
    {
        Engineer? engineer = DataSource.Engineers.Find(engineer => engineer.Id == id);//find the engineer with the id and if not found return null
        if (engineer == null)//if not found
            throw new Exception($"Engineer with ID={id} does not exist");//throw exception
        return engineer;
    }

    public List<Engineer> ReadAll()
    {
        return new List<Engineer>(DataSource.Engineers);//return a copy of the list of engineers
    }

    public void Update(Engineer item)
    {
        Engineer? engineer = DataSource.Engineers.Find(engineer => engineer.Id == item.Id);//find the index of the engineer with the same id
        if (engineer == null)//if not found
            throw new Exception($"Engineer with ID={item.Id} does not exist");//throw exception

        DataSource.Engineers.Remove(engineer);//remove the engineer
        DataSource.Engineers.Add(item);//add the new engineer
    }
    public void Delete(int id)
    {
        Task? task = DataSource.Tasks.Find(engineer => engineer.EngineerId == id);//find the index of the task with the same id that the engineer is working on
        if (task != null)//the engineer is working on a task
            throw new Exception($"cannot delete the engineer with the ID={id} because he works on the task with the ID={task.Id}");

        Engineer? engineer = DataSource.Engineers.Find(engineer => engineer.Id == id);//find the index of the engineer with the same id
        if (engineer == null)//if not found
            throw new Exception($"Engineer with ID={id} does not exist");//throw exception

        DataSource.Engineers.Remove(engineer);//remove the engineer
    }
}

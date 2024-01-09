namespace Dal;
using DalApi;
using DO;

internal class EngineerImplementation : IEngineer
{
    //create a new engineer
    public int Create(Engineer item)
    {
        Engineer? engineer = DataSource.Engineers.Find(engineer => engineer.Id == item.Id);
        if(engineer != null)//if the engineer already exists
            throw new Exception($"Engineer with ID={item.Id} already exists");

        DataSource.Engineers.Add(item);//add the task to the list
        return item.Id;//return the id
    }

    //read a engineer and return it. if not found return null
    public Engineer? Read(int id)
    {
        Engineer? engineer = DataSource.Engineers.Find(engineer => engineer.Id == id);//find the engineer with the id and if not found return null
        if (engineer == null)//if not found
            throw new Exception($"Engineer with ID={id} does not exist");//throw exception
        return engineer;
    }

    //return a copy of the list of engineers
    public IEnumerable<Engineer> ReadAll()
    {
        IEnumerable<Engineer> engineers = DataSource.Engineers;
        return from engineer in engineers
               select engineer;
    }

    //update a engineer by removing the old one and adding the new one
    public void Update(Engineer item)
    {
        Engineer? engineer = DataSource.Engineers.Find(engineer => engineer.Id == item.Id);//find the index of the engineer with the same id
        if (engineer == null)//if not found
            throw new Exception($"Engineer with ID={item.Id} does not exist");//throw exception

        DataSource.Engineers.Remove(engineer);//remove the engineer
        DataSource.Engineers.Add(item);//add the new engineer
    }
    
    //we dont need to check if there is no tasks with the engineer id because we will check it in the logic layer
    public void Delete(int id)
    {
        //we dont need to check if there is no tasks with the engineer id because we will check it in the logic layer

        Engineer? engineer = DataSource.Engineers.Find(engineer => engineer.Id == id);//find the index of the engineer with the same id
        if (engineer == null)//if not found
            throw new Exception($"Engineer with ID={id} does not exist");//throw exception

        DataSource.Engineers.Remove(engineer);//remove the engineer
    }
}

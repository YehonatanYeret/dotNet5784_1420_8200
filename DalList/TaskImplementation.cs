namespace Dal;
using DalApi;
using DO;

public class TaskImplementation : ITask
{
    public int Create(Task item)
    {
        int ID = DataSource.Config.NextTaskId;//get the next id
        Task task = item with { Id = ID };//create a new task with the new id
        DataSource.Tasks.Add(task);//add the task to the list
        return ID;//return the id
    }

    public Task? Read(int id)
    {
        return DataSource.Tasks.Find(task => task.Id == id && task.isActive);//find the task with the id and if not found return null
    }

    public List<Task> ReadAll()
    {
        //return the list of tasks but only with the active ones

        List<Task>? tList = new List<Task>();//create a new list
        foreach (Task? item in DataSource.Tasks)//for each task
        {
            if(item.isActive) tList.Add(item);//if the task is active add it to the list
        }
        return  (tList);//return a copy of the list of tasks
    }

    public void Update(Task item)
    {
        Task? task =  DataSource.Tasks.Find(task => task.Id == item.Id && task.isActive);//find the index of the task with the same id
        if (task==null)//if not found
            throw new Exception($"Task with ID={item.Id} does not exist");//throw exception

        //DataSource.Tasks[task.Id] = item;//update the task
        //can work but not how we have been asked for

        DataSource.Tasks.Remove(task);//remove the task
        DataSource.Tasks.Add(item);//add the new task
    }

    public void Delete(int id)
    {
        Task? task = DataSource.Tasks.Find(task => task.Id == id && task.isActive);//find the index of the task with the same id
        if(task == null)//if not found
            throw new Exception($"Task with ID={id} does not exist");//throw exception

        foreach(var i in DataSource.Dependencies)//for each dependency
        {
            if (i.DependentOnTask == id)//if the task is the one we want to delete
                throw new Exception($"cannot delete the task with ID={id} because the task with the ID={i.DependentTask} depends on it");
        }
        Task? t = task with { isActive = false };//create a new task with the same data but not active
        DataSource.Tasks.Remove(task);//remove the task
        DataSource.Tasks.Add(t);//add the new task
    }
}

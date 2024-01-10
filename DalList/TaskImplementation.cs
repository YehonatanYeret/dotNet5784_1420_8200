namespace Dal;
using DalApi;
using DO;

internal class TaskImplementation : ITask
{
    //create a new task
    public int Create(Task item)
    {
        int ID = DataSource.Config.NextTaskId;//get the next id
        Task task = item with { Id = ID };//create a new task with the new id
        DataSource.Tasks.Add(task);//add the task to the list
        return ID;//return the id
    }

    //read a task and return it. if not found return null
    public Task? Read(int id)
    {
        return DataSource.Tasks.Find(task => task.Id == id && task.isActive);//find the task with the id and if not found return null
    }

    //return a copy of the list of tasks
    public IEnumerable<Task> ReadAll()
    {
        IEnumerable<Task> tasks = DataSource.Tasks;
        return from task in tasks
               where task.isActive == true
               select task;

    }

    //update a task by removing the old one and adding the new one
    public void Update(Task item)
    {
        //find the index of the task with the same id
        Task? task = DataSource.Tasks.Find(task => task.Id == item.Id && task.isActive);
        if (task == null)//if not found
            throw new DO.DalNotExistException($"Task with ID={item.Id} does not exist");//throw exception

        //DataSource.Tasks[task.Id] = item;//update the task
        //can work but not how we have been asked for

        //remove the task
        DataSource.Tasks.RemoveAll(t => t?.Id == task.Id);
        DataSource.Tasks.Add(item);//add the new task
    }

    //we dont need to check if there is no tasks with the task id because we will check it in the logic layer
    public void Delete(int id)
    {
        Task? task = DataSource.Tasks.Find(task => task.Id == id && task.isActive);//find the index of the task with the same id
        if (task == null)//if not found
            throw new Exception($"Task with ID={id} does not exist");//throw exception

        foreach (var i in DataSource.Dependencies)//for each dependency
        {
            if (i.DependentOnTask == id)//if the task is the one we want to delete
                throw new Exception($"cannot delete the task with ID={id} because the task with the ID={i.DependentTask} depends on it");
        }
        Task? t = task with { isActive = false };//create a new task with the same data but not active
        DataSource.Tasks.RemoveAll(temp => temp.Id == id);//remove the task
        DataSource.Tasks.Add(t);//add the new task
    }
}

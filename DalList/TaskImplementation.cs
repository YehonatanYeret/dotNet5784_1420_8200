namespace Dal;
using DalApi;
using DO;

/// <summary>
/// implement the interface ITask
/// </summary>
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
        return DataSource.Tasks.FirstOrDefault(task => task.Id == id);//find the task with the id and if not found return null
    }

    //read a task that matches the condition and return it. if not found return null
    public Task? Read(Func<Task, bool> filter)
    {
        // find the task that matches the condition and if not found return null
        return DataSource.Tasks.FirstOrDefault(filter);
    }

    //return a copy of the list of tasks
    public IEnumerable<Task> ReadAll(Func<Task, bool>? filter = null)
    {
        if(filter != null)
            return DataSource.Tasks.Where(task => filter(task));//return a copy of the list of tasks

        return DataSource.Tasks.Select(task => task);//return a copy of the list of tasks
    }

    //update a task by removing the old one and adding the new one
    public void Update(Task item)
    {
        //find the index of the task with the same id
        Task? task = DataSource.Tasks.FirstOrDefault(task => task.Id == item.Id);
        if (task == null)//if not found
            throw new DalDoesNotExistException($"Task with ID={item.Id} does not exist");//throw exception

        //remove the task
        DataSource.Tasks.RemoveAll(t => t?.Id == task.Id);
        DataSource.Tasks.Add(item);//add the new task
    }

    //we dont need to check if there is no tasks with the task id because we will check it in the logic layer
    public void Delete(int id)
    {
        Task? task = DataSource.Tasks.FirstOrDefault(task => task.Id == id && task.IsActive);//find the index of the task with the same id
        if (task == null)//if not found
            throw new DalDoesNotExistException($"Task with ID={id} does not exist");//throw exception

        Task? t = task with { IsActive = false };//create a new task with the same data but not active
        DataSource.Tasks.RemoveAll(temp => temp.Id == id);//remove the task
        DataSource.Tasks.Add(t);//add the new task
    }

    //delete all tasks
    public void DeleteAll() { DataSource.Tasks.Clear(); DataSource.Config.ResetTaskId(); }
}

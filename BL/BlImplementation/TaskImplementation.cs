namespace BlImplementation;
using BlApi;

internal class TaskImplementation : ITask
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    public void Create(BO.Task task)
    {
        if(task.Id<0)
            throw new Exception("id must be positive");//nedd to change to BO exception
        if(string.IsNullOrEmpty(task.Alias))
            throw new Exception("alias must not be empty");//nedd to change to BO exception
        //add all dependencies to the database
        //add the task to the database
    }

    public BO.Task Read(int id)
    {
        DO.Task? task = _dal.Task.Read(id);
        if (task == null)
            throw new Exception($"no task with id {id}");//nedd to change to BO exception 
        return CreateTask(task);
    }

    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        if(filter != null)
            return _dal.Task.ReadAll().Select(task => CreateTask(task!)).Where(filter);

        return  _dal.Task.ReadAll().Select(task=>CreateTask(task!));
    }

    public void Delete(int id)
    {
        throw new NotImplementedException();
    }

    public void Update(BO.Task task)
    {
        throw new NotImplementedException();
    }

    public void UpdateScheduledDate(int id, DateTime time)
    {
        throw new NotImplementedException();
    }


    private BO.TaskInList ConvertToTaskInList(int id)
    {
        DO.Task task = _dal.Task.Read(id)!;
        return new BO.TaskInList()
        {
            Id = id,
            Description = task.Description,
            Alias = task.Alias,
            Status = CalculateStatus(task)
        };
    }

    private List<BO.TaskInList> GetAllDependencies(int id)
    {
        //read all needed dependencies and convert all dependencies to BO.taskinlist
        IEnumerable<BO.TaskInList> dependenciesInList = from dep in _dal.Dependency.ReadAll(dep => dep.DependentOnTask == id)
                                                        where dep.DependentTask is not null
                                                        select ConvertToTaskInList((int)dep.DependentTask!);
        return dependenciesInList.ToList();
    }

    private BO.EngineerInTask? ConvertToEngineerInTask(int? id)
    {
        if (id is null) return null;

        DO.Engineer engineer = _dal.Engineer.Read((int)id)!;
        return new BO.EngineerInTask()
        {
            Id = (int)id,
            Name = engineer.Name
        };
    }

    private BO.Status CalculateStatus(DO.Task task)
    {
        if (task.ScheduledDate is null) return BO.Status.Unscheduled;
        if (task.IsMileStone) return BO.Status.InJeopardy;
        if (task.CompleteDate < DateTime.Now) return BO.Status.Done;
        if (task.StartDate < DateTime.Now) return BO.Status.OnTrack;
        return BO.Status.Unscheduled;
    }

    private BO.Task CreateTask(DO.Task task)
    {
        //convert the task to BO.task and return it
        return new BO.Task()
        {
            Id = task.Id,
            Description = task.Description,
            Alias = task.Alias,
            CreatedAtDate = task.CreatedAtDate,
            Status = CalculateStatus(task),
            Dependencies = GetAllDependencies(task.Id),
            //Milestone = task.Milestone,
            RequiredEffortTime = task.RequiredEffortTime,
            StartDate = task.StartDate,
            ScheduledDate = task.ScheduledDate,
            ForecastDate = (task.ScheduledDate > task.StartDate + task.RequiredEffortTime) ?
                                task.ScheduledDate : task.StartDate + task.RequiredEffortTime,
            DeadlineDate = task.DeadlineDate,
            CompleteDate = task.CompleteDate,
            Deliverables = task.Deliverables,
            Remarks = task.Remarks,
            Engineer = ConvertToEngineerInTask(task.EngineerId),
            Copmlexity = (BO.EngineerExperience?)task.Complexity,
        };
    }
}

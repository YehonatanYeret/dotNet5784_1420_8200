namespace BlImplementation;
using BlApi;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;
    private readonly IBl _bl;
    internal EngineerImplementation(IBl bl) => _bl = bl;
    /// <summary>
    /// Creates a new engineer.
    /// </summary>
    /// <param name="engineer">The engineer object to create.</param>
    /// <returns>The ID of the newly created engineer.</returns>
    public int Create(BO.Engineer engineer)
    {
        //check if the values is correct
        checkEngineer(engineer);

        // Create a new DO.Engineer object from the BO.Engineer object.
        DO.Engineer? doEngineer = new DO.Engineer
        (
            Id: engineer.Id,
            Cost: engineer.Cost,
            Name: engineer.Name,
            Email: engineer.Email,
            Level: (DO.EngineerExperience)engineer.Level,
            Image: engineer.Image
        );

        try
        {
            return _dal.Engineer.Create(doEngineer);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BLAlreadyExistsException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Retrieves a specific engineer by their ID.
    /// </summary>
    /// <param name="id">The ID of the engineer to retrieve.</param>
    /// <returns>The engineer with the specified ID.</returns>
    public BO.Engineer Read(int id)
    {

        DO.Engineer? engineer = _dal.Engineer.Read(id);

        // If the engineer does not exist, throw a BLDoesNotExistException.
        if (engineer is null)
            throw new BO.BLDoesNotExistException($"Engineer with ID {id} does not exist");

        // Otherwise, create a new BO.Engineer object from the DO.Engineer object.

        //get the task of the engineer if exist
        BO.TaskInEngineer? task = (from t in _dal.Task.ReadAll()
                                   where t.EngineerId == id
                                   && CalculateStatus(t) == BO.Status.OnTrack
                                   select new BO.TaskInEngineer
                                   {
                                       Id = t.Id,
                                       Alias = t.Alias
                                   }).FirstOrDefault();

        //create the BO.Engineer object 
        BO.Engineer? boEngineer = new BO.Engineer()
        {
            Id = id,
            Cost = engineer.Cost,
            Name = engineer.Name,
            Email = engineer.Email,
            Level = (BO.EngineerExperience)engineer.Level,
            Task = task,
            Image = engineer.Image
        };

        return boEngineer;
    }

    /// <summary>
    /// Retrieves all engineers based on an optional filter.
    /// </summary>
    /// <param name="filter">An optional filter function to apply to the engineers.</param>
    /// <returns>An IEnumerable of engineers that satisfy the provided filter.</returns>
    /// 
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null)
    {
        //if there is no filter, return all the engineers
        if (filter is null)
            filter = (e) => true;

        //get all the engineers that return true with filter from the dal
        return (from e in _dal.Engineer.ReadAll()
                let engineer = new BO.Engineer
                {
                    Id = e.Id,
                    Cost = e.Cost,
                    Name = e.Name,
                    Email = e.Email,
                    Level = (BO.EngineerExperience)e.Level,
                    Image = e.Image
                }
                where filter(engineer)
                select engineer);
    }

    /// <summary>
    /// Updates an existing engineer.
    /// </summary>
    /// <param name="engineer">The engineer object with updated information.</param>
    public void Update(BO.Engineer engineer)
    {
        //check the values of the engineer
        checkEngineer(engineer);

        if (Read(engineer.Id) != null && engineer.Level < Read(engineer.Id)!.Level)
            throw new BO.BLValueIsNotCorrectException($"Engineer level cannot be lower than the current level: {engineer.Level}");

        try
        {
            _dal.Engineer.Update(new DO.Engineer
            (
            Id: engineer.Id,
            Cost: engineer.Cost,
            Name: engineer.Name,
            Email: engineer.Email,
            Level: (DO.EngineerExperience)engineer.Level,
            Image: engineer.Image
            ));

            //update the task of the engineer if exist
            if (engineer.Task is not null)
            {
                DO.Task? task = _dal.Task.Read(engineer.Task.Id);

                // If the task does exist, update the task with the new engineer ID.
                if (task is not null)
                    _dal.Task.Update(task with { EngineerId = engineer.Id });
            }
        }
        // If the engineer does not exist, throw a BLDoesNotExistException.
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException(ex.Message, ex);
        }

    }

    /// <summary>
    /// Deletes an existing engineer.
    /// </summary>
    /// <param name="id">The id of the engineer that need to delete.</param>
    public void Delete(int id)
    {
        //check if the engineer has tasks that on track or done
        IEnumerable<DO.Task?> tasks = from task in _dal.Task.ReadAll(task => task.EngineerId == id)
                                      let stat = CalculateStatus(task!)
                                      where stat == BO.Status.OnTrack || stat == BO.Status.Done
                                      select task;


        // If the engineer has tasks that are on track or done, throw a BLDeletionImpossible.
        if (tasks.Any())
            throw new BO.BLDeletionImpossible($"cannot delete Engineer with ID {id}");

        try
        {
            _dal.Engineer.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException(ex.Message, ex);
        }
    }

    /// <summary>
    /// check if the values of the engineer is correct
    /// </summary>
    /// <param name="engineer">the engineer that need to check</param>
    private void checkEngineer(BO.Engineer engineer)
    {
        //check if the id is not negative
        if (engineer.Id <= 0)
            throw new BO.BLValueIsNotCorrectException($"Engineer ID has to be positive: {engineer.Id}");

        //check if the name is not empty
        if (string.IsNullOrEmpty(engineer.Name))
            throw new BO.BLValueIsNotCorrectException($"Engineer name cannot be empty: {engineer.Name}");

        //check if the cost is not negative
        if (engineer.Cost <= 0)
            throw new BO.BLValueIsNotCorrectException($"Engineer cost has to be positive: {engineer.Cost}");

        //check if the level is none
        if (engineer.Level == BO.EngineerExperience.None)
            throw new BO.BLValueIsNotCorrectException("Cannot set none to level of engineer");
        //check if the email is valid
        var mail = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
        if (engineer.Email is null || !mail.IsValid(engineer.Email))
            throw new BO.BLValueIsNotCorrectException($"Engineer email is not valid: {engineer.Email}");
    }

    /// <summary>
    /// set the task to the engineer
    /// </summary>
    /// <param name="engineerId">the id of the engineer</param>
    /// <param name="taskId">the id of the task</param>
    public void SetTaskToEngineer(int engineerId, int taskId)
    {
        //check if the engineer and the task exist
        DO.Engineer? engineer = _dal.Engineer.Read(engineerId) ??
            throw new BO.BLDoesNotExistException($"Engineer with ID {engineerId} does not exist");

        DO.Task? task = _dal.Task.Read(taskId) ??
            throw new BO.BLDoesNotExistException($"Task with ID {taskId} does not exist");

        if (task.EngineerId is not null)
            throw new BO.BLAlreadyExistsException($"Task with ID {taskId} already have an engineer");

        //check if the engineer already work on a task
        var status = CalculateStatus(task);
        if (status == BO.Status.OnTrack && _dal.Task.ReadAll(tsk => tsk.EngineerId == engineerId && status is BO.Status.OnTrack).Any())
            throw new BO.BLAlreadyExistsException($"Engineer with ID {engineerId} already work on a task");

        //check if the engineer level is lower or equal then the task level
        if (engineer.Level < task.Complexity)
            throw new BO.BLValueIsNotCorrectException($"Engineer level cannot be lower than the task level: {engineer.Level}");

        //update the task with the engineer id
        _dal.Task.Update(task with { EngineerId = engineerId });
    }

    /// <summary>
    /// remove the task from the engineer
    /// </summary>
    /// <param name="engineerId"></param>
    /// <exception cref="BO.BLDoesNotExistException"></exception>
    public void RemoveTaskFromEngineer(int engineerId)
    {
        //check if the engineer exist 
        DO.Engineer? engineer = _dal.Engineer.Read(engineerId) ??
              throw new BO.BLDoesNotExistException($"Engineer with ID {engineerId} does not exist");

        //check if the engineer work on a task and remove the task
        var task = _dal.Task.Read(task => task.EngineerId == engineerId && task.StartDate > _bl.Time);
        if (task is not null)
            _dal.Task.Update(task with { EngineerId = null });
    }

    /// <summary>
    /// delete all the engineers
    /// </summary>
    public void Reset() => _dal.Engineer.DeleteAll();

    /// <summary>
    /// get the engineer that work on the task
    /// </summary>
    /// <param name="engineerId">the id of the engineer</param>
    /// <returns>the engineer that work on the task convert to EngineerInTask</returns>
    public BO.EngineerInTask GetEngineerInTask(int engineerId)
    {

        return new BO.EngineerInTask
        {
            Id = engineerId,
            Name = _dal.Engineer.Read(engineerId)!.Name
        };
    }

    /// <summary>
    /// get all the tasks of the engineer that he can start to work on
    /// </summary>
    /// <param name="engineerId">the id of the engineer</param>
    /// <returns>the tasks that the engineer can start to work on</returns>
    public IEnumerable<BO.TaskInList> GetTasksOfEngineerToWork(int engineerId)
    {
        //return IEnumerable of the tasks that the engineer can start to work on based on the status and the dependencies
        return GetAllTaskOfEngineer(engineerId, task => task.Status == BO.Status.Scheduled && !task.Dependencies!.Any(dep => dep.Status != BO.Status.Done));
    }

    /// <summary>
    /// get all the tasks of the engineer even if he can't start to work on or he already finish the task
    /// </summary>
    /// <param name="engineerId">the id of the engineer</param>
    /// <returns>the tasks that sets to the engineer</returns>
    public IEnumerable<BO.TaskInList> GetAllTaskOfEngineer(int engineerId, Func<BO.Task, bool>? filter = null)
    {
        //if there is no filter, return all the tasks without filter
        if (filter is null)
            filter = (t) => true;

        //return IEnumerable of the tasks after the filter in convert to TaskInList
        return (from t in _dal.Task.ReadAll(task => task.EngineerId == engineerId)
                let task = _bl.Task.Read(t.Id)
                where filter(task)
                select _bl.Task.ConvertToTaskInList(t.Id));
    }

    public string ConvertImageToBase64(string path)
    {
        using FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        byte[] bitmapBytes = new byte[stream.Length];
        stream.Read(bitmapBytes, 0, (int)stream.Length);

        // Convert the byte array to a Base64 string
        return Convert.ToBase64String(bitmapBytes);
    }

    /// <summary>
    /// Calculate the status of a task based on its scheduled date, complete date, and milestone status.
    /// </summary>
    /// <param name="task">The task that we calaulate the steatus for</param>
    /// <returns>the status of the task</returns>
    BO.Status CalculateStatus(DO.Task task)
    {
        //check if the task has scheduled date
        if (task.ScheduledDate is null) return BO.Status.Unscheduled;
        //chelk if the task not started yet
        else if (task.StartDate is null) return BO.Status.Scheduled;
        //check if the task not completed yet
        else if (task.CompleteDate is null) return BO.Status.OnTrack;
        //the task has started and completed
        else return BO.Status.Done;
    }
}

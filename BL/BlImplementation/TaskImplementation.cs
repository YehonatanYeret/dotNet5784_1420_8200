namespace BlImplementation;

internal class TaskImplementation : BlApi.ITask
{
    // Data Access Layer instance
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Creates a new task in the system.
    /// </summary>
    /// <param name="task">The task to be created</param>
    public int Create(BO.Task task)
    {
        CheckTask(task);
        // Create dependencies if they exist
        task.Dependencies?.Select(dep => _dal.Dependency.Create(new(0, task.Id, dep.Id)));
        // Create the task in the data access layer
        int id = _dal.Task.Create(new DO.Task()
        {
            // Map properties from business object to data object
            Id = task.Id,
            Alias = task.Alias,
            Description = task.Description,
            CreatedAtDate = task.CreatedAtDate,
            ScheduledDate = task.ScheduledDate,
            StartDate = task.StartDate,
            RequiredEffortTime = task.RequiredEffortTime,
            DeadlineDate = task.DeadlineDate,
            CompleteDate = task.CompleteDate,
            Deliverables = task.Deliverables,
            Remarks = task.Remarks,
            EngineerId = task.Engineer?.Id,
            Complexity = (DO.EngineerExperience?)task.Complexity,
        });
        return id;
    }

    /// <summary>
    /// Retrieves a task by its ID.
    /// </summary>
    /// <param name="id">The ID of the task to be retrieved</param>
    /// <returns>The task with the given ID</returns>
    public BO.Task Read(int id)
    {
        DO.Task task = _dal.Task.Read(id) ?? throw new BO.BLDoesNotExistException($"No task found with ID {id}");
        return CreateTask(task);
    }

    /// <summary>
    /// Retrieves all tasks based on an optional filter.
    /// </summary>
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null)
    {
        if (filter != null)
            return _dal.Task.ReadAll()!.Select(task => CreateTask(task!)).Where(filter);

        return _dal.Task.ReadAll()!.Select(task => CreateTask(task!));
    }

    /// <summary>
    /// Updates an existing task in the system.
    /// </summary>
    /// <param name="task">The task to be updated</param>
    public void Update(BO.Task task)
    {
        // Check if the task values are valid
        CheckTask(task);

        // Check if there is a circular dependency
      //  if (ThereIsCirculerDependency(task))
          //  throw new BO.BLValueIsNotCorrectException($"here is circuler dependency in the task with ID {task.Id}");

        try
        {
            // Perform the task update
            _dal.Task.Update(new DO.Task()
            {
                // Map properties from business object to data object
                Description = task.Description,
                Alias = task.Alias,
                CreatedAtDate = task.CreatedAtDate,
                ScheduledDate = _dal.Task.Read(task.Id)!.ScheduledDate,
                StartDate = task.StartDate,
                RequiredEffortTime = task.RequiredEffortTime,
                DeadlineDate = task.DeadlineDate,
                CompleteDate = task.CompleteDate,
                Deliverables = task.Deliverables,
                Remarks = task.Remarks,
                EngineerId = task.Engineer?.Id,
                Complexity = (DO.EngineerExperience?)task.Complexity,
            });

            // delete the old dependencies
            foreach (DO.Dependency item in _dal.Dependency.ReadAll(dep => dep.DependentTask == task.Id)!)
                _dal.Dependency.Delete(item!.Id);

            // Recreate dependencies if they exist
            task.Dependencies?.Select(dep => _dal.Dependency.Create(new(0, task.Id, dep.Id)));
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Updates the scheduled date of a task.
    /// </summary>
    /// <param name="id">The ID of the task to be updated</param>
    /// <param name="time">The new scheduled date</param>
    public void UpdateScheduledDate(int id, DateTime time)
    {
        DO.Task? taskToUpdate = _dal.Task.Read(id);
        if (taskToUpdate is null)
            throw new BO.BLDoesNotExistException($"No task found with ID {id}");

        // Check if the update is valid
        CheckForDate(id, time);

        // Update the scheduled date
        taskToUpdate = taskToUpdate with { ScheduledDate = time };
        _dal.Task.Update(taskToUpdate);
    }

/// <summary>
/// Deletes a task from the system.
/// </summary>
/// <param name="id">The ID of the task to be deleted</param>
    public void Delete(int id)
    {
        // Check if the task has dependencies before deletion
        if (_dal.Dependency.ReadAll(dep => dep.DependentOnTask == id)!.Any())
            throw new BO.BLDeletionImpossible($"cannot delete Task with ID {id}");

        try
        {
            // Attempt to delete the task
            _dal.Task.Delete(id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Calculates the closest start date for a task based on its dependencies and the project start date.
    /// </summary>
    /// <param name="id">The task that we calculate the date for</param>
    /// <param name="startProject">The date of the start of the program</param>
    /// <returns>The calculated date</returns>
    public DateTime CalculateClosestStartDate(int id, DateTime startProject)
    {
        // Read all dependencies for the given task ID
        IEnumerable<DO.Dependency>? dependencies = _dal.Dependency.ReadAll(dep => dep.DependentTask == id);

        // If there are no dependencies, return the project start date
        if (dependencies is null)
            return startProject;

        // If there are unscheduled dependencies, throw an exception
        if (dependencies.Any(dep => Read(dep.DependentOnTask).ScheduledDate is null))
            throw new BO.BLValueIsNotCorrectException($"Task with ID {id} has unscheduled dependencies");

        // Order dependencies by their forecastDate date, descending
        dependencies.OrderByDescending(dep => Read(dep.DependentOnTask).ForecastDate);

        // Return the forecast date of the first dependency (the one with the latest forecast date)
        return Read(dependencies.First().DependentOnTask).ForecastDate ?? startProject;
    }

    /// <summary>
    /// Convert the task to BO.TaskInList and return it
    /// </summary>
    /// <param name="id">The task that we convert id</param>
    /// <returns>The task but converted to TaskInList</returns>
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

    /// <summary>
    /// get all the dependencies of the task with the id
    /// </summary>
    /// <param name="id">The task id</param>
    /// <returns>List of TaskInList that presents the tasks that the task is dependent on</returns>
    private List<BO.TaskInList> GetAllDependencies(int id)
    {
        // Read all needed dependencies and convert them to BO.TaskInList
        IEnumerable<BO.TaskInList> dependenciesInList = from dep in _dal.Dependency.ReadAll(dep => dep.DependentOnTask == id)
                                                        select ConvertToTaskInList((int)dep.DependentTask!);
        return dependenciesInList.ToList();
    }

    /// <summary>
    /// convert the engineer to BO.EngineerInTask and return it
    /// </summary>
    /// <param name="id">The engineer that we convert from DO.Engineer to BO.EngineerInTask</param>
    /// <returns>The new BO.EngineerInTask</returns>
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

    /// <summary>
    /// convert the task to BO.task and return it
    /// </summary>
    /// <param name="task">The task that we convert from DO to BO</param>
    /// <returns>The new BO task</returns>
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
            Complexity = (BO.EngineerExperience?)task.Complexity,
        };
    }

    /// <summary>
    /// check if the task is valid
    /// </summary>
    /// <param name="task">The task that we chack</param>
    private static void CheckTask(BO.Task task)
    {
        if (string.IsNullOrEmpty(task.Alias))
            throw new BO.BLValueIsNotCorrectException("alias must not be empty");//nedd to change to BO exception
    }

    /// <summary>
    /// Checks if it's valid to update the scheduled date of a task based on its dependencies and completion dates.
    /// </summary>
    /// <param name="id">The ID of the task being updated.</param>
    /// <param name="time">The new scheduled date to be checked.</param>
    private void CheckForDate(int id, DateTime? time)
    {
        // If the scheduled date is not provided, no need to perform checks
        if (time is null)
            return;

        // Retrieve all dependent tasks for the given task ID
        IEnumerable<DO.Task>? tasks = from temp in _dal.Dependency.ReadAll(dep => dep.DependentTask == id)
                                      select _dal.Task.Read((int)temp.DependentOnTask!);

        // Check if any dependent task has an unscheduled date
        if (tasks.Any(task => task.ScheduledDate is null))
            throw new BO.BLValueIsNotCorrectException("Cannot update scheduled date of task with unscheduled dependencies");

        // Check if the new scheduled date is before the complete date of any dependent task
        if (tasks.Any(task => time < task.CompleteDate))
            throw new BO.BLValueIsNotCorrectException("Cannot update scheduled date of task to be before the complete date of its dependencies");
    }

    /// <summary>
    /// Calculate the status of a task based on its scheduled date, complete date, and milestone status.
    /// </summary>
    /// <param name="task">The task that we calaulate the steatus for</param>
    /// <returns>the status of the task</returns>
    internal static BO.Status CalculateStatus(DO.Task task)
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

    /// <summary>
    /// Checks if there is a circular dependency between tasks.
    /// </summary>
    /// <param name="task"> The task we wand to check if he has circuler dependency</param>
    /// <returns> Returns true if there is circuler dependency and else returns false</returns>
    internal bool ThereIsCirculerDependency(BO.Task task)
    {
        //iterate over all the dependencies of the task and check if there is circuler dependency
        foreach (var item in task.Dependencies!)
        {
            //if there is circuler dependency return true
            if (ThereIsCircularDependency(item.Id, task.Id))
                return true;
        }
        //after iterating over all the dependencies and there is no circuler dependency return false
        return false;
    }

    /// <summary>
    /// Checks if there is a circular dependency between tasks.
    /// </summary>
    /// <param name="taskId">The ID of the task that the current task depends on.</param>
    /// <param name="id">The ID of the current task being checked for circular dependencies.</param>
    /// <returns>True if a circular dependency exists, otherwise false.</returns>
    private bool ThereIsCircularDependency(int? taskId, int id)
    {
        // Base case: If the task ID is the same as the current task ID, there is a circular dependency.
        if (taskId == id)
            return true;

        // Base case: If the task ID is null, there is no circular dependency.
        if (taskId == null)
            return false;

        // Retrieve dependencies for the current task.
        IEnumerable<DO.Dependency> dependencies = _dal.Dependency.ReadAll(dep => dep.DependentTask == id)!;

        // Check for circular dependencies in each dependency.
        foreach (var item in dependencies)
        {
            // Recursive call to check for circular dependencies in the dependent task.
            if (ThereIsCircularDependency(item.DependentOnTask, id))
            {
                return true;
            }
        }

        // No circular dependency found for the current task.
        return false;
    }
}

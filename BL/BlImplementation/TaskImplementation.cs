using BlApi;
using BO;

namespace BlImplementation;

internal class TaskImplementation : BlApi.ITask
{
    // Data Access Layer instance
    private readonly DalApi.IDal _dal = DalApi.Factory.Get;
    private readonly IBl _bl;
    internal TaskImplementation(IBl bl) => _bl = bl;
    /// <summary>
    /// Creates a new task in the system.
    /// </summary>
    /// <param name="task">The task to be created</param>
    public int Create(BO.Task task)
    {
        CheckTask(task);

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
            Complexity = (DO.EngineerExperience)task.Complexity,
        });
        // Create dependencies if they exist
        task.Dependencies?.ForEach(dep => _dal.Dependency.Create(new(0, id, dep.Id)));
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
    public IEnumerable<BO.Task> ReadAllTask(Func<BO.Task, bool>? filter = null)
    {
        if (filter != null)
            return _dal.Task.ReadAll(task => task.IsActive)!.Select(task => CreateTask(task!)).Where(filter);

        return _dal.Task.ReadAll(task => task.IsActive)!.Select(task => CreateTask(task!));
    }

    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.TaskInList, bool>? filter = null)
    {
        if (filter != null)
            return _dal.Task.ReadAll(task => task.IsActive)!.Select(task => ConvertToTaskInList(task.Id)).Where(filter);

        return _dal.Task.ReadAll(task => task.IsActive
        )!.Select(task => ConvertToTaskInList(task.Id));
    }

    /// <summary>
    /// Updates an existing task in the system.
    /// </summary>
    /// <param name="task">The task to be updated</param>
    public void Update(BO.Task task)
    {
        // Check if the task values are valid
        CheckTask(task);

        // Create a graph to detect cyclic dependencies
        Graph graph = new(_dal.Task.ReadAll().Max(x=>x.Id));

        // Add edges to the graph
        foreach (TaskInList t in task.Dependencies!)
        {
            graph.AddEdge(task.Id-1, t.Id - 1);
        }
        //add the rest of the edges
        IEnumerable<DO.Dependency> dependencies = _dal.Dependency.ReadAll();
        foreach (DO.Dependency dep in dependencies)
            if ((dep.DependentTask != task.Id))
                graph.AddEdge(dep.DependentTask - 1, dep.DependentOnTask - 1);

        // Detect cross edges in the graph and throw an exception if found
        if (graph.DetectCycle())
            throw new BO.BLValueIsNotCorrectException("The task has a cyclic dependency");

        try
        {
            // Perform the task update
            _dal.Task.Update(new DO.Task()
            {
                // Map properties from business object to data object
                Id = task.Id,
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
                Complexity = (DO.EngineerExperience)task.Complexity
            });

            // delete the old dependencies
            foreach (DO.Dependency item in _dal.Dependency.ReadAll(dep => dep.DependentTask == task.Id)!)
                _dal.Dependency.Delete(item.Id);

            // Recreate dependencies if they exist
            task.Dependencies?.ForEach(dep => _dal.Dependency.Create(new(0, task.Id, dep.Id)));
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
    ///  Updates the scheduled date of a task.
    /// </summary>
    /// <param name="id">The ID of the task to be updated</param>
    /// <param name="time"> The new scheduled date for the task.</param>
    public void UpdateScheduledDate(int id, DateTime time)
    {
        DO.Task? taskToUpdate = _dal.Task.Read(id);
        if (taskToUpdate is null)
            throw new BO.BLDoesNotExistException($"No task found with ID {id}");

        // Update the scheduled date
        _dal.Task.Update(taskToUpdate with { ScheduledDate = time });
    }

    /// <summary>
    ///  Updates the dates of a task, including the deadline date if provided.
    /// </summary>
    /// <param name="id">The ID of the task to be updated</param>
    /// <param name="deadlineDate">The new deadline date for the task (nullable).</param>
    public void UpdateDeadLineDate(int id, DateTime? deadlineDate)
    {
        BO.Task? taskToUpdate = Read(id);

        // Check if the task exists
        if (taskToUpdate is null)
            throw new BO.BLDoesNotExistException($"No task found with ID {id}");

        // Check if the deadline date is valid
        if (deadlineDate is not null && taskToUpdate.RequiredEffortTime > deadlineDate - taskToUpdate.ScheduledDate)
            throw new BO.BLValueIsNotCorrectException("The required effort time is greater than the time between the scheduled date and the deadline date");

        // Update the date
        taskToUpdate.DeadlineDate = deadlineDate;

        Update(taskToUpdate);
    }

    /// <summary>
    /// Change the status of the task
    /// </summary>
    /// <param name="id">The ID of the task to be updated</param>
    /// <returns>The new status of the task.</returns>
    public BO.Status ChangeStatusOfTask(int id)
    {
        BO.Task? taskToUpdate = Read(id);

        // Check if the task exists
        if (taskToUpdate is null)
            throw new BO.BLDoesNotExistException($"No task found with ID {id}");

        // Check if the task has an engineer
        if (taskToUpdate.Status == BO.Status.Scheduled && taskToUpdate.Engineer is null)
            throw new BO.BLValueIsNotCorrectException("The task has no Engineer");

        // Check if the engineer already has a task on track except the task that we want to update
        if (ReadAllTask(task => task.Engineer?.Id == taskToUpdate.Engineer!.Id && task.Status == BO.Status.OnTrack).Any() && taskToUpdate.Status == BO.Status.Scheduled)
            throw new BO.BLValueIsNotCorrectException($"The engineer with ID {taskToUpdate.Engineer!.Id} has a task on track");

        if (taskToUpdate.Status == BO.Status.Scheduled && taskToUpdate.Dependencies!.Any(dep => dep.Status != BO.Status.Done))
            throw new BO.BLValueIsNotCorrectException("First complete the dependencies tasks");

        // Check if the task is unscheduled
        if (taskToUpdate.Status == BO.Status.Unscheduled)
            throw new BO.BLValueIsNotCorrectException("The task is unscheduled");

        // Check if the task is already done
        if (taskToUpdate.Status == BO.Status.Done)
            throw new BO.BLValueIsNotCorrectException("The task is already done");

        // Update the status of the task
        if (taskToUpdate.Status == BO.Status.Scheduled)
            taskToUpdate.StartDate = _bl.Time;

        // Update the complete date if the task is done
        if (taskToUpdate.Status == BO.Status.OnTrack)
            taskToUpdate.CompleteDate = _bl.Time;

        Update(taskToUpdate);

        // Return the new status of the task
        return ++taskToUpdate.Status;
    }

    /// <summary>
    /// Calculates the closest start date for a task based on its dependencies and the project start date.
    /// </summary>
    /// <param name="id">The task that we calculate the date for</param>
    /// <param name="startProject">The date of the start of the program</param>
    /// <returns>The calculated date</returns>
    public DateTime CalculateClosestStartDate(int id, DateTime startProject)
    {
        // Read all dependencies for the given task ID when the task with the ID is the needy task
        IEnumerable<DO.Dependency> dependencies = from dep in _dal.Dependency.ReadAll()
                                                  where dep.DependentTask == id
                                                  select dep;

        // If there are no dependencies, return the project start date
        if (!dependencies.Any())
            return startProject;

        // Read all tasks that the given task is dependent on
        IEnumerable<DO.Task> tasks = from dep in dependencies
                                     select _dal.Task.Read(dep.DependentOnTask!);

        // If there are unscheduled dependencies, throw an exception
        if (tasks.Any(task => task.ScheduledDate is null))
            throw new BO.BLValueIsNotCorrectException($"Task with ID {id} has unscheduled dependencies");

        // Order dependencies by their forecastDate date, descending
        BO.Task t = CreateTask(tasks.OrderByDescending(task => CreateTask(task).ForecastDate).First());

        return (DateTime)t.ForecastDate!;
    }

    /// <summary>
    /// Convert the task to BO.TaskInList and return it
    /// </summary>
    /// <param name="id">The task that we convert id</param>
    /// <returns>The task but converted to TaskInList</returns>
    public BO.TaskInList ConvertToTaskInList(int id)
    {
        DO.Task task = _dal.Task.Read(id)!;

        // Convert the task to a TaskInList object
        return new BO.TaskInList()
        {
            Id = task.Id,
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
        IEnumerable<BO.TaskInList> dependenciesInList = from dep in _dal.Dependency.ReadAll()
                                                        where dep.DependentTask == id
                                                        select ConvertToTaskInList(dep.DependentOnTask);
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
            DeadlineDate = task.DeadlineDate,
            CompleteDate = task.CompleteDate,
            Deliverables = task.Deliverables,
            Remarks = task.Remarks,
            Engineer = ConvertToEngineerInTask(task.EngineerId),
            Complexity = (BO.EngineerExperience)task.Complexity,
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
        if (string.IsNullOrEmpty(task.Description))
            throw new BO.BLValueIsNotCorrectException("description must not be empty");//nedd to change to BO exception
        if (task.RequiredEffortTime < TimeSpan.Zero)
            throw new BO.BLValueIsNotCorrectException("required effort time must not be negative");//nedd to change to BO exception
        if(task.Complexity == EngineerExperience.None)
            throw new BO.BLValueIsNotCorrectException("complexity must not be none");//nedd to change to BO exception
    }

    /// <summary>
    /// Calculate the status of a task based on its scheduled date, complete date, and milestone status.
    /// </summary>
    /// <param name="task">The task that we calaulate the steatus for</param>
    /// <returns>the status of the task</returns>
    internal BO.Status CalculateStatus(DO.Task task)
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
    /// Set all the dates of the tasks automatically
    /// </summary>
    /// <param name="startProject">The start date of the project.</param>
    public void StartProject(DateTime startProject)
    {
        //go through all the tasks and set the scheduled date
        while (ReadAllTask(task => task.ScheduledDate == null).Any())
        {
            foreach (var item in ReadAllTask(task => task.ScheduledDate == null))
            {
                try
                {
                    // calculate the closest date based on the startDate of the program and the depndencies 
                    DateTime closest = CalculateClosestStartDate(item.Id, startProject);
                    UpdateScheduledDate(item.Id, closest);
                    UpdateDeadLineDate(item.Id, closest + item.RequiredEffortTime);
                }
                //if the task has unscheduled dependencies ignore it
                catch { }
            }
        }
        //start the project
        _dal.Clock.SetStartProject(startProject);
    }


    /// <summary>
    /// return if the task is in delay
    /// </summary>
    /// <param name="id">the id of the task</param>
    /// <returns>if the task is in delay</returns>
    public bool InDelay(int id)
    {
        BO.Task task = Read(id);

        //there are dependencies and one of them in delay
        if (task.Dependencies!.Any(t => InDelay(t.Id))) return true;

        //there are no dependencies in delay and the task is on delay
        if (task.ForecastDate < _bl.Time && task.CompleteDate == null) return true;

        //if all the dependencies are not in delay and the task is not in delay
        return false;
    }

    /// <summary>
    /// return the task in topology order
    /// </summary>
    /// <returns>the task in topology order</returns>
    public IEnumerable<BO.Task> GetTopologicalTasks()
    {
        Graph graph = new(_dal.Task.ReadAll(task => task.IsActive).Count());
        foreach (var item in _dal.Dependency.ReadAll())
        {
            graph.AddEdge(item.DependentTask - 1, item.DependentOnTask - 1);
        }
        return graph.TopologicalSort().Select(id => Read(id + 1));
    }

    /// <summary>
    /// all the tasks that are deleted to recover
    /// </summary>
    /// <returns>all of the deleted tasks</returns>
    public IEnumerable<BO.TaskInList> GetDeletedTasks()
    {
        return from t in _dal.Task.ReadAll(task => !task.IsActive)
               select new TaskInList()
               {
                   Id = t.Id,
                   Description = t.Description,
                   Alias = t.Alias,
                   Status = CalculateStatus(t)
               };
    }


    /// <summary>
    /// recover the task
    /// </summary>
    /// <param name="id">the id of the task</param>
    public void RecoverTask(int id)
    {
        DO.Task? task = _dal.Task.Read(id);

        if(task is null)
            throw new BO.BLDoesNotExistException($"No task found with ID {id}");

        _dal.Task.Update(task with { IsActive = true});
    }


    /// <summary>
    /// delete all the tasks and dependencies
    /// </summary>
    public void Reset()
    {
        _dal.Task.DeleteAll();
        _dal.Dependency.DeleteAll();
    }

}

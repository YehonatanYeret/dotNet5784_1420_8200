namespace BlApi;

/// <summary>
///  Task interface
/// </summary>
public interface ITask
{
    /// <summary>
    /// Creates a new task.
    /// </summary>
    /// <param name="task">The task to be created.</param>
    /// <returns>The ID of the created task.</returns>
    public int Create(BO.Task task);

    /// <summary>
    /// Retrieves a task based on its ID.
    /// </summary>
    /// <param name="id">The ID of the task to be retrieved.</param>
    /// <returns>The task with the specified ID.</returns>
    public BO.Task Read(int id);

    /// <summary>
    /// Retrieves all tasks based on an optional filter.
    /// </summary>
    /// <param name="filter">An optional filter function to apply on tasks.</param>
    /// <returns>A collection of tasks that match the specified filter.</returns>
    public IEnumerable<BO.Task> ReadAllTask(Func<BO.Task, bool>? filter = null);

    /// <summary>
    /// Retrieves all tasks based on an optional filter.
    /// </summary>
    /// <param name="filter">An optional filter function to apply on tasks.</param>
    /// <returns>A collection of TaskInList that match the specified filter.</returns>
    public IEnumerable<BO.TaskInList> ReadAll(Func<BO.TaskInList, bool>? filter = null);

    /// <summary>
    /// Updates an existing task.
    /// </summary>
    /// <param name="task">The updated task object.</param>
    public void Update(BO.Task task);

    /// <summary>
    /// Updates the scheduled date of a task.
    /// </summary>
    /// <param name="id">The ID of the task to be updated.</param>
    /// <param name="time">The new scheduled date for the task.</param>
    public void UpdateScheduledDate(int id, DateTime time);

    /// <summary>
    /// Deletes a task based on its ID.
    /// </summary>
    /// <param name="id">The ID of the task to be deleted.</param>
    public void Delete(int id);

    /// <summary>
    /// Calculates the closest start date for a task based on the project's start date.
    /// </summary>
    /// <param name="id">The ID of the task.</param>
    /// <param name="startProject">The start date of the project.</param>
    /// <returns>The calculated closest start date for the task.</returns>
    public DateTime CalculateClosestStartDate(int id, DateTime startProject);

    /// <summary>
    /// Updates the dates of a task, including the deadline date if provided.
    /// </summary>
    /// <param name="id">The ID of the task to be updated.</param>
    /// <param name="deadlineDate">The new deadline date for the task (nullable).</param>
    public void UpdateDeadLineDate(int id, DateTime? deadlineDate);

    /// <summary>
    /// Changes the status of a task.
    /// </summary>
    /// <param name="id">The ID of the task to be updated.</param>
    /// <param name="status">The new status for the task.</param>
    /// <returns>The new status of the task.</returns>
    public BO.Status ChangeStatusOfTask(int id);

    /// <summary>
    /// Updates the engineer of a task.
    /// </summary>
    /// <param name="id">The ID of the task to be updated.</param>
    /// <returns>The new engineer of the task.</returns>
    public BO.TaskInList ConvertToTaskInList(int id);

    /// <summary>
    /// Set all the dates of the tasks automatically
    /// </summary>
    /// <param name="startProject">The start date of the project.</param>
    public void StartProject(DateTime startProject);

    /// <summary>
    /// return if the task is in delay
    /// </summary>
    /// <param name="id">the id of the task</param>
    /// <returns>if the task is in delay</returns>
    public bool InDelay(int id);

    /// <summary>
    /// return the task in topology order
    /// </summary>
    /// <returns>the task in topology order</returns>
    public IEnumerable<BO.Task> GetTopologicalTasks();

    /// <summary>
    /// all the tasks that are deleted to recover
    /// </summary>
    /// <returns>all of the deleted tasks</returns>
    public IEnumerable<BO.TaskInList> GetDeletedTasks();

    /// <summary>
    /// recover the task
    /// </summary>
    /// <param name="id">the id of the task</param>
    public void RecoverTask(int id);

    /// <summary>
    /// Reset the task and the dependencies
    /// </summary>
    public void Reset();
}

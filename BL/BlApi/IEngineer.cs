namespace BlApi;

/// <summary>
/// Represents the interface for managing engineer-related functionalities.
/// </summary>
public interface IEngineer
{

    /// <summary>
    /// Creates a new engineer.
    /// </summary>
    /// <param name="engineer">The engineer object to create.</param>
    public int Create(BO.Engineer engineer);

    /// <summary>
    /// Retrieves a specific engineer by their ID.
    /// </summary>
    /// <param name="id">The ID of the engineer to retrieve.</param>
    /// <returns>The engineer with the specified ID.</returns>
    public BO.Engineer Read(int id);

    /// <summary>
    /// Retrieves all engineers based on an optional filter.
    /// </summary>
    /// <param name="filter">An optional filter function to apply to the engineers.</param>
    /// <returns>An IEnumerable of engineers that satisfy the provided filter.</returns>
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null);

    /// <summary>
    /// Updates an existing engineer.
    /// </summary>
    /// <param name="engineer">The engineer object with updated information.</param>
    public void Update(BO.Engineer engineer);

    /// <summary>
    /// Deletes an existing engineer.
    /// </summary>
    /// <param name="id">The id of the engineer that need to delete.</param>
    public void Delete(int id);

    /// <summary>
    /// Get all tasks of a specific engineer
    /// </summary>
    /// <param name="engineerId">The id of the engineer</param>
    /// <param name="taskId">The id of the task</param>
    public void SetTaskToEngineer(int engineerId, int taskId);

    /// <summary>
    /// Remove task from engineer
    /// </summary>
    /// <param name="engineerId">The id of the engineer</param>
    public void RemoveTaskFromEngineer(int engineerId);
}

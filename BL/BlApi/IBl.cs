namespace BlApi;

/// <summary>
/// Represents the main interface for accessing business logic functionalities.
/// </summary>
public interface IBl
{
    /// <summary>
    /// Gets an instance of the IEngineer interface for accessing engineer-related functionalities.
    /// </summary>
    public IEngineer Engineer { get; }

    /// <summary>
    /// Gets an instance of the ITask interface for accessing task-related functionalities.
    /// </summary>
    public ITask Task { get; }

    /// <summary>
    /// Gets an instance of the IClock interface for accessing clock-related functionalities.
    /// </summary>
    public IClock Clock { get; }
}

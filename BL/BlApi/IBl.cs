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

    /// <summary>
    /// Gets an instance of the IUser interface for accessing user-related functionalities.
    /// </summary>
    public IUser User { get; }

    /// <summary>
    /// initialize the db
    /// </summary>
    public void InitializeDB();

    /// <summary>
    /// delete all the db
    /// </summary>
    public void ResetDB();

    #region
    public DateTime Time { get; }
    public void AddYears();
    public void AddMonths();
    public void AddDays();
    public void AddHours();
    public void ResetTime();
    #endregion
}
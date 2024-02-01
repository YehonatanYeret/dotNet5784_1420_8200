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

    public BO.ProjectStatus GetProjectStatus()
    {
        if(StartProject == null)
            return BO.ProjectStatus.NotStarted;
        if (EndProject == null)
            return BO.ProjectStatus.InProgress;
        return BO.ProjectStatus.Done;
    }

    public DateTime? StartProject
    {
        get=>StartProject;
        set
        {
            if (GetProjectStatus() != BO.ProjectStatus.NotStarted)
                throw new InvalidOperationException("Project already started");
            StartProject = value;
        } 
    }

    public DateTime? EndProject
    {
        get => EndProject;
        set
        {
            if (GetProjectStatus() == BO.ProjectStatus.Done)
                throw new InvalidOperationException("Project already ended");
            EndProject = value;
        }
    }
}

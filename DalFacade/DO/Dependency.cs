namespace DO;

/// <summary>
/// Represents a Dependency entity for the database, representing a milestone in the project with all the relevant data.
/// </summary>
/// <param name="Id">The Id of the Dependency.</param>
/// <param name="DependentTask">The Id of the Task that depends on this Dependency.</param>
/// <param name="DependentOnTask">The Id of the last Task that our Task depends on.</param>
public record Dependency
(
    int Id,
    int DependentTask,
    int DependentOnTask
)
{
    public Dependency() : this(Id: 0, DependentTask: 0, DependentOnTask: 0) { }
}

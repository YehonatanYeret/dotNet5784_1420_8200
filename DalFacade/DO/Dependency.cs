namespace DO;


/// <summary>
/// Dependency entitiy for the database. represents a milestone in the project with all the relevant data
/// </summary>
/// <param name="Id">the Id of the Milestone</param>
/// <param name="DependentTask">the Id of the Task that depend on this Mailestone</param>
/// <param name="DependentOnTask">the Id of the last Task that our Task depend on </param>
public record Dependency
(
   int Id,
   int? DependentTask = null,
   int? DependentOnTask = null
)
{ 
    public Dependency() : this(0) { }// empty ctor for Milestone
}


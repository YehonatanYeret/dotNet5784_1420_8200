namespace DO;


/// <summary>
/// Milestone entitiy for the database. represents a milestone in the project with all the relevant data
/// </summary>
/// <param name="ID">the ID of the Milestone</param>
/// <param name="DependentTask">the ID of the Task that depend on this Mailestone</param>
/// <param name="DependentOnTask">the ID of the last Task that our Task depend on </param>
public record Milestone
(
   int ID,
   int? DependentTask = null,
   int? DependentOnTask = null
)
{ 
    public Milestone() : this(0) { }// empty ctor for Milestone
}


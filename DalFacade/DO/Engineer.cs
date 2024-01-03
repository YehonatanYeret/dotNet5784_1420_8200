namespace DO;

/// <summary>
/// Engineer entitiy for the database. represents an engineer in the project with all the relevant data
/// </summary>
/// <param name="Id">the Id of the Engineer</param>
/// <param name="Cost">how much the Engineer take for hour</param>
/// <param name="Name">the name of the Engineer</param>
/// <param name="Email">the email of the Engineer</param>
/// <param name="Level">the level of the Engineer - from Beginner to Expert</param>
public record Engineer
(
    int Id,
    double Cost,
    string Name,
    string Email,
    DO.EngineerExperience Level
)
{ 
    public Engineer() : this(0, 0, "", "", DO.EngineerExperience.Beginner) { }// empty ctor for Engineer
}

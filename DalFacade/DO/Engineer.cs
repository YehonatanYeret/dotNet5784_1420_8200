namespace DO;

/// <summary>
/// Represents an Engineer entity for the database, representing an engineer in the project with all the relevant data.
/// </summary>
/// <param name="Id">The Id of the Engineer.</param>
/// <param name="Cost">How much the Engineer charges per hour.</param>
/// <param name="Name">The name of the Engineer.</param>
/// <param name="Email">The email of the Engineer.</param>
/// <param name="Level">The level of the Engineer - from Beginner to Expert.</param>
public record Engineer
(
    int Id,
    double Cost,
    string Name,
    string Email,
    DO.EngineerExperience Level
)
{
    public Engineer() : this(0, 0, "", "", DO.EngineerExperience.Beginner) { }
}

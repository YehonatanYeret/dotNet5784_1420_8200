namespace DO;

/// <summary>
/// Represents an Engineer entity for the database, representing an engineer in the project with all the relevant data.
/// </summary>
/// <param name="Id">The Id of the Engineer.</param>
/// <param name="Cost">How much the Engineer charges per hour.</param>
/// <param name="Name">The name of the Engineer.</param>
/// <param name="Email">The email of the Engineer.</param>
/// <param name="Level">The level of the Engineer - from Beginner to Expert.</param>
/// <param name="Image">The image of the Engineer.</param>
public record Engineer
(
    int Id,
    double Cost,
    string Name,
    string Email,
    DO.EngineerExperience Level,
    string? Image=null
)
{
    public Engineer() : this(Id: 0, Cost: 0, Name: "",Email: "",Level: DO.EngineerExperience.Beginner) { }

    public bool ShouldSerializeImage() { return !string.IsNullOrEmpty(Image); }
}

namespace DO;

/// <summary>
///  Represents a user entity in the database with all the relevant data.
/// </summary>
/// <param name="Type">Type of the user (taken from DO.UserType) </param>
/// <param name="Name">User name </param>
/// <param name="Password">User password </param>
/// <param name="Email">User email </param>
public record User
(
    UserType Type,
    string Name,
    string Password,
    string Email
)
{
    /// <summary>
    /// Default constructor for the user entity.
    /// </summary>
    public User() : this(Type: UserType.engineer, Name: "", Password: "", Email: "") { }
}


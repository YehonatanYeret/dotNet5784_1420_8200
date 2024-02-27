namespace BO;

/// <summary>
/// Represents a user within the system.
/// </summary>
public class User
{
    /// <summary>
    /// Gets or sets the type of the user.
    /// </summary>
    /// <remarks>
    /// The user type defines the role or category of the user within the system.
    /// </remarks>
    public UserType Type { get; set; }

    /// <summary>
    /// Gets or sets the name of the user.
    /// </summary>
    /// <remarks>
    /// The user's name is a string identifier associated with the user.
    /// </remarks>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the password of the user.
    /// </summary>
    /// <remarks>
    /// The user's password is used for authentication and should be kept secure.
    /// </remarks>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the email address of the user.
    /// </summary>
    /// <remarks>
    /// The user's email is a unique identifier and is used for communication and account recovery.
    /// </remarks>
    public string Email { get; set; }
}

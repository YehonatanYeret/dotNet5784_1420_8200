namespace BlApi;

/// <summary>
/// User interface
/// </summary>
public interface IUser
{
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="user">The user object containing information for creation.</param>
    /// <returns>A string identifier for the created user.</returns>
    public string Create(BO.User user);

    /// <summary>
    /// Reads a user based on their email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>The user object corresponding to the provided email.</returns>
    public BO.User Read(string email);

    /// <summary>
    /// Reads all users based on an optional filter.
    /// </summary>
    /// <param name="filter">An optional filter function to apply on users.</param>
    /// <returns>An IEnumerable of users satisfying the optional filter.</returns>
    public IEnumerable<BO.User> ReadAll(Func<BO.User, bool>? filter = null);

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="user">The user object containing updated information.</param>
    public void Update(BO.User user);

    /// <summary>
    /// Deletes a user based on their email address.
    /// </summary>
    /// <param name="email">The email address of the user to delete.</param>
    public void Delete(string email);

    /// <summary>
    /// Resets all users in the system and creates a new default admin user.
    /// </summary>
    public void Reset();
}

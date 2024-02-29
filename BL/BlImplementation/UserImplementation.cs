namespace BlImplementation;

internal class UserImplementation : BlApi.IUser
{
    private DalApi.IDal _dal = DalApi.Factory.Get;


    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="user">The user object to create.</param>
    /// <returns>The Email of the newly created user.</returns>
    public string Create(BO.User user)
    {
        //check if the values is correct
        checkUser(user);

        // Create a new DO.User object from the BO.User object.
        DO.User? doUser = new DO.User
        (
            Type: (DO.UserType)user.Type,
            Password: user.Password,
            Name: user.Name,
            Email: user.Email
        );

        try
        {
            return _dal.User.Create(doUser);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BLAlreadyExistsException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Retrieves a specific user by their Email.
    /// </summary>
    /// <param name="email">The Email of the user to retrieve.</param>
    /// <returns>The user with the specified Email.</returns>
    public BO.User Read(string email)
    {

        DO.User? user = _dal.User.Read(email);

        // If the user does not exist, throw a BLDoesNotExistException.
        if (user is null)
            throw new BO.BLDoesNotExistException($"User with Email {email} does not exist");

        // Otherwise, create a new BO.User object from the DO.User object.

        //create the BO.User object 
        BO.User? boUser = new BO.User()
        {
            Type = (BO.UserType)user.Type,
            Name = user.Name,
            Password = user.Password,
            Email = user.Email,
        };

        return boUser;
    }

    /// <summary>
    /// Retrieves all users based on an optional filter.
    /// </summary>
    /// <param name="filter">An optional filter function to apply to the users.</param>
    /// <returns>An IEnumerable of users that satisfy the provided filter.</returns>
    public IEnumerable<BO.User> ReadAll(Func<BO.User, bool>? filter = null)
    {
        //if there is no filter, return all the users
        if (filter is null)
            filter = (u) => true;

        //get all the users that return true with filter from the dal
        return (from u in _dal.User.ReadAll()
                where filter(new BO.User
                {
                    Type = (BO.UserType)u.Type,
                    Name = u.Name,
                    Password = u.Password,
                    Email = u.Email
                })
                select new BO.User
                {
                    Type = (BO.UserType)u.Type,
                    Name = u.Name,
                    Password = u.Password,
                    Email = u.Email
                });
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <param name="user">The user object with updated information.</param>
    public void Update(BO.User user)
    {
        //check the values of the engineer
        checkUser(user);

        try
        {
            _dal.User.Update(new DO.User
            (
                Type: (DO.UserType)user.Type,
                Name: user.Name,
                Password: user.Password,
                Email: user.Email
            ));
        }
        // If the user does not exist, throw a BLDoesNotExistException.
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException(ex.Message, ex);
        }

    }

    /// <summary>
    /// Deletes an existing user.
    /// </summary>
    /// <param name="email">The Email of the user that need to delete.</param>
    public void Delete(string email)
    {
        try
        {
            _dal.User.Delete(email);
            DO.Engineer? engineer = _dal.Engineer.Read(eng => eng.Email == email);

            //if the user is an engineer, delete the engineer
            if (engineer is not null)
                _dal.Engineer.Delete(engineer.Id);
        }
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException(ex.Message, ex);
        }
    }

    /// <summary>
    /// Resets all users in the system and initializes the list with the default admin user.
    /// </summary>
    public void Reset()
    {
        _dal.User.DeleteAll();
    }

    /// <summary>
    /// check if the values of the user is correct
    /// </summary>
    /// <param name="user">the user that need to check</param>
    public void checkUser(BO.User user)
    {
        //check if the name is not empty
        if (string.IsNullOrEmpty(user.Name))
            throw new BO.BLValueIsNotCorrectException($"User name cannot be empty: {user.Name}");

        //check if the email is valid
        var mail = new System.ComponentModel.DataAnnotations.EmailAddressAttribute();
        if (user.Email is null || !mail.IsValid(user.Email))
            throw new BO.BLValueIsNotCorrectException($"User email is not valid: {user.Email}");

        //check if the password is valid
        if (string.IsNullOrEmpty(user.Password))
            throw new BO.BLValueIsNotCorrectException($"User password is not valid: {user.Password}");

        if (user.Password.Length < 8 || user.Password.Length > 14)
            throw new BO.BLValueIsNotCorrectException($"User password most be between 8-14 characters: {user.Password}");

        if (!user.Password.Any(char.IsUpper) || !user.Password.Any(char.IsLower) || !user.Password.Any(char.IsDigit))
            throw new BO.BLValueIsNotCorrectException($"User password must contain at least one uppercase letter, one lowercase letter and one digit: {user.Password}");
    }
}

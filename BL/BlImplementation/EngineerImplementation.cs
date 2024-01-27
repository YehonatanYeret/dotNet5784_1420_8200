namespace BlImplementation;
using BlApi;
using System.Text.RegularExpressions;

internal class EngineerImplementation : IEngineer
{
    private DalApi.IDal _dal = DalApi.Factory.Get;

    /// <summary>
    /// Creates a new engineer.
    /// </summary>
    /// <param name="engineer">The engineer object to create.</param>
    public int Create(BO.Engineer engineer)
    {
        //check if the values is correct
        checkEngineer(engineer);

        // Create a new DO.Engineer object from the BO.Engineer object.
        DO.Engineer? doEngineer = new DO.Engineer
        (
            Id: engineer.Id,
            Cost: engineer.Cost,
            Name: engineer.Name,
            Email: engineer.Email,
            Level: (DO.EngineerExperience)engineer.Level
        );

        try
        {
            int id = _dal.Engineer.Create(doEngineer);
            return id;
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BLAlreadyExistsException($"Engineer with ID {engineer.Id} already exists", ex);
        }

    }

    /// <summary>
    /// Retrieves a specific engineer by their ID.
    /// </summary>
    /// <param name="id">The ID of the engineer to retrieve.</param>
    /// <returns>The engineer with the specified ID.</returns>
    public BO.Engineer? Read(int id)
    {

        DO.Engineer? engineer = _dal.Engineer.Read(id);

        // If the engineer does not exist, throw a BLDoesNotExistException.
        if (engineer is null)
            throw new BO.BLDoesNotExistException($"Engineer with ID {id} does not exist");

        // Otherwise, create a new BO.Engineer object from the DO.Engineer object.

        //get the task of the engineer if exist
        BO.TaskInEngineer? task = (from t in _dal.Task.ReadAll()
                                   where t.EngineerId == id
                                   select new BO.TaskInEngineer
                                   {
                                       Id = t.Id,
                                       Alias = t.Alias
                                   }).FirstOrDefault();


        BO.Engineer? boEngineer = new BO.Engineer()
        {
            Id = id,
            Cost = engineer.Cost,
            Name = engineer.Name,
            Email = engineer.Email,
            Level = (BO.EngineerExperience)engineer.Level,
            Task = task
        };

        return boEngineer;
    }

    /// <summary>
    /// Retrieves all engineers based on an optional filter.
    /// </summary>
    /// <param name="filter">An optional filter function to apply to the engineers.</param>
    /// <returns>An IEnumerable of engineers that satisfy the provided filter.</returns>
    /// 
    public IEnumerable<BO.Engineer> ReadAll(Func<BO.Engineer, bool>? filter = null) 
    {
        //if there is no filter, return all the engineers
        if(filter is null)
            filter = (e) => true;

        //get all the engineers that return true with filter from the dal
        return (from e in _dal.Engineer.ReadAll()
                where filter(new BO.Engineer
                {
                    Id = e.Id,
                    Cost = e.Cost,
                    Name = e.Name,
                    Email = e.Email,
                    Level = (BO.EngineerExperience)e.Level
                })
                select new BO.Engineer
                {
                    Id = e.Id,
                    Cost = e.Cost,
                    Name = e.Name,
                    Email = e.Email,
                    Level = (BO.EngineerExperience)e.Level
                });
    }

    /// <summary>
    /// Updates an existing engineer.
    /// </summary>
    /// <param name="engineer">The engineer object with updated information.</param>
    public void Update(BO.Engineer engineer)
    {
        //check the values of the engineer
        checkEngineer(engineer);

        if (Read(engineer.Id) != null && engineer.Level < Read(engineer.Id)!.Level)
            throw new BO.BLValueIsNotCorrectException($"Engineer level cannot be lower than the current level: {engineer.Level}");

        try
        {
            _dal.Engineer.Update(new DO.Engineer
            (
            Id: engineer.Id,
            Cost: engineer.Cost,
            Name: engineer.Name,
            Email: engineer.Email,
            Level: (DO.EngineerExperience)engineer.Level
            ));

            //update the task of the engineer if exist
            if (engineer.Task is not null)
            {
                DO.Task? task = _dal.Task.Read(engineer.Task.Id);

                // If the task does exist, update the task with the new engineer ID.
                if (task is not null)
                    _dal.Task.Update(task with { EngineerId = engineer.Id });
            }
        }
        // If the engineer does not exist, throw a BLDoesNotExistException.
        catch (DO.DalDoesNotExistException ex)
        {
            throw new BO.BLDoesNotExistException($"Engineer with ID {engineer.Id} does not exist", ex);
        }
    }

    /// <summary>
    /// Deletes an existing engineer.
    /// </summary>
    /// <param name="engineer">The engineer object to delete.</param>
    public void Delete(BO.Engineer engineer) { }


    void checkEngineer(BO.Engineer engineer)
    {
        if (engineer.Id < 0)
            throw new BO.BLValueIsNotCorrectException($"Engineer ID cannot be negative: {engineer.Id}");

        if (string.IsNullOrEmpty(engineer.Name))
            throw new BO.BLValueIsNotCorrectException($"Engineer name cannot be empty: {engineer.Name}");

        if (engineer.Cost < 0)
            throw new BO.BLValueIsNotCorrectException($"Engineer cost cannot be negative: {engineer.Cost}");

        string regex = @"^[^@\s]+@[^@\s]+\.(com|net|org|gov)$";

        if (!Regex.IsMatch(engineer.Email, regex, RegexOptions.IgnoreCase))
            throw new BO.BLValueIsNotCorrectException($"Engineer email is not valid: {engineer.Email}");
    }
}

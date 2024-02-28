namespace Dal;
using DalApi;

/// <summary>
/// Represents a Data Access Layer (DAL) list providing access to different entities.
/// </summary>
sealed internal class DalList : IDal
{
    private DalList() { }

    private static readonly Lazy<DalList> lazy = new Lazy<DalList>(() => new DalList());
    public static DalList Instance { get { return lazy.Value; } }

    /// <summary>
    /// Gets an instance of the <see cref="IEngineer"/> interface for accessing engineer-related data.
    /// </summary>
    public IEngineer Engineer => new EngineerImplementation();

    /// <summary>
    /// Gets an instance of the <see cref="ITask"/> interface for accessing task-related data.
    /// </summary>
    public ITask Task => new TaskImplementation();

    /// <summary>
    /// Gets an instance of the <see cref="IDependency"/> interface for accessing dependency-related data.
    /// </summary>
    public IDependency Dependency => new DependencyImplementation();

    /// <summary>
    /// Gets an instance of the <see cref="IUser"/> interface for accessing user-related data.
    /// </summary>
    public IClock Clock => new ClockImplementation();

    /// <summary>
    /// Gets an instance of the <see cref="IClock"/> interface for accessing clock-related data.
    /// </summary>
    public IUser User => new UserImplementation();
}


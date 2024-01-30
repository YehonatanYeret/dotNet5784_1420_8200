namespace Dal;
using DalApi;

sealed internal class DalXml : IDal
{
    private DalXml() { }

    private static readonly Lazy<DalXml> lazy = new Lazy<DalXml>(() => new DalXml());
    public static DalXml Instance {  get { return lazy.Value; } }

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
    public IDependency Dependency => new DepenencyImplemention();
}

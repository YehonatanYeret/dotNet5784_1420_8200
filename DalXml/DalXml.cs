namespace Dal;
using DalApi;

sealed internal class DalXml : IDal
{
    private DalXml() { }
    public static IDal Insatance { get; } = new DalXml();

    public IEngineer Engineer =>  new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DepenencyImplemention();
}

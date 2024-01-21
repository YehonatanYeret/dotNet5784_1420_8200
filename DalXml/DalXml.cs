namespace Dal;
using DalApi;

public class DalXml : IDal
{
    public IEngineer Engineer =>  new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DepenencyImplemention();
}

namespace Dal;
using DalApi;
using System.Diagnostics;

sealed internal class DalXml : IDal
{
    private DalXml() { }

    public static IDal Instance { get; } = new DalXml();
    //public static Lazy<IDal> Instance { get; } = new Lazy<IDal>(()=>new DalXml());

    public IEngineer Engineer =>  new EngineerImplementation();

    public ITask Task => new TaskImplementation();

    public IDependency Dependency => new DepenencyImplemention();
}

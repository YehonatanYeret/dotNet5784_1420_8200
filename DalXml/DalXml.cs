using DalApi;

namespace Dal
{
    public class DalXml : IDal
    {
        public IEngineer Engineer =>  new EngineerImplementation();

        public ITask Task => new TaskImplementation();

        public IDependency Dependency => new DepenencyImplemention();
    }
}

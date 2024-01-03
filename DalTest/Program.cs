namespace DalTest;
using DalApi;
using Dal;

internal class Program
{
    //new implementation of IDal
    private static IDependency? s_dalDependency = new DependencyImplementation();//stage 1
    private static ITask? s_dalTask = new TaskImplementation();//stage 1
    private static IEngineer? s_dalEngineer = new EngineerImplementation();//stage 1
    static void Main(string[] args)
    {
        Initialization.DO(s_dalTask, s_dalEngineer, s_dalDependency);


       
    }
}

namespace DalApi;

/// <summary>
/// creates an interface for the data
/// contain an ITask, Iengneer, and IDependency that inplement the data operations
/// </summary>
public interface IDal
{
    IEngineer Engineer { get; }
    ITask Task { get; }
    IDependency Dependency { get; }
    IClock Clock { get;} 
}

namespace BlApi;

/// <summary>
///  Task interface
/// </summary>
public interface ITask
{
    public void Create(BO.Task task);
    public BO.Task Read(int id);
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null);
    public void Update(BO.Task task);
    public void Delete(int id);
}
using BO;

namespace BlApi;

/// <summary>
///  Task interface
/// </summary>
public interface ITask
{
    public int Create(BO.Task task);
    public BO.Task Read(int id);
    public IEnumerable<BO.Task> ReadAll(Func<BO.Task, bool>? filter = null);
    public void Update(BO.Task task);
    public void UpdateScheduledDate(int id, DateTime time);
    public void Delete(int id);
    public DateTime CalculateClosestStartDate(int id, DateTime startProject);
    public void updateDates(int id, TimeSpan? required, DateTime? deadlineDate);
    public void ChangeStatusOfTask(int id, Status status);
}
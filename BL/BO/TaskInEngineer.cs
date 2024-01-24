namespace BO;

public class TaskInEngineer
{
    public int Id { get; init; } // Primary Key, References BO.Task.Id
    public string Alias { get; set; } // Not Null
}

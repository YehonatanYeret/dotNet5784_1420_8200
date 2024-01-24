namespace BO;

public class TaskInList
{
    public int Id { get; init; } // Primary Key, References BO.Task.Id
    public string Description { get; set; } // Not Null
    public string Alias { get; set; } // Not Null
    public Status Status { get; set; } // Calculated
}


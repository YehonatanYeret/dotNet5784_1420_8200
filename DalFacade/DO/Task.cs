namespace DO;

/// <summary>
/// task entitiy for the database. represents a task in the project with all the relevant data
/// </summary>
/// <param name="Id">unique id for each task</param>
/// <param name="Alias">an alias for each task</param>
/// <param name="Description"></param>
/// <param name="CreatedAtDate">the time when the task have been created</param>
/// <param name="IsMileStone"> does the task is a milestone</param>
/// <param name="ScheduledDate">the planned start date</param>
/// <param name="StartDate">the real start date</param>
/// <param name="RequiredEffortTime">how many days needed for the task</param>
/// <param name="DeadlineDate">the latest complete date</param>
/// <param name="CompleteDate">real completion date</param>
/// <param name="Deliverables">description of deliverables</param>
/// <param name="Remarks"></param>
/// <param name="EngineerId">the id of the engineer who work on the project</param>
/// <param name="Copmlexity">the complexity of the task. taken from DO.EngineerExperience</param>
public record Task
(
    int Id,
    string Alias,
    string Description,
    DateTime CreatedAtDate,
    bool IsMileStone = false,
    DateTime? ScheduledDate = null,
    DateTime? StartDate = null,
    TimeSpan? RequiredEffortTime = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null,
    int? EngineerId = null,
    EngineerExperience? Copmlexity = null
)
{
    public Task() : this(0, "", "", DateTime.Now) { }// empty ctor for task
}


namespace DO;

/// <summary>
/// Represents a task entity in the database with all the relevant data.
/// </summary>
/// <param name="Id">Unique id for each task</param>
/// <param name="Alias">An alias for each task</param>
/// <param name="Description">Task description</param>
/// <param name="CreatedAtDate">Time when the task has been created</param>
/// <param name="IsActive">Indicates if the task is active</param>
/// <param name="ScheduledDate">Planned start date</param>
/// <param name="StartDate">Real start date</param>
/// <param name="RequiredEffortTime">How many days needed for the task</param>
/// <param name="DeadlineDate">Latest completion date</param>
/// <param name="CompleteDate">Real completion date</param>
/// <param name="Deliverables">Description of deliverables</param>
/// <param name="Remarks">Remarks</param>
/// <param name="EngineerId">Id of the engineer who works on the project</param>
/// <param name="Complexity">Complexity of the task (taken from DO.EngineerExperience)</param>
public record Task
(
    int Id,
    string Alias,
    string Description,
    DateTime CreatedAtDate,
    bool IsActive = true,
    DateTime? ScheduledDate = null,
    DateTime? StartDate = null,
    TimeSpan? RequiredEffortTime = null,
    DateTime? DeadlineDate = null,
    DateTime? CompleteDate = null,
    string? Deliverables = null,
    string? Remarks = null,
    int? EngineerId = null,
    EngineerExperience? Complexity = null
)
{
    public Task() : this(Id: 0, Alias: "", Description: "", CreatedAtDate: DateTime.Now) { }

    // functions that indicate whether to serialize a property or not so the XML file will not contain empty tags:
    public bool ShouldSerializeScheduledDate() { return ScheduledDate.HasValue; }
    public bool ShouldSerializeStartDate() { return StartDate.HasValue; }
    public bool ShouldSerializeRequiredEffortTime() { return RequiredEffortTime.HasValue; }
    public bool ShouldSerializeDeadlineDate() { return DeadlineDate.HasValue; }
    public bool ShouldSerializeCompleteDate() { return CompleteDate.HasValue; }
    public bool ShouldSerializeDeliverables() { return !string.IsNullOrEmpty(Deliverables); }
    public bool ShouldSerializeRemarks() { return !string.IsNullOrEmpty(Remarks); }
    public bool ShouldSerializeEngineerId() { return EngineerId.HasValue; }
    public bool ShouldSerializeComplexity() { return Complexity.HasValue; }
}

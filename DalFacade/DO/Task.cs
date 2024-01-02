namespace DO;

public record Task
(
    int Id,
    string Alias,
    string Description,
    bool IsMileStone,
    DateTime CreatedAtDate,
    DateTime ScheduledDate,
    DateTime StartDate,
    TimeSpan RequiredEffortTime,
    DateTime DeadlineDate,
    DateTime CompleteDate,
    string Deliverables,
    string Remarks,
    int EngineerId,
    EngineerExperience Copmlexity
);

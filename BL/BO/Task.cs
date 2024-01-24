namespace BO;

public class Task
{
    public int Id { get; init; } // Primary Key
    public string Description { get; set; } // Not Null
    public string Alias { get; set; } // Not Null
    public DateTime CreatedAtDate { get; set; } // Not Null, Date when the task was added to the system
    public Status Status { get; set; } // Calculated
    public List<TaskInList> Dependencies { get; set; } // Relevant only before the schedule is built
    public MilestoneInTask Milestone { get; set; } // Calculated when building schedule, populated if there is a milestone in dependency, relevant only after the schedule is built
    public TimeSpan RequiredEffortTime { get; set; } // How many men-days needed for the task
    public DateTime StartDate { get; set; } // The real start date
    public DateTime ScheduledDate { get; set; } // The planned start date
    public DateTime ForecastDate { get; set; } // Calculated planned completion date
    public DateTime DeadlineDate { get; set; } // The latest complete date
    public DateTime CompleteDate { get; set; } // Real completion date
    public string Deliverables { get; set; } // Description of deliverables for MS completion
    public string Remarks { get; set; } // Free remarks from project meetings
    public EngineerInTask Engineer { get; set; } // Reference to BOEIT.Id
    public EngineerExperience Copmlexity { get; set; } // Minimum experience for an engineer to be assigned
}
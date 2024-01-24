namespace BO
{
    /// <summary>
    /// Represents a task within the system.
    /// </summary>
    public class Task
    {
        /// <summary>
        /// Gets or initializes the unique identifier for the task.
        /// </summary>
        /// <remarks>
        /// This property serves as the primary key.
        /// </remarks>
        public int Id { get; init; }

        /// <summary>
        /// Gets or sets the description of the task.
        /// </summary>
        /// <remarks>
        /// The description cannot be null.
        /// </remarks>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the alias of the task.
        /// </summary>
        /// <remarks>
        /// The alias cannot be null.
        /// </remarks>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the date when the task was added to the system.
        /// </summary>
        /// <remarks>
        /// The CreatedAtDate cannot be null.
        /// </remarks>
        public DateTime CreatedAtDate { get; init; }

        /// <summary>
        /// Gets or sets the calculated status of the task.
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Gets or sets the list of dependencies for the task.
        /// </summary>
        /// <remarks>
        /// Relevant only before the schedule is built.
        /// </remarks>
        public List<TaskInList> Dependencies { get; set; }

        /// <summary>
        /// Gets or sets the calculated milestone for the task.
        /// </summary>
        /// <remarks>
        /// Calculated when building the schedule and populated if there is a milestone in the dependency.
        /// Relevant only after the schedule is built.
        /// </remarks>
        public MilestoneInTask Milestone { get; set; }

        /// <summary>
        /// Gets or sets the required effort time for the task.
        /// </summary>
        /// <remarks>
        /// Represents how many men-days are needed for the task.
        /// </remarks>
        public TimeSpan RequiredEffortTime { get; set; }

        /// <summary>
        /// Gets or sets the real start date of the task.
        /// </summary>
        public DateTime StartDate { get; init; }

        /// <summary>
        /// Gets or sets the planned start date of the task.
        /// </summary>
        public DateTime ScheduledDate { get; set; }

        /// <summary>
        /// Gets or sets the calculated planned completion date of the task.
        /// </summary>
        public DateTime ForecastDate { get; set; }

        /// <summary>
        /// Gets or sets the latest complete date of the task.
        /// </summary>
        public DateTime DeadlineDate { get; set; }

        /// <summary>
        /// Gets or sets the real completion date of the task.
        /// </summary>
        public DateTime CompleteDate { get; init; }

        /// <summary>
        /// Gets or sets the description of deliverables for milestone completion.
        /// </summary>
        public string Deliverables { get; set; }

        /// <summary>
        /// Gets or sets free remarks from project meetings.
        /// </summary>
        public string Remarks { get; set; }

        /// <summary>
        /// Gets or sets the reference to the engineer associated with the task.
        /// </summary>
        /// <remarks>
        /// References BO.EngineerInTask.Id.
        /// </remarks>
        public EngineerInTask Engineer { get; set; }

        /// <summary>
        /// Gets or sets the minimum experience required for an engineer to be assigned to the task.
        /// </summary>
        public EngineerExperience Copmlexity { get; set; }
    }
}

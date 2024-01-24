namespace BO
{
    /// <summary>
    /// Represents a task in the list of the dependencies of another task.
    /// </summary>
    public class TaskInList
    {
        /// <summary>
        /// Gets or initializes the unique identifier for the task.
        /// </summary>
        /// <remarks>
        /// This property serves as the primary key and references BO.Task.Id.
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
        /// Gets or sets the status of the task.
        /// </summary>
        /// <remarks>
        /// The status is calculated.
        /// </remarks>
        public Status? Status { get; set; }
    }
}

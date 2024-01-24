namespace BO
{
    /// <summary>
    /// Represents a task associated with an engineer.
    /// </summary>
    public class TaskInEngineer
    {
        /// <summary>
        /// Gets or initializes the unique identifier for the task.
        /// </summary>
        /// <remarks>
        /// This property serves as the primary key and references BO.Task.Id.
        /// </remarks>
        public int Id { get; init; }

        /// <summary>
        /// Gets or sets the alias of the task.
        /// </summary>
        /// <remarks>
        /// The alias cannot be null.
        /// </remarks>
        public string Alias { get; set; }
    }
}
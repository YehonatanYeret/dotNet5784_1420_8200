namespace BlApi
{
    /// <summary>
    /// Interface representing a clock for managing project time-related operations.
    /// </summary>
    public interface IClock
    {
        /// <summary>
        /// Sets the start project date.
        /// </summary>
        /// <param name="startProject">The start project date to be set.</param>
        /// <returns>A nullable DateTime representing the set start project date.</returns>
        public DateTime? SetStartProject(DateTime startProject);

        /// <summary>
        /// Gets the start project date.
        /// </summary>
        /// <returns>A nullable DateTime representing the start project date.</returns>
        public DateTime? GetStartProject();

        /// <summary>
        /// Sets the end project date.
        /// </summary>
        /// <param name="endProject">The end project date to be set.</param>
        /// <returns>A nullable DateTime representing the set end project date.</returns>
        public DateTime? SetEndProject(DateTime endProject);

        /// <summary>
        /// Gets the end project date.
        /// </summary>
        /// <returns>A nullable DateTime representing the end project date.</returns>
        public DateTime? GetEndProject();

        /// <summary>
        /// Gets the status of the project based on the current date and project dates.
        /// </summary>
        /// <returns>A ProjectStatus enum value indicating the project status.</returns>
        public BO.ProjectStatus GetProjectStatus();
    }
}

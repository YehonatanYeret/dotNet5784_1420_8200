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
        public void SetStartProject(DateTime startProject);

        /// <summary>
        /// Gets the start project date.
        /// </summary>
        /// <returns>A nullable DateTime representing the start project date.</returns>
        public DateTime? GetStartProject();

        /// <summary>
        /// Resets the project timeline.
        /// </summary>
        public void Reset();

        /// <summary>
        /// Gets the status of the project based on the current date and project dates.
        /// </summary>
        /// <returns>A ProjectStatus enum value indicating the project status.</returns>
        public BO.ProjectStatus GetProjectStatus();

        /// <summary>
        ///  sets the current time.
        /// </summary>
        /// <param name="currentTime"></param>
        public void SetCurrentTime(DateTime currentTime);

        /// <summary>
        /// Gets the current time.
        /// </summary>
        /// <returns></returns>
        public DateTime? GetCurrentTime();
    }
}

namespace BlImplementation
{
    using BlApi;

    /// <summary>
    /// Implementation of the business logic interface (IBl).
    /// </summary>
    internal class Bl : IBl
    {
        /// <summary>
        /// Gets an instance of the Engineer interface.
        /// </summary>
        public IEngineer Engineer => new EngineerImplementation();

        /// <summary>
        /// Gets an instance of the Task interface.
        /// </summary>
        public ITask Task => new TaskImplementation();

        /// <summary>
        /// Gets an instance of the Clock interface.
        /// </summary>
        public IClock Clock => new ClockImplementation();
    }
}
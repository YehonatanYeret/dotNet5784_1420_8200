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

        /// <summary>
        /// Gets an instance of the User interface.
        /// </summary>
        public IUser User => new UserImplementation();

        /// <summary>
        /// initialize the db
        /// </summary>
        public void InitializeDB() => DalTest.Initialization.Do();

        /// <summary>
        /// delete all the db
        /// </summary>
        public void ResetDB()=>DalTest.Initialization.Reset();
    }
}
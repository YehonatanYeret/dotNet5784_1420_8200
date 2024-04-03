namespace BlImplementation
{
    using BlApi;
    using System;

    /// <summary>
    /// Implementation of the business logic interface (IBl).
    /// </summary>
    internal class Bl : IBl
    {
        /// <summary>
        /// Gets an instance of the Engineer interface.
        /// </summary>
        public IEngineer Engineer => new EngineerImplementation(this);

        /// <summary>
        /// Gets an instance of the Task interface.
        /// </summary>
        public ITask Task => new TaskImplementation(this);

        /// <summary>
        /// Gets an instance of the Clock interface.
        /// </summary>
        public IClock Clock => new ClockImplementation(this);

        /// <summary>
        /// Gets an instance of the User interface.
        /// </summary>
        public IUser User => new UserImplementation();

        private static DateTime s_Clock = DateTime.Now.Date;

        public DateTime Time { get { return s_Clock; } private set { s_Clock = value; } }

        public void AddDay()
        {
            Time = Time.AddDays(1);
        }

        public void AddHour()
        {
            Time = Time.AddHours(1);
        }

        /// <summary>
        /// initialize the db
        /// </summary>
        public void InitializeDB() => DalTest.Initialization.Do();

        /// <summary>
        /// delete all the db
        /// </summary>
        public void ResetDB()
        {
            DalTest.Initialization.Reset();
        }

        public void ResetTime()
        {
            Time = DateTime.Now.Date;
        }

        public void Init()
        {
            Time = Factory.Get().Clock.GetCurrentTime() ?? DateTime.Now.Date;
        }
    }
}
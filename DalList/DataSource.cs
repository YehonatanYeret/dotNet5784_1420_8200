namespace Dal;

internal static class DataSource
{
    /// <summary>
    /// the class config contains trios of start id, next id, and a function Next id
    /// the start id is a const integer represent the start of the runnning key
    /// the next id will equal to the start id, but it will grow over time
    /// the Next id is a function to return the next id and grow it
    /// </summary>
    internal static class Config
    {
        //running id for the task
        internal const int StartTaskId = 1;
        private static int nextTaskId = StartTaskId;
        internal static int NextTaskId { get => nextTaskId++; }
        internal static void ResetTaskId() => nextTaskId = StartTaskId;

        //running id for the engineers
        internal const int StartDependencyId = 1;
        private static int nextDependencyId = StartDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
        internal static void ResetDependencyId() => nextDependencyId = StartDependencyId;

        //the start date of the project
        internal static DateTime? StartDate;

        //the end date of the project
        internal static DateTime? CurrentTime;
    }

    //create a lists of the data
    internal static List<DO.Task> Tasks { get; } = new();
    internal static List<DO.Engineer> Engineers { get; } = new();
    internal static List<DO.Dependency> Dependencies { get; } = new();
    internal static List<DO.User> Users { get; } = new();
}
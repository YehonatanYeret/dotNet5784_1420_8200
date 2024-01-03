namespace DalTest;
using DalApi;
using Dal;
using DO;

internal class Program
{
    //new implementation of IDal
    private static IDependency? s_dalDependency = new DependencyImplementation();//stage 1
    private static ITask? s_dalTask = new TaskImplementation();//stage 1
    private static IEngineer? s_dalEngineer = new EngineerImplementation();//stage 1


    static void CreateTask()
    {
        string alias = Console.ReadLine()?? "";
        string description = Console.ReadLine() ?? "" ;
        DateTime createdAtDate = DateTime.Parse(Console.ReadLine());
        bool isMileStone = bool.Parse(Console.ReadLine());
        DateTime? scheduledDate = DateTime.Parse(Console.ReadLine()!);
        DateTime? startDate = DateTime.Parse(Console.ReadLine()!);
        TimeSpan? requiredEffortTime = TimeSpan.Parse(Console.ReadLine()!);
        DateTime? deadlineDate = DateTime.Parse(Console.ReadLine()!);
        DateTime? completeDate = DateTime.Parse(Console.ReadLine()!);
        string? deliverables = Console.ReadLine();
        string? remarks = Console.ReadLine();
        int? engineerId = int.Parse(Console.ReadLine()!);
        EngineerExperience? copmlexity = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine()!);

        Task task = new(
            0,
            alias,
            description,
            createdAtDate,
            isMileStone,
            scheduledDate,
            startDate,
            requiredEffortTime,
            deadlineDate,
            completeDate,
            deliverables,
            remarks,
            engineerId,
            copmlexity
        );
        s_dalTask.Create(task);
    }

    static void ReadTask()
    {
        int id = int.Parse(Console.ReadLine()!);
        Task? task = s_dalTask.Read(id);
        Console.WriteLine(task);
    }

    static void ReadAllTask()
    {
        List<Task> tasks = s_dalTask.ReadAll();
        foreach (Task task in tasks)
        {
            Console.WriteLine(task);
        }
    }

    static void UpdateTask()
    {
        int id = int.Parse(Console.ReadLine()!);
        string alias = Console.ReadLine() ?? "";
        string description = Console.ReadLine() ?? "";
        DateTime createdAtDate = DateTime.Parse(Console.ReadLine());
        bool isMileStone = bool.Parse(Console.ReadLine());
        DateTime? scheduledDate = DateTime.Parse(Console.ReadLine()!);
        DateTime? startDate = DateTime.Parse(Console.ReadLine()!);
        TimeSpan? requiredEffortTime = TimeSpan.Parse(Console.ReadLine()!);
        DateTime? deadlineDate = DateTime.Parse(Console.ReadLine()!);
        DateTime? completeDate = DateTime.Parse(Console.ReadLine()!);
        string? deliverables = Console.ReadLine();
        string? remarks = Console.ReadLine();
        int? engineerId = int.Parse(Console.ReadLine()!);
        EngineerExperience? copmlexity = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine()!);

        Task task = new(
            id,
            alias,
            description,
            createdAtDate,
            isMileStone,
            scheduledDate,
            startDate,
            requiredEffortTime,
            deadlineDate,
            completeDate,
            deliverables,
            remarks,
            engineerId,
            copmlexity
);
        s_dalTask.Update(task);
    }

    static void DeleteTask()
    {
        int id = int.Parse(Console.ReadLine()!);
        s_dalTask.Delete(id);
    }

    static void Main(string[] args)
    {
        Initialization.DO(s_dalTask, s_dalEngineer, s_dalDependency);


       
    }
}

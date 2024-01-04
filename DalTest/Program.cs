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
    static void ShowMenu()
    {
        int choice;
        do
        {
            Console.WriteLine("Choose an option:\n"+
                "0. Exit \n" +
                "1. task \n" +
                "2. engineer\n" +
                "3. dependency");
            
            if(!int.TryParse(Console.ReadLine(), out choice))// if we entered invalid response 
                choice=10;// set invalid value

            switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                    case 2:
                    case 3:
                        SubMenu(choice);
                        break;
                    default:
                        Console.WriteLine("Invalid input");
                        break;
            }
        } while (choice != 0);
    }

    static void SubMenu(int choice)
    {
        int subChoice;
        do
        {
            Console.WriteLine("Choose an option:\n"+
                "0. Back\n"+
                "1. Create\n"+
                "2. Read\n"+
                "3. ReadAll\n"+
                "4. Update\n"+
                "5. Delete\n");

            if(!int.TryParse(Console.ReadLine(), out subChoice))
                subChoice = 10;
            try
            {
                switch (choice, subChoice)
                {
                    case (1, 1):
                        CreateTask();
                        break;
                    case (1, 2):
                        ReadTask();
                        break;
                    case (1, 3):
                        ReadAllTask();
                        break;
                    case (1, 4):
                        UpdateTask();
                        break;
                    case (1, 5):
                        DeleteTask();
                        break;


                    case (2, 1):
                        CreateEngineer();
                        break;
                    case (2, 2):
                        ReadEngineer();
                        break;
                    case (2, 3):
                        ReadAllEngineer();
                        break;
                    case (2, 4):
                        UpdateEngineer();
                        break;
                    case (2, 5):
                        DeleteEngineer();
                        break;

                    case (3, 1):
                        CreateDependency();
                        break;
                    case (3, 2):
                        ReadDependency();
                        break;
                    case (3, 3):
                        ReadAllDependency();
                        break;
                    case (3, 4):
                        UpdateDependency();
                        break;
                    case (3, 5):
                        DeleteDependency();
                        break;

                    case (1, 0):
                    case (2, 0):
                    case (3, 0):
                        break;

                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }while (subChoice != 0);
    }

    static void CreateTask()
    {
        //get the new values
        string alias = Console.ReadLine()!;
        string description = Console.ReadLine()!;
        DateTime createdAtDate = DateTime.Parse(Console.ReadLine()!);
        bool isMileStone = bool.Parse(Console.ReadLine()?? "false");
        DateTime? scheduledDate = DateTime.Parse(Console.ReadLine());
        DateTime? startDate = DateTime.Parse(Console.ReadLine());
        TimeSpan? requiredEffortTime = TimeSpan.Parse(Console.ReadLine());
        DateTime? deadlineDate = DateTime.Parse(Console.ReadLine());
        DateTime? completeDate = DateTime.Parse(Console.ReadLine());
        string? deliverables = Console.ReadLine();
        string? remarks = Console.ReadLine();
        int? engineerId = int.Parse(Console.ReadLine()!);
        EngineerExperience? copmlexity = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine()!);

        //create the new task
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
        s_dalTask.Create(task);//add the task to the data base
    }
    static void ReadTask()
    {
        int id = int.Parse(Console.ReadLine()!);
        Task? task = s_dalTask.Read(id);//find the task with the id
        Console.WriteLine(task);//print the task
    }
    static void ReadAllTask()
    {
        //get all the tasks
        List<Task> tasks = s_dalTask.ReadAll();
        foreach (Task task in tasks)
        {
            Console.WriteLine(task);//print any task of the tasks
        }
    }
    static void UpdateTask()
    {
        int id = int.Parse(Console.ReadLine()!);

        Task? oldTask = s_dalTask.Read(id);//find the index of the task with the same id
        Console.WriteLine(oldTask);//print the task

        //get the new values
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

        //create the new task
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
        s_dalTask.Update(task);//update the task
    }

    static void DeleteTask()
    {
        int id = int.Parse(Console.ReadLine()!);
        s_dalTask.Delete(id);//delete the task
    }

    private static void CreateEngineer()
    {
        //get the new values
        int id = int.Parse(Console.ReadLine()!);
        double cost = double.Parse(Console.ReadLine()!);
        string name = Console.ReadLine() ?? "";
        string email = Console.ReadLine() ?? "";
        EngineerExperience level = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine()!);

        //create the new engineer
        Engineer engineer = new(
            id,
            cost,
            name,
            email,
            level
        );
        s_dalEngineer.Create(engineer);//add the engineer to the data base
    }
    private static void ReadEngineer()
    {
        //get the id of the engineer
        int id = int.Parse(Console.ReadLine()!);
        Engineer? engineer = s_dalEngineer.Read(id) ;
        Console.WriteLine(engineer);//print the engineer
    }
    private static void ReadAllEngineer()
    {
        //get all the engineers
        List<Engineer> engineers = s_dalEngineer.ReadAll();
        foreach (Engineer engineer in engineers)
        {
            Console.WriteLine(engineer);//print any engineer of the engineers
        }
    }
    private static void UpdateEngineer()
    {
        int id = int.Parse(Console.ReadLine()!);

        Engineer? oldEngineer = s_dalEngineer.Read(id);//find the index of the engineer with the same id
        Console.WriteLine(oldEngineer);//print the engineer

        //get the new values
        double cost = double.Parse(Console.ReadLine()!);
        string name = Console.ReadLine() ?? "";
        string email = Console.ReadLine() ?? "";
        EngineerExperience level = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), Console.ReadLine()!);

        //create the new engineer
        Engineer engineer = new(
            id,
            cost,
            name,
            email,
            level
        );
        s_dalEngineer.Update(engineer);//update the engineer
    }
    private static void DeleteEngineer()
    {
        int id = int.Parse(Console.ReadLine()!);
        s_dalEngineer.Delete(id);//delete the engineer
    }

    private static void CreateDependency()
    {
        //get the new values
        int id = int.Parse(Console.ReadLine()!);
        int taskId = int.Parse(Console.ReadLine()!);
        int dependOnTaskId = int.Parse(Console.ReadLine()!);

        //create the new dependency
        Dependency dependency = new(
            id,
            taskId,
            dependOnTaskId
        );
        s_dalDependency.Create(dependency);//add the dependency to the data base

    }
    private static void ReadDependency()
    {
        int id = int.Parse(Console.ReadLine()!);
        Dependency? dependency = s_dalDependency.Read(id);
        Console.WriteLine(dependency);//print the dependency
    }
    private static void ReadAllDependency()
    {
        //get all the dependencies
        List<Dependency> dependencies = s_dalDependency.ReadAll();
        foreach (Dependency dependency in dependencies)
        {
            Console.WriteLine(dependency);//print any dependency of the dependencies
        }
    }
    private static void UpdateDependency()
    {
        int id = int.Parse(Console.ReadLine()!);

        Dependency? oldDependency = s_dalDependency.Read(id);//find the index of the dependency with the same id
        Console.WriteLine(oldDependency);//print the dependency

        //get the new values
        int taskId = int.Parse(Console.ReadLine()!);
        int dependOnTaskId = int.Parse(Console.ReadLine()!);

        //create the new dependency
        Dependency dependency = new(
            id,
            taskId,
            dependOnTaskId
        );
        s_dalDependency.Update(dependency);//update the dependency
    }
    private static void DeleteDependency()
    {
        int id = int.Parse(Console.ReadLine()!);
        s_dalDependency.Delete(id);//delete the dependency
    }

    static void Main(string[] args)
    {
        Initialization.DO(s_dalTask, s_dalEngineer, s_dalDependency);

        Console.WriteLine("!strat of the program!\n");
        ShowMenu();
    }
}

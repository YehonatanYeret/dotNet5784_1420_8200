namespace DalTest;
using DalApi;
using Dal;
using DO;
using System.Threading.Channels;

internal class Program
{
    //new implementation of IDal
    private static IDependency? s_dalDependency = new DependencyImplementation();//stage 1
    private static ITask? s_dalTask = new TaskImplementation();//stage 1
    private static IEngineer? s_dalEngineer = new EngineerImplementation();//stage 1

    static void ShowMenu()
    {
        int choice = 0 ;
        do
        {
            try
            {
                Console.WriteLine("Choose an option:\n" +
                    "0. Exit \n" +
                    "1. task \n" +
                    "2. engineer\n" +
                    "3. dependency\n"+
                    "4. clear the console");

                if (!int.TryParse(Console.ReadLine(), out choice))
                    throw new Exception("Invalid input");// if we entered invalid response 

                switch (choice)
                {
                    case 0:
                        return;
                    case 1:
                    case 2:
                    case 3:
                        SubMenu(choice);
                        break;
                    case 4:
                        Console.Clear();
                        break;
                    default:
                        throw new Exception("Invalid input");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                choice = 10 ;
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
                throw new Exception("Invalid input");// if we entered invalid response 

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
                        throw new Exception("Invalid input");
                }

        }while (subChoice != 0);
    }

    static void CreateTask()
    {
        DateTime temp1;// to use for the nullable dates
        TimeSpan temp2;
        int temp3;
        Console.WriteLine("Enter the values of the task:");

        //get the new values
        Console.Write("alias:");
        string alias = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(alias))
            throw new ArgumentNullException();
        
        Console.Write("description:");
        string description = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentNullException();

        Console.Write("created at date:");
        DateTime createdAtDate;
        if(!DateTime.TryParse(Console.ReadLine()!, out createdAtDate))
            throw new ArgumentException("Invalid input");

        Console.Write("scheduled date:");
        DateTime? scheduledDate=null;
        if(DateTime.TryParse(Console.ReadLine(), out temp1))
            scheduledDate = temp1;

        Console.Write("start date:");
        DateTime? startDate=null;
        if (DateTime.TryParse(Console.ReadLine() , out temp1))
            startDate = temp1;

        Console.Write("required effort time:");
        TimeSpan? requiredEffortTime = null;
        if(TimeSpan.TryParse(Console.ReadLine(), out temp2))
            requiredEffortTime = temp2;
        

        Console.Write("deadline date:");
        DateTime? deadlineDate = null;
        if (DateTime.TryParse(Console.ReadLine(), out temp1))
            deadlineDate = temp1;

        Console.Write("complete date:");
        DateTime? completeDate = null;
        if (DateTime.TryParse(Console.ReadLine(), out temp1))
            completeDate = temp1;

        Console.Write("deliverables:");
        string? deliverables = Console.ReadLine();

        Console.Write("remarks:");
        string? remarks = Console.ReadLine();

        Console.Write("engineer id:");
        int? engineerId = null;
        if(int.TryParse(Console.ReadLine(), out temp3))
            engineerId=temp3;
        

        Console.Write("copmlexity:");
        EngineerExperience? copmlexity=null;
        if(Enum.TryParse<EngineerExperience>(Console.ReadLine(), out EngineerExperience experience))
        {
            copmlexity = experience;
        }

        //create the new task
        Task task = new(
            0,
            alias,
            description,
            createdAtDate,
            false,
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
        Console.WriteLine("Enter the id of the task:");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new ArgumentException("Invalid input");

        Task? task = s_dalTask.Read(id);//find the task with the id
        Console.WriteLine(task);//print the task
    }

    static void ReadAllTask()
    {
        Console.WriteLine("All of the tasks:");
        //get all the tasks
        List<Task> tasks = s_dalTask.ReadAll();
        foreach (Task task in tasks)
        {
            Console.WriteLine(task);//print any task of the tasks
        }
    }

    static void UpdateTask()
    {
        Console.WriteLine("Enter the id of the task:");
        if (!int.TryParse(Console.ReadLine()!, out int id))
            throw new ArgumentException("Invalid input"); ;

        Task? oldTask = s_dalTask.Read(id);//find the index of the task with the same id
        Console.WriteLine("The old task:");
        Console.WriteLine(oldTask);//print the task

        DateTime temp1;// to use for the nullable dates
        TimeSpan temp2;
        int temp3;
        Console.WriteLine("Enter the values of the task:");

        //get the new values
        Console.Write("alias:");
        string alias = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(alias))
            throw new ArgumentNullException();

        Console.Write("description:");
        string description = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentNullException();

        Console.Write("created at date:");
        DateTime createdAtDate;
        if (!DateTime.TryParse(Console.ReadLine()!, out createdAtDate))
            throw new ArgumentException("Invalid input");

        Console.Write("scheduled date:");
        DateTime? scheduledDate = null;
        if (DateTime.TryParse(Console.ReadLine(), out temp1))
            scheduledDate = temp1;

        Console.Write("start date:");
        DateTime? startDate = null;
        if (DateTime.TryParse(Console.ReadLine(), out temp1))
            startDate = temp1;

        Console.Write("required effort time:");
        TimeSpan? requiredEffortTime = null;
        if (TimeSpan.TryParse(Console.ReadLine(), out temp2))
            requiredEffortTime = temp2;


        Console.Write("deadline date:");
        DateTime? deadlineDate = null;
        if (DateTime.TryParse(Console.ReadLine(), out temp1))
            deadlineDate = temp1;

        Console.Write("complete date:");
        DateTime? completeDate = null;
        if (DateTime.TryParse(Console.ReadLine(), out temp1))
            completeDate = temp1;

        Console.Write("deliverables:");
        string? deliverables = Console.ReadLine();

        Console.Write("remarks:");
        string? remarks = Console.ReadLine();

        Console.Write("engineer id:");
        int? engineerId = null;
        if (int.TryParse(Console.ReadLine(), out temp3))
            engineerId = temp3;


        Console.Write("copmlexity:");
        EngineerExperience? copmlexity = null;
        if (Enum.TryParse<EngineerExperience>(Console.ReadLine(), out EngineerExperience experience))
            copmlexity = experience;

        //create the new task
        Task task = new(
            id,
            alias,
            description,
            createdAtDate,
            false,// told us to put only null for now
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
        Console.WriteLine("Enter the id of the task:");
        if(!int.TryParse(Console.ReadLine()!, out int id))
            throw new ArgumentException("Invalid input"); 
        s_dalTask.Delete(id);//delete the task
    }

    private static void CreateEngineer()
    {
        Console.WriteLine("Enter the values of the engineer:");

        //get the new values
        Console.Write("id:");
        if(!int.TryParse(Console.ReadLine(),out int id))
            throw new ArgumentException("Invalid input"); 

        Console.Write("cost:");
        if(!double.TryParse(Console.ReadLine()! ,out double cost))
            throw new ArgumentException("Invalid input");

        Console.Write("name:");
        string name = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException();

        Console.WriteLine("email:");
        string email = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentNullException();

        Console.WriteLine("level:");
        EngineerExperience level = 0;
        if(!Enum.TryParse<EngineerExperience>(Console.ReadLine()!, out EngineerExperience experience))
            throw new ArgumentException("Invalid input");

        level = experience;
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
        Console.WriteLine("Enter the id of the engineer:");

        //get the id of the engineer
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new ArgumentException("Invalid input");
        Engineer? engineer = s_dalEngineer.Read(id) ;
        Console.WriteLine(engineer);//print the engineer
    }

    private static void ReadAllEngineer()
    {
        Console.WriteLine("All of the engineers:");

        //get all the engineers
        List<Engineer> engineers = s_dalEngineer.ReadAll();
        foreach (Engineer engineer in engineers)
        {
            Console.WriteLine(engineer);//print any engineer of the engineers
        }
    }

    private static void UpdateEngineer()
    {

        Console.WriteLine("Enter the id of the engineer:");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new ArgumentException("Invalid input");

        Engineer? oldEngineer = s_dalEngineer.Read(id);//find the index of the engineer with the same id
        Console.WriteLine("The old engineer:");
        Console.WriteLine(oldEngineer);//print the engineer

        //get the new values
        Console.Write("cost:");
        if (!double.TryParse(Console.ReadLine()!, out double cost))
            throw new ArgumentException("Invalid input");

        Console.Write("name:");
        string name = Console.ReadLine()!;
        if (string.IsNullOrEmpty(name)) throw new ArgumentNullException();

        Console.WriteLine("email:");
        string email = Console.ReadLine()!;
        if (string.IsNullOrEmpty(email)) throw new ArgumentNullException();

        Console.WriteLine("level:");
        EngineerExperience level = 0;
        if(!Enum.TryParse<EngineerExperience>(Console.ReadLine()!, out EngineerExperience experience)) 
            throw new ArgumentException("Invalid input");
        level = experience;
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
        Console.WriteLine("Enter the id of the engineer:");

        //get the id of the engineer
        if(!int.TryParse(Console.ReadLine()!,out int id))
            throw new ArgumentException("Invalid input");
        s_dalEngineer.Delete(id);//delete the engineer
    }

    private static void CreateDependency()
    {
        Console.WriteLine("Enter the values of the dependency:");

        //get the new values
        Console.Write("task id:");
        if (int.TryParse(Console.ReadLine(), out int taskId))
            throw new ArgumentException("Invalid input");

        Console.Write("depend on task id:");
        if (int.TryParse(Console.ReadLine(), out int dependOnTaskId))
            throw new ArgumentException("Invalid input");

        //create the new dependency
        Dependency dependency = new(
            0,
            taskId,
            dependOnTaskId
        );
        s_dalDependency.Create(dependency);//add the dependency to the data base

    }

    private static void ReadDependency()
    {
        //get the id of the dependency
        Console.WriteLine("Enter the id of the dependency:");
        if (!int.TryParse(Console.ReadLine(), out int id))
            throw new ArgumentException("Invalid input");

        Dependency? dependency = s_dalDependency.Read(id);
        Console.WriteLine(dependency);//print the dependency
    }

    private static void ReadAllDependency()
    {
        Console.WriteLine("All of the dependencies:");

        //get all the dependencies
        List<Dependency> dependencies = s_dalDependency.ReadAll();
        foreach (Dependency dependency in dependencies)
        {
            Console.WriteLine(dependency);//print any dependency of the dependencies
        }
    }

    private static void UpdateDependency()
    {
        Console.Write("Enter the id of the dependency:");
        if (!int.TryParse(Console.ReadLine()!, out int id))
            throw new ArgumentException("Invalid input");

        Dependency? oldDependency = s_dalDependency.Read(id);//find the index of the dependency with the same id
        Console.WriteLine("The old dependency:");
        Console.WriteLine(oldDependency);//print the dependency

        Console.WriteLine("Enter the new values of the dependency:");

        //get the new values
        Console.Write("task id:");
        if (int.TryParse(Console.ReadLine(), out int taskId))
            throw new ArgumentException("Invalid input");

        Console.Write("depend on task id:");
        if (int.TryParse(Console.ReadLine(), out int dependOnTaskId))
            throw new ArgumentException("Invalid input");

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
        Console.WriteLine("Enter the id of the dependency:");
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

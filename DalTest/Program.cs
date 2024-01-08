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

    /// <summary>
    /// show the menu and call the sub menu with the choice
    /// </summary>
    static void ShowMenu()
    {
        int choice;
        do
        {
            Console.WriteLine("Choose an option:\n" +
                "0. Exit \n" +
                "1. task \n" +
                "2. engineer\n" +
                "3. dependency\n"+
                "4. clear the console");

            int.TryParse(Console.ReadLine(), out choice);
            switch (choice)
            { 
                case 0:// if we chose 0 we will exit the program
                    return;
                case 1:// if we chose 1 we will go to the task menu
                    SubMenu("Task"); ;
                    break;
                case 2:// if we chose 2 we will go to the engineer menu
                    SubMenu("Engineer");
                    break;
                case 3:// if we chose 3 we will go to the dependency menu
                    SubMenu("Dependency");
                    break;
                default:// if we chose any other number we will throw an exception
                    Console.WriteLine("Invalid input");
                    break;
            }        
        } while (choice != 0);
    }

    /// <summary>
    /// print the sub menu and call the function that the user chose
    /// </summary>
    /// <param name="choice">the choice from the previuos menu</param>
    static void SubMenu(string choice)
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
        try
        {
            int.TryParse(Console.ReadLine(), out subChoice);

            switch (choice, subChoice)
            {
                case ("Task", 1):// task create
                    CreateTask();
                    break;
                case ("Task", 2):// task read
                    ReadTask();
                    break;
                case ("Task", 3):
                    ReadAllTask();// task read all
                    break;
                case ("Task", 4):
                    UpdateTask();// task update
                    break;
                case ("Task", 5):
                    DeleteTask();// task delete
                    break;

                case ("Engineer", 1):/// engineer create
                    CreateEngineer();
                    break;
                case ("Engineer", 2):
                    ReadEngineer();// engineer read
                    break;
                case ("Engineer", 3):
                    ReadAllEngineer();// engineer read all
                    break;
                case ("Engineer", 4):
                    UpdateEngineer();// engineer update
                    break;
                case ("Engineer", 5):
                    DeleteEngineer();// engineer delete
                    break;

                case ("Dependency", 1):
                    CreateDependency();// dependency create
                    break;
                case ("Dependency", 2):
                    ReadDependency();// dependency read
                    break;
                case ("Dependency", 3):
                    ReadAllDependency();// dependency read all
                    break;
                case ("Dependency", 4):
                    UpdateDependency();// dependency update
                    break;
                case ("Dependency", 5):
                    DeleteDependency();// dependency delete
                    break;

                case ("Task", 0):// if we chose 0 we will go back to the main menu
                case ("Engineer", 0):
                case ("Dependency", 0):
                    break;

                default:// if we chose any other number we will throw an exception
                    Console.WriteLine("Invalid input");
                    break;
            }
        }
           catch (Exception ex)// catch all the exceptions
           {
                Console.WriteLine(ex.Message);// print the exception
                subChoice = 10;// initialize the choice to 10 so we will not exit the program
           }

        } while (subChoice != 0);
    }
    /// <summary>
    /// create a new task and add it to the data base
    /// </summary>
    static void CreateTask()
    {
        int id = GetId();
        Task task = TaskCreation(id);//create the task
        s_dalTask.Create(task);//add the task to the data base
    }

    /// <summary>
    /// read a task from the data base and print it
    /// </summary>
    static void ReadTask()
    {
        int id = GetId();

        Task? task = s_dalTask.Read(id);//find the task with the id
        Console.WriteLine(task);//print the task
    }

    /// <summary>
    /// return all the tasks from the data base and print them
    /// </summary>
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

    /// <summary>
    /// update a task from the data base
    /// </summary>
    static void UpdateTask()
    {
        int id = GetId();
        Task? oldTask = s_dalTask.Read(id);//find the index of the task with the same id
        Console.WriteLine("The old task:");
        Console.WriteLine(oldTask);//print the task

        Task task = TaskCreation(id);//create the new task
        s_dalTask.Update(task);//update the task
    }

    /// <summary>
    /// delete a task from the data base
    /// </summary>
    static void DeleteTask()
    {
        int id = GetId();
        s_dalTask.Delete(id);//delete the task
    }

    /// <summary>
    /// create a new engineer and add it to the data base
    /// </summary>
    private static void CreateEngineer()
    {
        int id = GetId();
        Engineer engineer = EngineerCreation(id);//create the engineer
        s_dalEngineer.Create(engineer);//add the engineer to the data base
    }

    /// <summary>
    /// read a engineer from the data base and print it
    /// </summary>
    private static void ReadEngineer()
    {
        Console.WriteLine("Enter the id of the engineer:");

        //get the id of the engineer
        int id = GetId();
        Engineer? engineer = s_dalEngineer.Read(id) ;
        Console.WriteLine(engineer);//print the engineer
    }

    /// <summary>
    /// return all the engineers from the data base and print them
    /// </summary>
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

    /// <summary>
    /// update a engineer from the data base
    /// </summary>
    private static void UpdateEngineer()
    {
        int id = GetId();

        Engineer? oldEngineer = s_dalEngineer.Read(id);//find the index of the engineer with the same id
        Console.WriteLine("The old engineer:");
        Console.WriteLine(oldEngineer);//print the engineer

        Engineer engineer = EngineerCreation(id);//create the new engineer
        s_dalEngineer.Update(engineer);//update the engineer
    }

    /// <summary>
    /// delete a engineer from the data base
    /// </summary>
    private static void DeleteEngineer()
    {

        int id = GetId();
        s_dalEngineer.Delete(id);//delete the engineer
    }

    /// <summary>
    /// create a new dependency and add it to the data base
    /// </summary>
    private static void CreateDependency()
    {
        int id = GetId();
        Dependency dependency = DependencyCreation(id);//create the dependency
        s_dalDependency.Create(dependency);//add the dependency to the data base

    }

    /// <summary>
    /// read a dependency from the data base and print it
    /// </summary>
    private static void ReadDependency()
    {
        //get the id of the dependency
        int id = GetId();
        Dependency? dependency = s_dalDependency.Read(id);
        Console.WriteLine(dependency);//print the dependency
    }

    /// <summary>
    /// return all the dependencies from the data base and print them
    /// </summary>
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

    /// <summary>
    /// update a dependency from the data base
    /// </summary>
    private static void UpdateDependency()
    {
        int id = GetId();

        Dependency? oldDependency = s_dalDependency.Read(id);//find the index of the dependency with the same id
        Console.WriteLine("The old dependency:");
        Console.WriteLine(oldDependency);//print the dependency

        Dependency dependency = DependencyCreation(id);//create the new dependency
        s_dalDependency.Update(dependency);//update the dependency
    }

    /// <summary>
    /// delete a dependency from the data base
    /// </summary>
    private static void DeleteDependency()
    {
        int id = GetId();
        s_dalDependency.Delete(id);//delete the dependency
    }

    /// <summary>
    /// create a new task and return it
    /// </summary>
    /// <param name="id">the id of the new Task</param>
    /// <returns></returns>
    private static Task TaskCreation(int id)
    {
        // because we have nullable values we need to use temp variables to check if the input is valid
        DateTime temp1;// to use for the nullable dates
        TimeSpan temp2;// to use for the nullable time span
        int temp3;

        Console.WriteLine("Enter the values of the task:");

        //get the new values
        Console.Write("alias:");
        string alias = Console.ReadLine()!;// we use ! because we know that the input is not need to be null

        Console.Write("description:");
        string description = Console.ReadLine()!;// we use ! because we know that the input is not need to be null

        Console.Write("created at date:");
        DateTime.TryParse(Console.ReadLine()!, out DateTime createdAtDate);

        Console.Write("scheduled date:");
        DateTime? scheduledDate = null;// we initialize the nullable to default 
        if (DateTime.TryParse(Console.ReadLine(), out temp1))// if the input is valid
            scheduledDate = temp1;// we change the nullable to the input

        Console.Write("start date:");
        DateTime? startDate = null;// we initialize the nullable to default
        if (DateTime.TryParse(Console.ReadLine(), out temp1))// if the input is valid
            startDate = temp1;// we change the nullable to the input

        Console.Write("required effort time:");
        TimeSpan? requiredEffortTime = null;// we initialize the nullable to default
        if (TimeSpan.TryParse(Console.ReadLine(), out temp2))// if the input is valid
            requiredEffortTime = temp2;// we change the nullable to the input

        Console.Write("deadline date:");
        DateTime? deadlineDate = null;// we initialize the nullable to default
        if (DateTime.TryParse(Console.ReadLine(), out temp1))// if the input is valid
            deadlineDate = temp1;// we change the nullable to the input

        Console.Write("complete date:");
        DateTime? completeDate = null;// we initialize the nullable to default
        if (DateTime.TryParse(Console.ReadLine(), out temp1))// if the input is valid
            completeDate = temp1;// we change the nullable to the input

        Console.Write("deliverables:");
        string? deliverables = Console.ReadLine();

        Console.Write("remarks:");
        string? remarks = Console.ReadLine();

        Console.Write("engineer id:");
        int? engineerId = null;// we initialize the nullable to default
        if (int.TryParse(Console.ReadLine(), out temp3))// if the input is valid
            engineerId = temp3;// we change the nullable to the input

        Console.Write("copmlexity:");
        EngineerExperience? copmlexity = null;// we initialize the nullable to default
        if (Enum.TryParse<EngineerExperience>(Console.ReadLine(), out EngineerExperience experience))
            copmlexity = experience;//if the input is valid we change the nullable to the input

        //create the new task
        Task task = new(
            id,// we put 0 because the id is auto increment
            alias,
            description,
            createdAtDate,
            false,// told us to put only null for now
            true,
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
        return task;
    }

    /// <summary>
    /// create a new engineer and return it
    /// </summary>
    /// <param name="id">the id of the new Engineer</param>
    /// <returns></returns>
    private static Engineer EngineerCreation(int id)
    {
        //get the new values
        Console.Write("cost:");
        double.TryParse(Console.ReadLine()!, out double cost);

        Console.Write("name:");
        string name = Console.ReadLine()!;// we use ! because we know that the input is not need to be null

        Console.Write("email:");
        string email = Console.ReadLine()!;// we use ! because we know that the input is not need to be null

        Console.Write("level:");
        Enum.TryParse<EngineerExperience>(Console.ReadLine()!, out EngineerExperience level);

        //create the new engineer
        Engineer engineer = new(
            id,
            cost,
            name,
            email,
            level
        );
        return engineer;
    }

    /// <summary>
    /// create a new dependency
    /// </summary>
    /// <param name="id"> the id of the dependency</param>
    /// <returns></returns>
    private static Dependency DependencyCreation(int id)
    {
        Console.WriteLine("Enter the values of the dependency:");

        //get the new values
        Console.Write("task id:");
        int? taskId = null;// we initialize the nullable to default
        if (int.TryParse(Console.ReadLine(), out int dependent))// if the input is valid
            taskId = dependent;// we change the nullable to the input

        Console.Write("depend on task id:");
        int? dependOnTaskId = null;// we initialize the nullable to default
        if (int.TryParse(Console.ReadLine(), out int dependsOn))// if the input is valid
            dependOnTaskId = dependsOn;// we change the nullable to the input


        //create the new dependency
        Dependency dependency = new(
            id,
            taskId,
            dependOnTaskId
        );
        return dependency;
    }

    /// <summary>
    /// the function get an id from the user and return it, while checking if the input is valid
    /// </summary>
    private static int GetId()
    {
        Console.Write("enter the id:");
        int.TryParse(Console.ReadLine()!, out int id);
        return id;
    }

    static void Main(string[] args)
    {
        try
        {
            Initialization.DO(s_dalTask, s_dalEngineer, s_dalDependency);//initialize the data base

            Console.WriteLine("!start of the program!\n");
            ShowMenu();//show the menu
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        Console.WriteLine("press any key to continue");
        Console.ReadKey();
    }
}
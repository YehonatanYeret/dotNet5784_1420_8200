namespace BlTest;

using BO;
using DalTest;

enum CHOISE
{
    EXIT,
    TASK,
    ENGINEER,
    DEPENDENCY,
    SETDATES
}
enum SUBCHOISE
{
    BACK,
    CREATE,
    READ,
    READALL,
    UPDATE,
    DELETE
}

enum EXECUTION_STAGE
{
    EXIT,
    ENGINEER,
    TASK_UPDATES,
    TASK_READ,
    TASK_READALL,
    DEPENDENCIES_READ,
    DEPENDENCIES_READALL,
    TASK_SETENGINEER,
    TASK_CHANGE_STATUS
}

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    private static readonly Random s_rand = new(); // Random number generator

    /// <summary>
    /// Display the main menu options to the user
    /// </summary>
    /// <returns>the user's choice</returns>
    static int ShowMenu()
    {
        try
        {
            // Display the menu options to the user
            Console.WriteLine("Choose an option:\n" +
                "0. Exit the planning stage\n" +
                "1. Task \n" +
                "2. Engineer\n" +
                "3. Dependency\n" +
                "4. Set the date off the tasks");

            // Read the user's input and parse it to an integer, storing it in the 'choice' variable
            CHOISE choice = (CHOISE)Enum.Parse(typeof(CHOISE), Console.ReadLine()!);

            // Use a switch statement to handle different cases based on the user's choice
            return choice switch
            {
                // If the user chose 0, exit the program
                CHOISE.EXIT => (int)CHOISE.EXIT,

                // If the user chose 1, go to the SubMenu with the argument "Task"
                CHOISE.TASK => SubMenuForTask(),

                // If the user chose 2, go to the SubMenu with the argument "Engineer"
                CHOISE.ENGINEER => SubMenuForEngineer(),

                // If the user chose 3, go to the SubMenu with the argument "Dependency"
                CHOISE.DEPENDENCY => SubMenuForDependency(),

                // If the user chose 4, set the dates of the tasks and start the project
                CHOISE.SETDATES => UpdateAllDates(),

                _ => throw new Exception("Invalid input")
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return (int)CHOISE.EXIT;
        }
    }

    /// <summary>
    ///  Display the submenu options of engineer to the user
    /// </summary>
    /// <returns>the user's choice</returns> 
    static int SubMenuForEngineer()
    {
        // Display the submenu options to the user
        Console.WriteLine("Choose an option:\n" +
            "0. Back\n" +
            "1. Create\n" +
            "2. Read\n" +
            "3. ReadAll\n" +
            "4. Update\n" +
            "5. Delete\n");

        // Read the user's input and parse it to an integer, storing it in the 'subChoice' variable
        SUBCHOISE subChoice = (SUBCHOISE)Enum.Parse(typeof(CHOISE), Console.ReadLine()!);

        // Process user input based on the main menu choice and submenu option
        try
        {
            return subChoice switch
            {
                // Task submenu options
                SUBCHOISE.BACK => (int)subChoice,
                SUBCHOISE.CREATE => CreateEngineer(),
                SUBCHOISE.READ => ReadEngineer(),
                SUBCHOISE.READALL => ReadAllEngineer(),
                SUBCHOISE.UPDATE => UpdateEngineer(),
                SUBCHOISE.DELETE => DeleteEngineer(),
                _ => throw new Exception("Invalid input")
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return 2;
        }
    }

    /// <summary>
    /// Display the submenu options of task to the user
    /// </summary>
    /// <returns>the user's choice</returns>
    static int SubMenuForTask()
    {
        // Display the submenu options to the user
        Console.WriteLine("Choose an option:\n" +
            "0. Back\n" +
            "1. Create\n" +
            "2. Read\n" +
            "3. ReadAll\n" +
            "4. Update\n" +
            "5. Delete\n");
        try
        {
            // Read the user's input and parse it to an integer, storing it in the 'subChoice' variable
            SUBCHOISE subChoice = (SUBCHOISE)Enum.Parse(typeof(CHOISE), Console.ReadLine()!);

            // Process user input based on the main menu choice and submenu option
            return subChoice switch
            {
                // Task submenu options
                SUBCHOISE.BACK => (int)subChoice,
                SUBCHOISE.CREATE => CreateTask(),
                SUBCHOISE.READ => ReadTask(),
                SUBCHOISE.READALL => ReadAllTask(),
                SUBCHOISE.UPDATE => UpdateTask(),
                SUBCHOISE.DELETE => DeleteTask(),
                _ => throw new Exception("Invalid input")
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return 1;
        }
    }

    static int SubMenuForDependency()
    {
        // Display the submenu options to the user
        Console.WriteLine("Choose an option:\n" +
            "0. Back\n" +
            "1. Create\n" +
            "2. Read\n" +
            "3. ReadAll\n");

        // Read the user's input and parse it to an integer, storing it in the 'subChoice' variable
        SUBCHOISE subChoice = (SUBCHOISE)Enum.Parse(typeof(CHOISE), Console.ReadLine()!);

        // Process user input based on the main menu choice and submenu option
        return subChoice switch
        {
            // Task submenu options
            SUBCHOISE.BACK => (int)subChoice,
            SUBCHOISE.CREATE => CreateDependency(),
            SUBCHOISE.READ => ReadDependeny(),
            SUBCHOISE.READALL => ReadAllDependeny(),
            _ => throw new Exception("Invalid input")
        };
    }

    //-------------------------------------------------------------------------------------------

    /// <summary>
    /// Update all the dates of the tasks
    /// </summary>
    static int UpdateAllDates()
    {
        try
        {
            Console.WriteLine("What is the desired date to start the project");
            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startProject))
                throw new FormatException("Wrong input");

            while (s_bl!.Task.ReadAllTask(task => task.ScheduledDate is null).Any())
            {
                foreach (var item in s_bl.Task.ReadAllTask(task => task.ScheduledDate == null))
                {
                    try
                    {
                        // calculate the closest date based on the startDate of the program and the depndencies 
                        DateTime closest = s_bl.Task.CalculateClosestStartDate(item.Id, startProject);

                        // if the user want to set a date that is closer then the closest date then the closest date will be the new date
                        Console.WriteLine($"Do you want to set a forther date then {closest} to the task {item.Id}? (Y/N)");
                        if (Console.ReadLine()!.ToUpper() == "Y")
                        {
                            Console.WriteLine("What is the desired date to start the task");
                            if (!DateTime.TryParse(Console.ReadLine(), out DateTime startTask) || startTask < closest)
                                throw new FormatException("cannot set a closer date");
                            s_bl.Task.UpdateScheduledDate(item.Id, startTask);

                        }
                        else
                            s_bl.Task.UpdateScheduledDate(item.Id, closest);

                        Console.WriteLine("What is the deadline for the task");
                        // if the user typed a wrong input/enter then the deadline will be the closest date + the required time + 1 day
                        if (!DateTime.TryParse(Console.ReadLine(), out DateTime deadline))
                            deadline = closest + s_bl.Task.Read(item.Id).RequiredEffortTime + TimeSpan.FromDays(1);

                        // update the deadline of the task
                        s_bl.Task.UpdateDeadLineDate(item.Id, deadline);

                        Console.WriteLine(s_bl.Task.Read(item.Id));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "so first satrt his dependencies");
                        continue;
                    }
                }
            }
            s_bl.Clock.SetStartProject(startProject);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return 4;
    }

    //-------------------------------------------------------------------------------------------

    /// <summary>
    /// Display the main menu options to the user of the execution stage
    /// </summary>
    /// <returns>the user's choice</returns>
    static int ExecuteStage()
    {
        try
        {
            Console.WriteLine("choose an option:\n" +
                "0. Exit the execution stage\n" +
                "1. Create, Read, Update or Delete Engineer\n" +
                "2. Updates details of Tasks\n" +
                "3. Read Tasks\n" +
                "4. Read All Tasks\n" +
                "5. Read Dependencies of a Task\n" +
                "6. Read All Dependencies\n" +
                "7. Set an Engineer to a Task\n" +
                "8. Change status of Task\n");

            // Read the user's input and parse it to an integer, storing it in the 'choice' variable
            EXECUTION_STAGE choice = (EXECUTION_STAGE)Enum.Parse(typeof(EXECUTION_STAGE), Console.ReadLine()!);

            // Use a switch statement to handle different cases based on the user's choice
            return choice switch
            {
                // If the user chose 0, exit the program
                EXECUTION_STAGE.EXIT => (int)EXECUTION_STAGE.EXIT,

                // If the user chose 1, go to the SubMenu with the argument "Engineer"
                EXECUTION_STAGE.ENGINEER => SubMenuForEngineer(),

                // If the user chose 2, go to update the details of the tasks without changing the required effort time
                EXECUTION_STAGE.TASK_UPDATES => UpdateTask(),

                // If the user chose 3, go Read the details of a specific task
                EXECUTION_STAGE.TASK_READ => ReadTask(),

                // If the user chose 4, go Read the details of all tasks
                EXECUTION_STAGE.TASK_READALL => ReadAllTask(),

                // If the user chose 5, go Read the dependencies of a specific task
                EXECUTION_STAGE.DEPENDENCIES_READ => ReadDependeny(),

                // If the user chose 6, go Read the dependencies of all tasks
                EXECUTION_STAGE.DEPENDENCIES_READALL => ReadAllDependeny(),

                // If the user chose 7, go Set an Engineer to a Task
                EXECUTION_STAGE.TASK_SETENGINEER => SetEngineerToTask(),

                // If the user chose 8, go Change status of Task
                EXECUTION_STAGE.TASK_CHANGE_STATUS => changeStatusOfTask(),

                _ => throw new Exception("Invalid input")
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return 9;
        }
    }

    //-------------------------------------------------------------------------------------------

    /// <summary>
    /// Creates a new engineer, assigns it a unique identifier, and adds it to the database.
    /// </summary>
    /// <remarks>
    /// This method generates a unique identifier for the engineer using the <see cref="GetId"/> function,
    /// creates a new engineer object through the <see cref="EngineerCreation"/> function, and then adds
    /// the engineer to the database using the data access layer. The engineer creation and database insertion
    /// are performed sequentially within this method.
    /// </remarks>
    /// <seealso cref="GetId"/>
    /// <seealso cref="EngineerCreateAndUpdate"/>
    private static int CreateEngineer()
    {
        // Obtain a unique identifier for the engineer
        int id = GetId();
        //get the new values
        Console.Write("cost:");
        if (!double.TryParse(Console.ReadLine()!, out double cost))// if the input is not valid
            throw new BO.BLValueIsNotCorrectException("cannot convert cost");// throw an exception

        Console.Write("name:");
        string name = Console.ReadLine()!;// we use ! because we know that the input is not need to be null

        Console.Write("email:");
        string email = Console.ReadLine()!;// we use ! because we know that the input is not need to be null
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentNullException();// if the input is still null or empty or only spaces

        Console.Write("level:");
        // if the input is not valid or not in the enum we will throw an exception
        if (!Enum.TryParse<BO.EngineerExperience>(Console.ReadLine()!, out BO.EngineerExperience level))
            throw new ArgumentException("Invalid input");

        //create the new engineer
        BO.Engineer engineer = new()
        {
            Id = id,
            Cost = cost,
            Name = name,
            Email = email,
            Level = level
        };

        // Add the created engineer to the database using the data access layer
        Console.WriteLine("the new id is: " + s_bl!.Engineer.Create(engineer));
        return 1;
    }


    /// <summary>
    /// Reads and displays information about a specific engineer based on the provided identifier.
    /// </summary>
    /// <remarks>
    /// This method prompts the user to enter an identifier for the engineer, retrieves the corresponding
    /// engineer from the database using the data access layer, and then prints the engineer information to
    /// the console. If the engineer is not found in the database, a null value is returned, and a message
    /// indicating that the engineer was not found is displayed.
    /// </remarks>
    /// <seealso cref="GetId"/>
    private static int ReadEngineer()
    {
        // Get the id of the engineer from the user
        int id = GetId();

        // Read the engineer from the database using the data access layer
        BO.Engineer? engineer = s_bl!.Engineer.Read(id);

        if (engineer == null)
            throw new BO.BLDoesNotExistException("The engineer is not exist");

        // Print the engineer information to the console
        Console.WriteLine(engineer);

        return 2;
    }

    /// <summary>
    /// Reads and displays information about all engineers stored in the database.
    /// </summary>
    /// <remarks>
    /// This method retrieves all engineers from the database using the data access layer's <see cref="bl.Engineer.ReadAll"/> method
    /// and prints the information of each engineer to the console. If there are no engineers in the database, a message indicating
    /// that there are no engineers is displayed.
    /// </remarks>
    private static int ReadAllEngineer()
    {
        Console.WriteLine("All of the engineers:");

        // Retrieve all engineers from the database using the data access layer
        var engineers = s_bl!.Engineer.ReadAll();

        // Iterate through the engineers and print their information to the console
        foreach (var engineer in engineers)
        {
            Console.WriteLine(engineer);
        }
        return 3;
    }

    /// <summary>
    /// Updates information for a specific engineer based on the provided identifier.
    /// </summary>
    /// <remarks>
    /// This method prompts the user to enter an identifier for the engineer using the <see cref="GetId"/> function,
    /// retrieves the corresponding engineer from the database using the data access layer, and displays the current
    /// information of the engineer to the console. If the engineer is not found in the database, a message is displayed,
    /// and the function exits. If the engineer exists, the <see cref="EngineerUpdate"/> function is used to create a new
    /// engineer with updated information, and the database is updated with the new engineer.
    /// </remarks>
    /// <seealso cref="GetId"/>
    /// <seealso cref="EngineerUpdate"/>
    private static int UpdateEngineer()
    {
        // Obtain an identifier for the engineer from the user
        int id = GetId();

        // Read the existing engineer from the database using the data access layer
        BO.Engineer? oldEngineer = s_bl!.Engineer.Read(id);

        // Check if the engineer exists in the database
        if (oldEngineer == null)
            throw new BO.BLDoesNotExistException("The engineer is not exist");

        // Display the current information of the engineer to the console
        Console.WriteLine("The old engineer:\n" + oldEngineer);

        Console.WriteLine("Enter the values of the enginner:");
        // Get the updated values from user input or use the old values if input is empty
        Console.Write("cost:");
        if (!double.TryParse(Console.ReadLine()!, out double cost))
            cost = oldEngineer.Cost;

        Console.Write("name:");
        string name = Console.ReadLine()!; // Non-nullable input
        if (string.IsNullOrEmpty(name))
            name = oldEngineer.Name;

        Console.Write("email:");
        string email = Console.ReadLine()!; // Non-nullable input
        if (string.IsNullOrEmpty(email))
            email = oldEngineer.Email;

        Console.Write("level:");
        if (!Enum.TryParse(Console.ReadLine()!, out BO.EngineerExperience level))
            level = oldEngineer.Level;

        // Create the new engineer with updated values
        BO.Engineer engineer = new BO.Engineer
        {
            Id = id, // Keep the old id or put 0 by defaulte
            Cost = cost,
            Name = name,
            Email = email,
            Level = level
        };
        // Update the engineer in the database using the data access layer
        s_bl!.Engineer.Update(engineer);

        return 4;
    }

    /// <summary>
    /// Deletes a specific engineer from the database based on the provided identifier.
    /// </summary>
    /// <remarks>
    /// This method prompts the user to enter an identifier for the engineer using the <see cref="GetId"/> function
    /// and deletes the corresponding engineer from the database using the data access layer. If the engineer is not found
    /// in the database, no action is taken.
    /// </remarks>
    /// <seealso cref="GetId"/>
    private static int DeleteEngineer()
    {
        // Obtain an identifier for the engineer from the user and delete if found
        int id = GetId();
        s_bl!.Engineer.Delete(id);

        return 5;
    }
    //-------------------------------------------------------------------------------------------

    /// <summary>
    /// Creates a new task, assigns it a unique identifier, and adds it to the database.
    /// </summary>
    /// <remarks>
    /// This method generates a unique identifier for the task using the <see cref="GetId"/> function,
    /// creates a new task object through the <see cref="TaskCreation"/> function, and then adds the
    /// task to the database using the data access layer. The task creation and database insertion
    /// are performed sequentially within this method.
    /// </remarks>
    /// <seealso cref="GetId"/>
    /// <seealso cref="TaskCreation"/>
    private static int CreateTask()
    {
        Console.WriteLine("Enter the values of the task:");

        // Get the updated values from user input or use the old values if input is empty
        Console.Write("alias:");
        string alias = Console.ReadLine()!;

        Console.Write("description:");
        string description = Console.ReadLine()!;

        // Get the updated values from user input or use the old values if input is empty
        // or if the program is in the execution stage
        Console.Write("required effort time:");
        TimeSpan requiredEffortTime = TimeSpan.Parse(Console.ReadLine()!);

        Console.Write("deliverables:");
        string? deliverables = Console.ReadLine();

        Console.Write("remarks:");
        string? remarks = Console.ReadLine();

        Console.Write("complexity:");
        BO.EngineerExperience? complexity = null;
        if(Enum.TryParse(Console.ReadLine(), out BO.EngineerExperience tempComplexity))
            complexity = tempComplexity;


        // Create the new task with updated values
        BO.Task task = new BO.Task
        {
            Id = 0, // Keep the old id or Deafault value
            Alias = alias,
            Description = description,
            CreatedAtDate = DateTime.Now, // The current time
            RequiredEffortTime = requiredEffortTime,
            Deliverables = deliverables,
            Remarks = remarks,
            Complexity = complexity
        };

        // Add the created task to the database using the data access layer
        Console.WriteLine("the new id is: " + s_bl!.Task.Create(task));

        return 1;
    }

    /// <summary>
    /// Reads and displays information about a specific task based on the provided identifier.
    /// </summary>
    /// <remarks>
    /// This method prompts the user to enter an identifier for the task using the <see cref="GetId"/> function,
    /// retrieves the corresponding task from the database using the data access layer, and then prints the task
    /// information to the console. If the task is not found in the database, a null value is returned, and a message
    /// indicating that the task was not found is displayed.
    /// </remarks>
    /// <seealso cref="GetId"/>
    private static int ReadTask()
    {
        // Obtain an identifier for the task from the user
        int id = GetId();

        // Read the task from the database using the data access layer
        BO.Task? task = s_bl!.Task.Read(id);

        // Print the task information to the console if found
        if (task == null)
            throw new BO.BLDoesNotExistException("The task is not exist");

        Console.WriteLine(task);

        return 2;
    }

    /// <summary>
    /// Reads and displays information about all tasks stored in the database.
    /// </summary>
    /// <remarks>
    /// This method retrieves all tasks from the database using the data access layer's <see cref="Bl.Task.ReadAll"/> method
    /// and prints the information of each task to the console. If there are no tasks in the database, a message indicating
    /// that there are no tasks is displayed.
    /// </remarks>
    private static int ReadAllTask()
    {
        Console.WriteLine("All of the tasks:");

        // Retrieve all tasks from the database using the data access layer
        var tasks = s_bl!.Task.ReadAll();

        //Iterate through the tasks and print their information to the console
        foreach (var task in tasks)
        {
            Console.WriteLine(task);
        }
        return 3;
    }

    /// <summary>
    /// Updates information for a specific task based on the provided identifier.
    /// </summary>
    /// <remarks>
    /// This method prompts the user to enter an identifier for the task using the <see cref="GetId"/> function,
    /// retrieves the corresponding task from the database using the data access layer, and displays the current
    /// information of the task to the console. If the task is not found in the database, a message is displayed,
    /// and the function exits. If the task exists, the <see cref="TaskUpdate"/> function is used to create a new
    /// task with updated information, and the database is updated with the new task.
    /// </remarks>
    /// <seealso cref="GetId"/>
    /// <seealso cref="TaskUpdate"/>
    private static int UpdateTask()
    {
        // Obtain an identifier for the task from the user
        int id = GetId();

        // Read the existing task from the database using the data access layer
        BO.Task? oldTask = s_bl!.Task.Read(id);

        // Check if the task exists in the database
        if (oldTask == null)
            throw new BO.BLDoesNotExistException("The task is not exist");

        // Display the current information of the task to the console
        Console.WriteLine("The old task: \n" + oldTask);

        Console.WriteLine("Enter new values of the task:");

        // Get the updated values from user input or use the old values if input is empty
        Console.Write("alias:");
        string? alias = Console.ReadLine();
        if (string.IsNullOrEmpty(alias))
            alias = oldTask.Alias;

        Console.Write("description:");
        string? description = Console.ReadLine();
        if (string.IsNullOrEmpty(description))
            description = oldTask.Description;

        // Get the updated values from user input or use the old values if input is empty
        // or if the program is in the execution stage
        TimeSpan requiredEffortTime = oldTask.RequiredEffortTime;
        if (s_bl.Clock.GetProjectStatus() is BO.ProjectStatus.NotStarted)
        {
            Console.Write("required effort time:");
            if (TimeSpan.TryParse(Console.ReadLine(), out TimeSpan temp2))
                requiredEffortTime = temp2;
        }

        Console.Write("deliverables:");
        string? deliverables = Console.ReadLine();
        if (string.IsNullOrEmpty(deliverables))
            deliverables = oldTask.Deliverables;

        Console.Write("remarks:");
        string? remarks = Console.ReadLine();
        if (string.IsNullOrEmpty(remarks))
            remarks = oldTask.Remarks;

        Console.Write("complexity:");
        BO.EngineerExperience? complexity = oldTask.Complexity;
        if (Enum.TryParse(Console.ReadLine(), out BO.EngineerExperience experience))
            complexity = experience;

        // Create the new task with updated values
        BO.Task task = new BO.Task
        {
            Id = oldTask.Id, // Keep the old id or Deafault value
            Alias = alias,
            Description = description,
            CreatedAtDate = DateTime.Now, // The current time
            RequiredEffortTime = requiredEffortTime,
            Deliverables = deliverables,
            Remarks = remarks,
            Complexity = complexity
        };
        // Update the task in the database using the data access layer
        s_bl!.Task.Update(task);

        return 4;
    }

    /// <summary>
    /// Deletes a specific task from the database based on the provided identifier.
    /// </summary>
    /// <remarks>
    /// This method prompts the user to enter an identifier for the task using the <see cref="GetId"/> function
    /// and deletes the corresponding task from the database using the data access layer. If the task is not found
    /// in the database, no action is taken.
    /// </remarks>
    /// <seealso cref="GetId"/>
    private static int DeleteTask()
    {
        // Obtain an identifier for the task from the user
        int id = GetId();

        // Delete the task from the database using the data access layer
        s_bl!.Task.Delete(id);

        return 5;
    }

    //-------------------------------------------------------------------------------------------

    /// <summary>
    /// Creates a new dependency for a specific task based on the provided identifier.
    /// </summary>
    private static int CreateDependency()
    {
        Console.WriteLine("Enter the id of the task that you want to add a dependency to");
        int id = int.Parse(Console.ReadLine()!);

        Console.WriteLine($"Enter the id of the task that you want task {id} to depend on");
        int dependencyId = int.Parse(Console.ReadLine()!);

        BO.Task task = s_bl.Task.Read(id);

        // Create a new dependency for the task in the database
        task.Dependencies = task.Dependencies!.Append(new BO.TaskInList
        {
            Id = dependencyId,
            Alias = s_bl.Task.Read(dependencyId).Alias,
            Description = s_bl.Task.Read(dependencyId).Description
        }).ToList();

        // Update the task with the new dependency
        s_bl.Task.Update(task);

        return 1;
    }

    /// <summary>
    ///  Reads and displays information about the dependencies of a specific task based on the provided identifier.
    /// </summary>
    private static int ReadDependeny()
    {
        Console.WriteLine("Enter the id of the task that you want to read its dependencies");
        int id = int.Parse(Console.ReadLine()!);

        BO.Task task = s_bl.Task.Read(id);

        // Print the task information to the console
        if (!task.Dependencies!.Any())
        {
            Console.WriteLine("The task has no dependencies");
            return 2;
        }

        // Print the dependencies of the task to the console
        foreach (var item in task.Dependencies!)
        {
            Console.WriteLine(item);
        }

        return 2;
    }

    /// <summary>
    ///  Reads and displays information about all dependencies stored in the database.
    /// </summary>
    private static int ReadAllDependeny()
    {
        Console.WriteLine("All of the dependencies:");

        IEnumerable<BO.Task>? tasks = s_bl.Task.ReadAllTask(task => task.Dependencies is not null).OrderBy(task => task.Id);

        // Iterate through the tasks and print their information to the console
        foreach (var task in tasks)
        {
            // Print the task information to the console
            if (!task.Dependencies!.Any())
                Console.WriteLine($"Task {task.Id} has no dependencies.");
            else
                Console.WriteLine($"The dependencies of the task {task.Id} are:");

            // Print the dependencies of the task to the console
            foreach (var item in task.Dependencies!)
                Console.WriteLine(item);

        }
        return 3;
    }

    //-------------------------------------------------------------------------------------------

    /// <summary>
    ///  Set an engineer to a task
    /// </summary>
    static int SetEngineerToTask()
    {
        Console.WriteLine("Enter the id of the task that you want to set an engineer to");
        int id = int.Parse(Console.ReadLine()!);

        Console.WriteLine("Enter the id of the engineer that you want to set to the task");
        int engineerId = int.Parse(Console.ReadLine()!);

        s_bl.Engineer.SetTaskToEngineer(engineerId, id);

        return 7;
    }

    /// <summary>
    ///  Change the status of a specific task based on the provided identifier.
    /// </summary>
    static int changeStatusOfTask()
    {
        Console.WriteLine("Enter the id of the task that you want to change its status");
        int id = int.Parse(Console.ReadLine()!);

        // Change the status of the task in the database and print the new status
        Console.WriteLine("The new status of the task is: " + s_bl.Task.ChangeStatusOfTask(id));

        return 8;
    }

    //-------------------------------------------------------------------------------------------



    /// <summary>
    /// Retrieves an integer identifier from the user, ensuring the input is valid.
    /// </summary>
    /// <returns>The user-provided integer identifier.</returns>
    private static int GetId()
    {
        Console.Write("Enter the id:");
        if (!int.TryParse(Console.ReadLine()!, out int id))
            throw new BO.BLValueIsNotCorrectException("Invalid input");
        return id;
    }
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Welcome to the project management system!");

            Console.WriteLine("Would you like to create Initial data? (Y/N)");
            string ans = Console.ReadLine()!;

            if (ans.ToUpper() == "Y")// if the user typed Y or y then create initial data and erase the old data
                Initialization.Do();

            // Display the main menu options to the user of the planning stage
            if (s_bl.Clock.GetProjectStatus() is BO.ProjectStatus.NotStarted)
            {
                Console.WriteLine("The planning stage:");
                while (ShowMenu() is not 0 and not 4) ;
            }
            if (s_bl.Clock.GetProjectStatus() is BO.ProjectStatus.InProgress)
            {
                // Display the main menu options to the user of the execution stage
                Console.WriteLine("The execution stage:");
                while (ExecuteStage() is not 0) ;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        finally
        {
            Console.WriteLine("Goodbye!");
        }
    }
}
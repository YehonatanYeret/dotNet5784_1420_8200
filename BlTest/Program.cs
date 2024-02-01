namespace BlTest;

using DalApi;
using DalTest;

enum CHOISE
{
    EXIT,
    TASK,
    ENGINEER,
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

internal class Program
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

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
                "0. Exit \n" +
                "1. Task \n" +
                "2. Engineer\n");

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

                _ => throw new Exception("Invalid input")
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return 1;
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
        return subChoice switch
        {
            // Task submenu options
            SUBCHOISE.BACK  => 0,
            SUBCHOISE.CREATE  => CreateEngineer(),
            SUBCHOISE.READ  => ReadEngineer(),
            SUBCHOISE.READALL => ReadAllEngineer(),
            SUBCHOISE.UPDATE  => UpdateEngineer(),
            SUBCHOISE.DELETE  => DeleteEngineer(),
            _ => throw new Exception("Invalid input")
        };
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

        // Read the user's input and parse it to an integer, storing it in the 'subChoice' variable
        SUBCHOISE subChoice = (SUBCHOISE)Enum.Parse(typeof(CHOISE), Console.ReadLine()!);

        // Process user input based on the main menu choice and submenu option
        return subChoice switch
        {
            // Task submenu options
            SUBCHOISE.BACK => 0,
            SUBCHOISE.CREATE => CreateTask(),
            SUBCHOISE.READ   => ReadTask(),
            SUBCHOISE.READALL => ReadAllTask(),
            SUBCHOISE.UPDATE  => UpdateTask(),
            SUBCHOISE.DELETE  => DeleteTask(),
            _ => throw new Exception("Invalid input")
        };
    }

    /// <summary>
    /// Update all the dates of the tasks
    /// </summary>
    static void UpdateAllDates()
    {
        Console.WriteLine("What is the desired date to start the project");
        if(!DateTime.TryParse(Console.ReadLine(), out DateTime startProject))
            throw new FormatException("Wrong input");
        foreach (var item in s_bl.Task.ReadAll())
        {
            DateTime closest = s_bl.Task.CalculateClosestStartDate(item.Id, startProject);
            Console.WriteLine($"Do you want to set a forther date then {closest}?");
            if (Console.ReadLine()!.ToUpper() == "Y")
            {
                Console.WriteLine("What is the desired date to start the task");
                if (!DateTime.TryParse(Console.ReadLine(), out DateTime startTask) || startTask<closest)
                    throw new FormatException("Wrong input");
            }
            else
                s_bl.Task.UpdateScheduledDate(item.Id, closest);
        }
        s_bl.StartProject = startProject;
    }

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
    /// <seealso cref="EngineerCreation"/>
    private static int CreateEngineer()
    {
        // Obtain a unique identifier for the engineer
        int id = GetId();

        // Create a new engineer using the obtained identifier
        BO.Engineer engineer = EngineerCreateAndUpdate(new(), id);

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
        {
            Console.WriteLine("The engineer is not exist");
            return 2;
        }

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
        {
            Console.WriteLine("The engineer is not exist");
            return 4;
        }

        // Display the current information of the engineer to the console
        Console.WriteLine("The old engineer:\n" + oldEngineer);

        // Update the engineer in the database using the data access layer
        s_bl!.Engineer.Update(EngineerCreateAndUpdate(oldEngineer, id));

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
        // Create a new task using the obtained identifier
        BO.Task task = TaskCreateAndUpdate(new());

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
        {
            Console.WriteLine("The task is not exist");
            return 2;
        }
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
        {
            Console.WriteLine("The task is not exist");
            return 4;
        }

        // Display the current information of the task to the console
        Console.WriteLine("The old task: \n" + oldTask);

        // Update the task in the database using the data access layer
        s_bl!.Task.Update(TaskCreateAndUpdate(oldTask));

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


    /// <summary>
    /// Returns a new engineer with updated values based on the provided old engineer.
    /// </summary>
    /// <param name="oldEngineer">The engineer to update.</param>
    /// <returns>The updated engineer with new values.</returns>
    private static BO.Engineer EngineerCreateAndUpdate(BO.Engineer oldEngineer, int id)
    {
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
        return engineer;
    }

    /// <summary>
    /// Returns a new task with updated values based on the provided old task.
    /// </summary>
    /// <param name="oldTask">The old task to update.</param>
    /// <returns>The updated task with new values.</returns>
    private static BO.Task TaskCreateAndUpdate(BO.Task oldTask)
    {
        Console.WriteLine("Enter the values of the task:");

        // Get the updated values from user input or use the old values if input is empty
        Console.Write("alias:");
        string? alias = Console.ReadLine();
        if (string.IsNullOrEmpty(alias))
            alias = oldTask.Alias;

        Console.Write("description:");
        string? description = Console.ReadLine();
        if (string.IsNullOrEmpty(description))
            description = oldTask.Description;

        Console.Write("required effort time:");
        TimeSpan? requiredEffortTime = oldTask.RequiredEffortTime;
        if (TimeSpan.TryParse(Console.ReadLine(), out TimeSpan temp2))
            requiredEffortTime = temp2;

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
        BO.Task task = new BO.Task {
            Id = oldTask.Id, // Keep the old id or Deafault value
            Alias= alias,
            Description= description,
            CreatedAtDate= DateTime.Now, // The current time
            RequiredEffortTime= requiredEffortTime,
            Deliverables= deliverables,
            Remarks= remarks,
            Complexity= complexity
        };
        return task;
    }


    /// <summary>
    /// Retrieves an integer identifier from the user, ensuring the input is valid.
    /// </summary>
    /// <returns>The user-provided integer identifier.</returns>
    private static int GetId()
    {
        Console.Write("Enter the id:");
        int id = int.Parse(Console.ReadLine()!);
        return id;
    }
    static void Main(string[] args)
    {
        try
        {
            Console.WriteLine("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");

            if (ans.ToUpper() == "Y")// if the user typed Y or y then create initial data and erase the old data
                Initialization.Do();

            while (ShowMenu() != 0) { }
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

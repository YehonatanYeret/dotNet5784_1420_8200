namespace DalTest;
using DO;

internal class Program
{
    //new implementation of IDal

    /*static readonly IDal s_dal = new DalList();*/
    static readonly DalApi.IDal s_dal = DalApi.Factory.Get;//Stage 3

    /// <summary>
    /// Displays a menu to the user and navigates to submenus based on their choices.
    /// </summary>
    /// <remarks>
    /// This method presents a menu with options for the user to choose from, including options
    /// to navigate to specific submenus or exit the program. The user's input is processed
    /// using a switch statement, directing the program flow accordingly. The loop continues
    /// until the user chooses to exit the program (entering 0).
    /// </remarks>
    /// <seealso cref="SubMenu(string)"/>
    static void ShowMenu()
    {
        int choice; // Declare a variable to store the user's choice

        do
        {
            // Display the menu options to the user
            Console.WriteLine("Choose an option:\n" +
                "0. Exit \n" +
                "1. Task \n" +
                "2. Engineer\n" +
                "3. Dependency\n" +
                "4. Initialize");

            // Read the user's input and parse it to an integer, storing it in the 'choice' variable
            choice= int.Parse(Console.ReadLine()!);

            // Use a switch statement to handle different cases based on the user's choice
            switch (choice)
            {
                case 0:
                    // If the user chose 0, exit the program
                    return;
                case 1:
                    // If the user chose 1, go to the SubMenu with the argument "Task"
                    SubMenu("Task");
                    break;
                case 2:
                    // If the user chose 2, go to the SubMenu with the argument "Engineer"
                    SubMenu("Engineer");
                    break;
                case 3:
                    // If the user chose 3, go to the SubMenu with the argument "Dependency"
                    SubMenu("Dependency");
                    break;
                case 4:
                    // If the user chose 4, initialize the data if the user really wants to
                    Console.WriteLine("Would you like to create Initial data? (Y/N)");
                    string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");

                    if (ans.ToUpper() == "Y")// if the user typed Y or y then create initial data and erase the old data
                        Initialization.Do();
                    break;
                default:
                    // If the user chose any other number, display an error message
                    Console.WriteLine("Invalid input");
                    break;
            }

        } while (choice != 0); // Continue the loop as long as the user's choice is not 0
    }

    /// <summary>
    /// Displays a submenu based on the provided choice and allows the user to perform actions within that submenu.
    /// </summary>
    /// <param name="choice">The main menu option that led to this submenu (e.g., "Task", "Engineer", "Dependency").</param>
    /// <remarks>
    /// This method presents a submenu with options specific to the main menu choice. Users can perform actions such as
    /// creating, reading, updating, or deleting items based on the submenu context. The user's input is processed using
    /// a switch statement, directing the program flow accordingly. The loop continues until the user chooses to go back to
    /// the main menu (entering 0). Any exceptions thrown during submenu operations are caught, and an error message is displayed.
    /// </remarks>
    /// <seealso cref="CreateTask"/>
    /// <seealso cref="ReadTask"/>
    /// <seealso cref="ReadAllTask"/>
    /// <seealso cref="UpdateTask"/>
    /// <seealso cref="DeleteTask"/>
    /// <seealso cref="CreateEngineer"/>
    /// <seealso cref="ReadEngineer"/>
    /// <seealso cref="ReadAllEngineer"/>
    /// <seealso cref="UpdateEngineer"/>
    /// <seealso cref="DeleteEngineer"/>
    /// <seealso cref="CreateDependency"/>
    /// <seealso cref="ReadDependency"/>
    /// <seealso cref="ReadAllDependency"/>
    /// <seealso cref="UpdateDependency"/>
    /// <seealso cref="DeleteDependency"/>
    static void SubMenu(string choice)
    {
        int subChoice;

        do
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
                subChoice = int.Parse(Console.ReadLine()!); // Read the user's input and parse it to an integer

                // Process user input based on the main menu choice and submenu option
                switch (choice, subChoice)
                {
                    // Task submenu options
                    case ("Task", 1):
                        CreateTask();
                        break;
                    case ("Task", 2):
                        ReadTask();
                        break;
                    case ("Task", 3):
                        ReadAllTask();
                        break;
                    case ("Task", 4):
                        UpdateTask();
                        break;
                    case ("Task", 5):
                        DeleteTask();
                        break;

                    // Engineer submenu options
                    case ("Engineer", 1):
                        CreateEngineer();
                        break;
                    case ("Engineer", 2):
                        ReadEngineer();
                        break;
                    case ("Engineer", 3):
                        ReadAllEngineer();
                        break;
                    case ("Engineer", 4):
                        UpdateEngineer();
                        break;
                    case ("Engineer", 5):
                        DeleteEngineer();
                        break;

                    // Dependency submenu options
                    case ("Dependency", 1):
                        CreateDependency();
                        break;
                    case ("Dependency", 2):
                        ReadDependency();
                        break;
                    case ("Dependency", 3):
                        ReadAllDependency();
                        break;
                    case ("Dependency", 4):
                        UpdateDependency();
                        break;
                    case ("Dependency", 5):
                        DeleteDependency();
                        break;

                    // Going back to the main menu
                    case ("Task", 0):
                    case ("Engineer", 0):
                    case ("Dependency", 0):
                        break;

                    // Handling invalid input
                    default:
                        Console.WriteLine("Invalid input");
                        break;
                }
            }
            catch (Exception ex)
            {
                // Catch and handle any exceptions that occur during submenu operations
                Console.WriteLine(ex.Message);
                subChoice = 10; // Set subChoice to a non-zero value to avoid exiting the program
            }

        } while (subChoice != 0);
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
    static void CreateTask()
    {
        // Create a new task using the obtained identifier
        Task task = TaskCreateAndUpdate(new());

        // Add the created task to the database using the data access layer
        Console.WriteLine("the new id is: "+ s_dal!.Task.Create(task));
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
    static void ReadTask()
    {
        // Obtain an identifier for the task from the user
        int id = GetId();

        // Read the task from the database using the data access layer
        Task? task = s_dal!.Task.Read(id);

        // Print the task information to the console if found
        if (task == null)
        {
            Console.WriteLine("The task is not exist");
            return;
        }
        Console.WriteLine(task);
    }

    /// <summary>
    /// Reads and displays information about all tasks stored in the database.
    /// </summary>
    /// <remarks>
    /// This method retrieves all tasks from the database using the data access layer's <see cref="Dal.Task.ReadAll"/> method
    /// and prints the information of each task to the console. If there are no tasks in the database, a message indicating
    /// that there are no tasks is displayed.
    /// </remarks>
    static void ReadAllTask()
    {
        Console.WriteLine("All of the tasks:");

        // Retrieve all tasks from the database using the data access layer
        var tasks = s_dal!.Task.ReadAll();

        //Iterate through the tasks and print their information to the console
        foreach (var task in tasks)
        {
            Console.WriteLine(task);
        }
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
    static void UpdateTask()
    {
        // Obtain an identifier for the task from the user
        int id = GetId();

        // Read the existing task from the database using the data access layer
        Task? oldTask = s_dal!.Task.Read(id);

        // Check if the task exists in the database
        if (oldTask == null)
        {
            Console.WriteLine("The task is not exist");
            return;
        }

        // Display the current information of the task to the console
        Console.WriteLine("The old task: \n" + oldTask);

        // Update the task in the database using the data access layer
        s_dal!.Task.Update(TaskCreateAndUpdate(oldTask));
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
    static void DeleteTask()
    {
        // Obtain an identifier for the task from the user
        int id = GetId();

        // Delete the task from the database using the data access layer
        s_dal!.Task.Delete(id);
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
    private static void CreateEngineer()
    {
        // Obtain a unique identifier for the engineer
        int id = GetId();

        // Create a new engineer using the obtained identifier
        Engineer engineer = EngineerCreateAndUpdate(new(), id);

        // Add the created engineer to the database using the data access layer
        Console.WriteLine("the new id is: " + s_dal!.Engineer.Create(engineer));
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
    private static void ReadEngineer()
    {
        // Get the id of the engineer from the user
        int id = GetId();

        // Read the engineer from the database using the data access layer
        Engineer? engineer = s_dal!.Engineer.Read(id);

        if (engineer == null)
        {
            Console.WriteLine("The engineer is not exist");
            return;
        }

        // Print the engineer information to the console
        Console.WriteLine(engineer);
    }

    /// <summary>
    /// Reads and displays information about all engineers stored in the database.
    /// </summary>
    /// <remarks>
    /// This method retrieves all engineers from the database using the data access layer's <see cref="Dal.Engineer.ReadAll"/> method
    /// and prints the information of each engineer to the console. If there are no engineers in the database, a message indicating
    /// that there are no engineers is displayed.
    /// </remarks>
    private static void ReadAllEngineer()
    {
        Console.WriteLine("All of the engineers:");

        // Retrieve all engineers from the database using the data access layer
        var engineers = s_dal!.Engineer.ReadAll();

        // Iterate through the engineers and print their information to the console
        foreach (var engineer in engineers)
        {
            Console.WriteLine(engineer);
        }
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
    private static void UpdateEngineer()
    {
        // Obtain an identifier for the engineer from the user
        int id = GetId();

        // Read the existing engineer from the database using the data access layer
        Engineer? oldEngineer = s_dal!.Engineer.Read(id);

        // Check if the engineer exists in the database
        if (oldEngineer == null)
        {
            Console.WriteLine("The engineer is not exist");
            return;
        }

        // Display the current information of the engineer to the console
        Console.WriteLine("The old engineer:\n" + oldEngineer);

        // Update the engineer in the database using the data access layer
        s_dal!.Engineer.Update(EngineerCreateAndUpdate(oldEngineer, id));
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
    private static void DeleteEngineer()
    {
        // Obtain an identifier for the engineer from the user and delete if found
        int id = GetId();
        s_dal!.Engineer.Delete(id);
    }

    /// <summary>
    /// Creates a new dependency, assigns it a unique identifier, and adds it to the database.
    /// </summary>
    /// <remarks>
    /// This method generates a unique identifier for the dependency using the <see cref="GetId"/> function,
    /// creates a new dependency object through the <see cref="DependencyCreation"/> function, and then adds
    /// the dependency to the database using the data access layer. The dependency creation and database insertion
    /// are performed sequentially within this method.
    /// </remarks>
    /// <seealso cref="GetId"/>
    /// <seealso cref="DependencyCreation"/>
    private static void CreateDependency()
    {
        // Create a new dependency using the obtained identifier and add defulte dependency to work with
        Dependency dependency = DependencyCreatAndUpdate(new());

        // Add the created dependency to the database using the data access layer
        Console.WriteLine("te new id is: " + s_dal!.Dependency.Create(dependency));
    }

    /// <summary>
    /// Reads and displays information about a specific dependency based on the provided identifier.
    /// </summary>
    /// <remarks>
    /// This method prompts the user to enter an identifier for the dependency, retrieves the corresponding
    /// dependency from the database using the data access layer, and then prints the dependency information to
    /// the console. If the dependency is not found in the database, a null value is returned, and a message
    /// indicating that the dependency was not found is displayed.
    /// </remarks>
    /// <seealso cref="GetId"/>
    private static void ReadDependency()
    {
        // Get the id of the dependency from the user
        int id = GetId();

        // Read the dependency from the database using the data access layer
        Dependency? dependency = s_dal!.Dependency.Read(id);

        if (dependency == null)
        {
            Console.WriteLine("The dependency is not exist");
            return;
        }

        // Print the dependency information to the console
        Console.WriteLine(dependency);
    }

    /// <summary>
    /// Reads and displays information about all dependencies stored in the database.
    /// </summary>
    /// <remarks>
    /// This method retrieves all dependencies from the database using the data access layer's <see cref="Dal.Dependency.ReadAll"/> method
    /// and prints the information of each dependency to the console. If there are no dependencies in the database, a message indicating
    /// that there are no dependencies is displayed.
    /// </remarks>
    private static void ReadAllDependency()
    {
        Console.WriteLine("All of the dependencies:");

        // Retrieve all dependencies from the database using the data access layer
        var dependencies = s_dal!.Dependency.ReadAll();

        // Iterate through the dependencies and print their information to the console
        foreach (var dependency in dependencies)
            Console.WriteLine(dependency);

        // Display a message if there are no dependencies in the database
        if (!dependencies.Any())
            Console.WriteLine("No dependencies found.");
    }

    /// <summary>
    /// Updates information for a specific dependency based on the provided identifier.
    /// </summary>
    /// <remarks>
    /// This method prompts the user to enter an identifier for the dependency using the <see cref="GetId"/> function,
    /// retrieves the corresponding dependency from the database using the data access layer, and displays the current
    /// information of the dependency to the console. If the dependency is not found in the database, a message is displayed,
    /// and the function exits. If the dependency exists, the <see cref="DependencyUpdate"/> function is used to create a new
    /// dependency with updated information, and the database is updated with the new dependency.
    /// </remarks>
    /// <seealso cref="GetId"/>
    /// <seealso cref="DependencyUpdate"/>
    private static void UpdateDependency()
    {
        // Obtain an identifier for the dependency from the user
        int id = GetId();

        // Read the existing dependency from the database using the data access layer
        Dependency? oldDependency = s_dal!.Dependency.Read(id);

        // Check if the dependency exists in the database
        if (oldDependency == null)
        {
            Console.WriteLine("The dependency is not exist");
            return;
        }

        // Display the current information of the dependency to the console
        Console.WriteLine("The old dependency:\n" + oldDependency);

        // Update the dependency in the database using the data access layer
        s_dal!.Dependency.Update(DependencyCreatAndUpdate(oldDependency));
    }

    /// <summary>
    /// Deletes a specific dependency from the database based on the provided identifier.
    /// </summary>
    /// <remarks>
    /// This method prompts the user to enter an identifier for the dependency using the <see cref="GetId"/> function
    /// and deletes the corresponding dependency from the database using the data access layer. If the dependency is not found
    /// in the database, no action is taken.
    /// </remarks>
    /// <seealso cref="GetId"/>
    private static void DeleteDependency()
    {
        // Obtain an identifier for the dependency from the user and delete the id if found
        int id = GetId();
        s_dal!.Dependency.Delete(id);
    }

    /// <summary>
    /// Returns a new task with updated values based on the provided old task.
    /// </summary>
    /// <param name="oldTask">The old task to update.</param>
    /// <returns>The updated task with new values.</returns>
    private static Task TaskCreateAndUpdate(Task oldTask)
    {
        // Temporary variables to check if the input is valid for nullable values
        //DateTime temp1;
        //TimeSpan temp2;
        //int temp3;

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

        Console.Write("created at date:");
        if (!DateTime.TryParse(Console.ReadLine(), out DateTime createdAtDate))
            createdAtDate = oldTask.CreatedAtDate;

        Console.Write("scheduled date:");
        DateTime? scheduledDate = oldTask.ScheduledDate;
        if (DateTime.TryParse(Console.ReadLine(), out DateTime temp1))
            scheduledDate = temp1;

        Console.Write("start date:");
        DateTime? startDate = oldTask.StartDate;
        if (DateTime.TryParse(Console.ReadLine(), out temp1))
            startDate = temp1;

        Console.Write("required effort time:");
        if (!TimeSpan.TryParse(Console.ReadLine(), out TimeSpan requiredEffortTime))
            requiredEffortTime = oldTask.RequiredEffortTime;

        Console.Write("deadline date:");
        DateTime? deadlineDate = oldTask.DeadlineDate;
        if (DateTime.TryParse(Console.ReadLine(), out temp1))
            deadlineDate = temp1;

        Console.Write("complete date:");
        DateTime? completeDate = oldTask.CompleteDate;
        if (DateTime.TryParse(Console.ReadLine(), out temp1))
            completeDate = temp1;

        Console.Write("deliverables:");
        string? deliverables = Console.ReadLine();
        if (string.IsNullOrEmpty(deliverables))
            deliverables = oldTask.Deliverables;

        Console.Write("remarks:");
        string? remarks = Console.ReadLine();
        if (string.IsNullOrEmpty(remarks))
            remarks = oldTask.Remarks;

        Console.Write("engineer id:");
        int? engineerId = oldTask.EngineerId;
        if (int.TryParse(Console.ReadLine(), out int temp3))
            engineerId = temp3;

        Console.Write("complexity:");
        EngineerExperience? complexity = oldTask.Complexity;
        if (Enum.TryParse(Console.ReadLine(), out EngineerExperience experience))
            complexity = experience;

        // Create the new task with updated values
        Task task = new(
            oldTask.Id, // Keep the old id or Deafault value
            Alias: alias,
            Description: description,
            CreatedAtDate: createdAtDate,
            IsActive: true, // Placeholder value (default value for boolean property)
            ScheduledDate: scheduledDate,
            StartDate: startDate,
            RequiredEffortTime: requiredEffortTime,
            DeadlineDate: deadlineDate,
            CompleteDate: completeDate,
            Deliverables: deliverables,
            Remarks: remarks,
            EngineerId: engineerId,
            Complexity: complexity
        );
        return task;
    }

    /// <summary>
    /// Returns a new engineer with updated values based on the provided old engineer.
    /// </summary>
    /// <param name="oldEngineer">The engineer to update.</param>
    /// <returns>The updated engineer with new values.</returns>
    private static Engineer EngineerCreateAndUpdate(Engineer oldEngineer, int id)
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
        if (!Enum.TryParse(Console.ReadLine()!, out EngineerExperience level))
            level = oldEngineer.Level;

        // Create the new engineer with updated values
        Engineer engineer = new(
            id, // Keep the old id or put 0 by defaulte
            Cost: cost,
            Name: name,
            Email: email,
            Level: level
        );
        return engineer;
    }

    /// <summary>
    /// Returns a new dependency with updated values based on the provided old dependency.
    /// </summary>
    /// <param name="oldDependency">The dependency to update.</param>
    /// <returns>The updated dependency with new values.</returns>
    private static Dependency DependencyCreatAndUpdate(Dependency oldDependency)
    {
        Console.WriteLine("Enter the values of the dependency:");

        // Get the updated values from user input or use the old values if input is empty
        Console.Write("task id:");
        int taskId = oldDependency.DependentTask;
        if (int.TryParse(Console.ReadLine(), out int dependent))
            taskId = dependent;

        Console.Write("depend on task id:");
        int dependOnTaskId = oldDependency.DependentOnTask;
        if (int.TryParse(Console.ReadLine(), out int dependsOn))
            dependOnTaskId = dependsOn;

        // Create the new dependency with updated values
        Dependency dependency = new(
            Id: oldDependency.Id, // Keep the old id or enter default values
            DependentTask: taskId,
            DependentOnTask: dependOnTaskId
        );
        return dependency;
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

    /// <summary>
    /// The entry point of the program.
    /// Initializes the database and displays the main menu.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    static void Main(string[] args)
    {
        try
        {
            ShowMenu(); // Display the main menu
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine("Press any key to continue");
        Console.ReadKey();
    }
}
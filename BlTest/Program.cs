namespace BlTest;
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

    static int ShowMenu()
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
        //update the start date of the project in the config file
        Dal.Config.StartDate = startProject;
    }
    static void Main(string[] args)
    {
        Console.WriteLine("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");

        if (ans.ToUpper() == "Y")// if the user typed Y or y then create initial data and erase the old data
            Initialization.Do();

        while (ShowMenu() != 0) ;
    }
}

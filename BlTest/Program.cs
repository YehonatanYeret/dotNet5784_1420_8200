namespace BlTest;
using DalTest;
using DO;

enum CHOISE
{
    EXIT,
    TASK,
    ENGINEER,
    DEPENDENCY
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
    static int ShowMenu()
    {
        // Display the menu options to the user
        Console.WriteLine("Choose an option:\n" +
            "0. Exit \n" +
            "1. Task \n" +
            "2. Engineer\n" +
            "3. Dependency");

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
            _ => throw new Exception("Invalid input")
        };
    }

    static int SubMenuForDependency()
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
            SUBCHOISE.CREATE => CreateDependency(),
            SUBCHOISE.READ => ReadDependency(),
            SUBCHOISE.READALL => ReadAllDependency(),
            SUBCHOISE.UPDATE => UpdateDependency(),
            SUBCHOISE.DELETE => DeleteDependency(),
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

    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    static void Main(string[] args)
    {
        Console.WriteLine("Would you like to create Initial data? (Y/N)");
        string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");

        if (ans.ToUpper() == "Y")// if the user typed Y or y then create initial data and erase the old data
            Initialization.Do();

        while (ShowMenu() != 0) ;
    }
}

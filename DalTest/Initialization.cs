﻿namespace DalTest;
using DalApi;
using DO;
public static class Initialization
{
    private static IDal? s_dal; // The database access layer

    private static readonly Random s_rand = new(); // Random number generator

    const int MINCOST = 10000;
    const int MAXCOST = 60000;
    const int MINID = 200000000;
    const int MAXID = 400000000;
    const int MINDAYSPAN = -80;
    const int MAXDAYSPAN = -20;

    /// <summary>
    /// Creates tasks with random data and adds them to the database.
    /// </summary>
    private static void CreateTasks()
    {
        // Alias and description pairs for tasks
        (string, string)[] aliasAndDescription = new (string, string)[] {
        ("digging", "the engineeer need to take control over the digging so we can start build"),
        ("foundations","the engineeer need to take control over the foundation proccess"),
        ("cement","the engineeer need to take controll over the creation of the cement"),
        ("iron","the engineeer need to take control over the iron for the structure"),
        ("brick","the engineeer need to take control over the import of the bricks"),
        ("protective measures","the engineeer need to take control over the buying of the protective measures"),
        ("ladder","the engineeer need to take control over buildig the ladder for the house"),
        ("cranes","the engineeer need to take control moving the cranes"),
        ("paint","the engineeer need to take control over painting the house"),
        ("electricity","the engineeer need to take control over the electric wires and the wiring"),
        ("plumbing","the engineeer need to take control over the plumbing"),
        ("workers","the engineeer need to take control over hirring workers"),
        ("plaster","the engineeer need to take control over the plaster of the building"),
        ("skull","the engineeer need to take control over the skull of the building"),
        ("flooring","the engineeer need to take control over the flooring"),
        ("windows and door","the engineeer need to take control over the exits of the building"),
        ("roof","the engineeer need to control the roof constraction"),
        ("Solar panels","the engineeer need to take control over putting Solar panels"),
        ("Air-Conditioner","the engineeer need to take control the Air-Conditioning"),
        ("rail","the engineeer need to take control over building the rails in the house"),
        ("architect","the engineeer need to take control over the architection"),
        ("Construction permits","the engineeer need to take control over the Construction permits")
        };

        foreach (var (alias, description) in aliasAndDescription)
        {
            // Random date from now to -80 days from -20 days
            DateTime dateTime = DateTime.Now.AddDays(s_rand.Next(MINDAYSPAN, MAXDAYSPAN + 1));

            // Add to new task database
            s_dal!.Task.Create(new Task
        {
            Id = 0,                      // Id will be updated in the creation
            Alias = alias,               // Task alias
            Description = description,   // Task description
            CreatedAtDate = dateTime      // Creation date
        });
        }
    }

    /// <summary>
    /// Creates engineers with random data and adds them to the database.
    /// </summary>
    private static void CreateEngineers()
    {
        // Array of first and last name pairs
        (string, string)[] names = new (string, string)[] {
        ("Adam", "Chohen"), ("Alex", "Charcov"), ("Aaron", "Israeli"),
        ("Ben", "Baruch"), ("David", "Levi"), ("Asaf", "Bir")
    };

        // Array of email domains
        string[] emails = new string[] { "@gmail.com", "@hotmail.com", "@yahoo.com", "@walla.com", "@outlook.com" };

        foreach ((string fName, string lName) in names)
        {
            int id = 0; // Default value
            Engineer? findEngineer;

            // Generate a unique engineer id
            do
            {
                try
                {
                    id = s_rand.Next(MINID, MAXID + 1); // Random id from 200000000 to 400000000
                    findEngineer = s_dal!.Engineer.Read(id);
                }
                catch
                {
                    findEngineer = null;
                }
            } while (findEngineer != null); // Check if id already exists

            int cost = s_rand.Next(MINCOST, MAXCOST + 1); // Random cost from 10000 to 20000
            string email = fName + emails[s_rand.Next(emails.Length)]; // Random email
            EngineerExperience engineerExperience = (EngineerExperience)s_rand.Next(5); // Random engineer experience

            // Create new engineer and add to the database
            s_dal!.Engineer.Create(new Engineer
            {
                Id = id, // Engineer id
                Cost = cost, // Engineer cost
                Name = fName + " " + lName, // Engineer name
                Email = email, // Engineer email
                Level = engineerExperience // Engineer experience level
            });
        }
    }

    /// <summary>
    /// Creates dependencies between tasks and adds them to the database.
    /// </summary>
    private static void CreateDependencies()
    {
        // Array of dependencies where the first number is the id of the task, and the second is the id of the task it depends on
        (int, int)[] dependencies = new (int, int)[] {
        (1, 22), (1, 6), (1, 8), (1, 12),
        (2, 1), (2, 3), (2, 4), (2, 5),
        (3, 12), (4, 22), (5, 22), (6, 12),
        (7, 4), (8, 22), (9, 13), (10, 14),
        (11, 14), (11, 22), (12, 21), (13, 14),
        (13, 3), (13, 10), (13, 7), (14, 2),
        (14, 5), (15, 13), (15, 3), (15, 9),
        (16, 14), (16, 5), (16, 7), (17, 14),
        (17, 5), (17, 7), (18, 17), (19, 14),
        (19, 10), (19, 11), (20, 17), (20, 4), (22, 21)
    };

        foreach ((int dependentTask, int dependentOnTask) in dependencies)
        {
            // Create a new dependency and add it to the database
            s_dal!.Dependency.Create(new Dependency
            {
                Id = 0, // Id is auto-generated
                DependentTask = dependentTask, // Id of the dependent task
                DependentOnTask = dependentOnTask // Id of the task it depends on
            });
        }
    }

    /// <summary>
    /// Initializes the data by creating tasks, engineers, and dependencies and adding them to the database.
    /// </summary>
    /// <param name="dal">The data access layer to use for database operations.</param>
    public static void Do(IDal dal)
    {
        s_dal = dal ?? throw new NullReferenceException("DAL object cannot be null");

        // Create tasks, engineers, and dependencies
        CreateTasks();
        CreateEngineers();
        CreateDependencies();
    }
}
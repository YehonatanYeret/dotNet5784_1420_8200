namespace DalTest;
using DalApi;
using DO;
public static class Initialization
{
    private static ITask? s_dalTask;
    private static IEngineer? s_dalEngineer;
    private static IDependency? s_dalDependency;

    private static readonly Random s_rand = new();

    const int COST = 10000;
    const int ID = 200000000;

    /// <summary>
    /// creat all the tasks
    /// </summary>
    private static void creatTask()
    {
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

        foreach(var (alias, description) in aliasAndDescription)
        {
            DateTime dateTime = DateTime.Now.AddDays(-s_rand.Next(60)-20);//random date from now to -80 days from -20 days
            Task task = new(0, alias, description, dateTime);//create new task
            s_dalTask!.Create(task);//add to data base
        }
    }

    /// <summary>
    /// creat all the engineers
    /// </summary>
    private static void creatEngineer()
    {
        (string, string)[] names = new (string, string)[] { ("Adam", "Chohen"), ("Alex", "Charcov"), ("Aaron", "Israeli"), ("Ben" ,"Baruch"), ("David", "Levi"), ("Asaf", "Bir") };
        string[] emails = new string[] { "@gmail.com", "@hotmail.com", "@yahoo.com", "@walla.com", "@outlook.com" };

        foreach ((string fName, string lName) in names)
        {
            int id=0;// add defaulte value
            Engineer? findEngineer;
            do
            {
                try {
                    id = s_rand.Next(ID, 2*ID + 1);//random id from 200000000 to 400000000
                    findEngineer = s_dalEngineer!.Read(id); 
                }//try to read the engineer with the id
                catch { findEngineer = null;}
            } while (findEngineer != null);//check if id already exist

            int cost = s_rand.Next(COST, 2*COST + 1);//random cost from 10000 to 20000
            string email = fName + emails[s_rand.Next(emails.Length)];//random email
            EngineerExperience engineerExperience = (EngineerExperience)s_rand.Next(5);//random engineerExperience

            Engineer engineer = new(id, cost, fName + " " + lName, email, engineerExperience);//create new engineer

            id = s_dalEngineer!.Create(engineer);//add to data base
            
        }
    }
    /// <summary>
    /// creat all the dependencies
    /// </summary>
    private static void creatDependency()
    {
        //all of the dependencies, first number is the id of the task and the second is the id of the task it depends on:
        (int, int)[] dependencies = new (int, int)[] {
        (1,22),(1,6),(1,8),(1,12),
        (2,1),(2,3),(2,4),(2,5),
        (3,12),(4,22),(5,22),(6,12),
        (7,4),(8,22),(9,13),(10,14),
        (11,14),(11,22),(12,21),(13,14),
        (13,3),(13,10),(13,7),(14,2),
        (14,5),(15,13),(15,3),(15,9),
        (16,14),(16,5),(16,7),(17,14),
        (17,5),(17,7),(18,17),(19,14),
        (19,10),(19,11),(20,17),(20,4),(22,21)};

        foreach ((int Item1, int Item2) in dependencies)
        {
            //id equal 0 because the id is auto generated
            Dependency newDependency = new(0, Item1, Item2);//create new dependency
            s_dalDependency!.Create(newDependency);//add to data base
        }
    }

    /// <summary>
    /// creat all the tasks, engineers and dependencies
    /// </summary>
    /// <param name="dalTask">list of tasks</param>
    /// <param name="dalEngineer">list of engineerss</param>
    /// <param name="dalDependency">list of dependencies</param>
    public static void DO(ITask? dalTask, IEngineer? dalEngineer, IDependency? dalDependency)
    {
        s_dalTask = dalTask?? throw new NullReferenceException("Dal cannot be null!");
        s_dalEngineer = dalEngineer?? throw new NullReferenceException("Dal cannot be null!");
        s_dalDependency = dalDependency?? throw new NullReferenceException("Dal cannot be null!");

        creatTask();
        creatEngineer();
        creatDependency();
    }

}

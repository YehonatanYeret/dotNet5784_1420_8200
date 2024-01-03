namespace DalTest;
using DalApi;
using DO;
public static class Initialization
{
    private static ITask? s_dalTask;
    private static IEngineer? s_dalEngineer;
    private static IDependency? s_dalDependency;

    private static readonly Random s_rand = new();

    private static void creatTask()
    {
     }
    private static void creatEngineer()
    {
        string[] firstName = new string[] { "Adam Chohen", "Alex Charcov", "Aaron Israeli", "Ben Baruch", "Carlo Batusik", "Asaf Lotz"};
        string[] emails = new string[] { "@gmail.com", "@hotmail.com", "@yahoo.com", "@walla.com", "@outlook.com" };

        foreach (string name in firstName)
        {
            int id;
            do
            {
                id = s_rand.Next(200000001) + 200000000;//random id from 200000000 to 400000000
            } while (s_dalEngineer!.Read(id) != null);//check if id already exist

            int cost = s_rand.Next(10001) + 10000;//random cost from 10000 to 20000
            string email = name + emails[s_rand.Next(emails.Length)];//random email
            EngineerExperience engineerExperience = (EngineerExperience)s_rand.Next(5);//random engineerExperience

            Engineer engineer = new(id, cost, name, email, engineerExperience);//create new engineer

            id = s_dalEngineer!.Create(engineer);//add to data base
            
        }
    }
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

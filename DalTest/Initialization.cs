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
        string[] firstName = new string[] { "Adam", "Alex", "Aaron", "Ben", "Carl", "Dan", "David", "Edward", "Fred", "Frank", "George", "Hal", "Hank", "Ike", "John", "Jack", "Joe", "Larry", "Monte", "Matthew", "Mark", "Nathan", "Otto", "Paul", "Peter", "Roger", "Roger", "Steve", "Thomas", "Tim", "Ty", "Victor", "Walter" };
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

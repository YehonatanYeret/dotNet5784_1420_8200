﻿namespace DalTest;
using DalApi;
using Dal;


internal class Program
{
    //new implementation of IDal
    private static IDependency? s_dalDependency = new DependencyImplementation();//stage 1
    private static ITask? s_dalTask = new TaskImplementation();//stage 1
    private static IEngineer? s_dalEngineer = new EngineerImplementation();//stage 1

    static void ShowMenu()
    {
        Console.WriteLine("Choose an option:");
        Console.WriteLine("0. Exit");
        Console.WriteLine("1. task");
        Console.WriteLine("2. engineer");
        Console.WriteLine("3. dependency");
        int? choice = int.Parse(Console.ReadLine()!);

        switch (choice)
        {
            case 0:
                return;
            case 1:
            case 2:     
            case 3:
                SubMenu(choice);
                break;
            default:
                Console.WriteLine("Invalid input");
                break;
        }
    }

    static void SubMenu(int? choice)
    {
        Console.WriteLine("Choose an option:");
        Console.WriteLine("0. Back");
        Console.WriteLine("1. Create");
        Console.WriteLine("2. Read");
        Console.WriteLine("3. ReadAll");
        Console.WriteLine("4. Update");
        Console.WriteLine("5. Delete");

        int? subChoice = int.Parse(Console.ReadLine()!);

        switch (choice, subChoice)
        {
            case (1,1):
                CreateTask();
                break;
            case (1,2):
                ReadTask();
                break;
            case (1,3):
                ReadAllTask();
                break;
            case (1,4):
                UpdateTask();
                break;
            case (1,5):
                DeleteTask();
                break;


            case (2,1):
                CreateEngineer();
                break;
            case (2,2):
                ReadEngineer();
                break;
            case (2,3):
                ReadAllEngineer();
                break;
            case (2,4):
                UpdateEngineer();
                break;
            case (2,5):
                DeleteEngineer();
                break;

            case (3,1):
                CreateDependency();
                break;
            case (3,2): 
                ReadDependency();
                break;
            case (3,3):
                ReadAllDependency();
                break;
            case (3,4):
                UpdateDependency();
                break;
            case (3,5):
                DeleteDependency();
                break;

            case (1,0):
            case (2,0):
            case (3,0):
                return;
 
            default:
                Console.WriteLine("Invalid input");
                break;
        }
    }

    private static void CreateEngineer()
    {
        throw new NotImplementedException();
    }

    static void Main(string[] args)
    {
        Initialization.DO(s_dalTask, s_dalEngineer, s_dalDependency);


       
    }
}

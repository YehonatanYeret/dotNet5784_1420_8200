
namespace Stage0
{
    partial class Program
    {

        private static void Main(string[] args)
        {
            welcome1420();
            welcome8200();
            Console.ReadKey();
        }

        static partial void welcome8200();
        private static void welcome1420()
        {
            Console.Write("Enter your name: ");
            string username = Console.ReadLine();
            Console.WriteLine("{0}, welcome to my first console application", username);
        }
    }
}

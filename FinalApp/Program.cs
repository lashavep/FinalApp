using FinalApp.Models;
using FinalApp.Services;

namespace FinalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("welcome to ATM-Bank App");
            Console.WriteLine("Choose 'register' to create accounts or 'login' to continue");
            Console.ResetColor();
            ConsoleUI consoleUI = new ConsoleUI();
            consoleUI.Run();
        }
    }
}

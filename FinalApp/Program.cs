using System;
using Figgle;
using System.Drawing;
using Console = Colorful.Console;
using Spectre.Console;
using Color = Spectre.Console.Color;
using Spectre.Console.Rendering;

namespace FinalApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "ATM-Bank";
            AnsiConsole.Write(new FigletText("ATM-Bank").Centered().Color(Color.Yellow));
            var line = new Text("\n=========================================\n\n\n").Centered();
            AnsiConsole.Write(line);
            Console.WriteLine("Choose 'register' to create accounts or 'login' to continue");
            Console.ResetColor();
            ConsoleUI consoleUI = new ConsoleUI();
            consoleUI.Run();
        }
    }
}

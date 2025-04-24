
using FinalApp.Models;
using Spectre.Console;

namespace FinalApp.Services
{
    public class BankService
    {
        private readonly UserService _userService;

        public BankService(UserService userService)
        {
            _userService = userService;
        }

        public void DepositToOwnAccount(User user, int accountIndex, decimal amount)
        {
            Console.Clear();
            PrintHeader();
            user.Accounts[accountIndex].Balance += amount;
            _userService.GetUsers().Find(x => x.Username == user.Username)!.Accounts[accountIndex].Balance = user.Accounts[accountIndex].Balance;
            _userService.SaveData();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nAmount deposited successfully. Your balance is {user.Accounts[accountIndex].Balance}\n");
            Console.WriteLine("=================\n");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Do you want to perform another operation?");
            Console.ResetColor();
        }

        public void TransferToOtherAccount(User sender, int senderAccountIndex, User receiver, int receiverAccountIndex, decimal amount)
        {
            Console.Clear();
            PrintHeader();
            if (sender.Accounts[senderAccountIndex].Balance < amount)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Insufficient balance.");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Do you want to perform another operation?");
                Console.ResetColor();
                return;
            }

            sender.Accounts[senderAccountIndex].Balance -= amount;
            receiver.Accounts[receiverAccountIndex].Balance += amount;
            _userService.GetUsers().Find(x => x.Username == sender.Username)!.Accounts[senderAccountIndex].Balance = sender.Accounts[senderAccountIndex].Balance;
            _userService.GetUsers().Find(x => x.Username == receiver.Username)!.Accounts[receiverAccountIndex].Balance = receiver.Accounts[receiverAccountIndex].Balance;
            _userService.SaveData();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Transfer successful. Your balance is {sender.Accounts[senderAccountIndex].Balance} GEL");
            Console.WriteLine("=================\n");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nDo you want to perform another operation?");
            Console.ResetColor();
        }

        public void WithdrawFromOwnAccount(User user, int accountIndex, decimal amount)
        {
            Console.Clear();
            PrintHeader();
            if (user.Accounts[accountIndex].Balance < amount)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Insufficient balance. Your balance is {user.Accounts[accountIndex].Balance} GEL");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nDo you want to perform another operation?");
                Console.ResetColor();
                return;
            }

            user.Accounts[accountIndex].Balance -= amount;
            _userService.GetUsers().Find(x => x.Username == user.Username)!.Accounts[accountIndex].Balance = user.Accounts[accountIndex].Balance;
            _userService.SaveData();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Withdrawal successful. Your balance is {user.Accounts[accountIndex].Balance}  GEL");
            Console.WriteLine("=================\n");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nDo you want to perform another operation?");
            Console.ResetColor();
        }

        public void PrintHeader()
        {
            AnsiConsole.Write(new FigletText("ATM-Bank").Centered().Color(Color.Yellow));
            var line = new Text("\n=========================================\n\n\n").Centered();
            AnsiConsole.Write(line);
        }
    }
}

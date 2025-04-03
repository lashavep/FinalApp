
using FinalApp.Models;

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
            user.Accounts[accountIndex].Balance += amount;
            _userService.GetUsers().Find(x => x.Username == user.Username)!.Accounts[accountIndex].Balance = user.Accounts[accountIndex].Balance;
            _userService.SaveData();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\nAmount deposited successfully. Your balance is {user.Accounts[accountIndex].Balance}\n");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Do you want to perform another operation?");
            Console.ResetColor();
        }

        public void TransferToOtherAccount(User sender, int senderAccountIndex, User receiver, int receiverAccountIndex, decimal amount)
        {
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
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nDo you want to perform another operation?");
            Console.ResetColor();
        }

        public void WithdrawFromOwnAccount(User user, int accountIndex, decimal amount)
        {
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
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nDo you want to perform another operation?");
            Console.ResetColor();
        }
    }
}

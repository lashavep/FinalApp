
using FinalApp.Models;
using FinalApp.Services;

namespace FinalApp
{
    public class ConsoleUI
    {
        private readonly UserService _userService;
        private readonly BankService _bankService;

        public ConsoleUI()
        {
            _userService = new UserService();
            _bankService = new BankService(_userService);
        }

        public void Run()
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit\n");
                Console.ResetColor();

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Register();
                        break;
                    case "2":
                        Login();
                        break;
                    case "3":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Exiting the application. Goodbye!");
                        Console.ResetColor();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice.");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private void Register()
        {
            Console.Write("\nUsername: ");
            string? username = Console.ReadLine();
            Console.Write("Password: ");
            string? password = Console.ReadLine();

            if (username != null && password != null)
            {
                _userService.Register(username, password);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Username and password cannot be empty.");
                Console.ResetColor();
            }
        }

        private void Login()
        {
            Console.Write("Username: ");
            string? username = Console.ReadLine();
            Console.Write("Password: ");
            string? password = Console.ReadLine();

            if (username != null && password != null)
            {
                User? currentUser = _userService.Login(username, password);

                if (currentUser != null)
                {
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\nWelcome, {currentUser.Username}! Your accounts:\n");
                    foreach (var account in currentUser.Accounts)
                    {
                        Console.WriteLine($"- Account Number: {account.AccountNumber}, Balance: {account.Balance} GEL");
                    }
                    Console.ResetColor();
                    MainMenu(currentUser);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect Username or Password. Try again!");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Username and password cannot be empty.");
                Console.ResetColor();
            }
        }

        private void MainMenu(User currentUser)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nChoose operation you want to do:");
                Console.WriteLine("\n1. Deposit to own account");
                Console.WriteLine("2. Transfer to other account");
                Console.WriteLine("3. Withdraw from own account");
                Console.WriteLine("4. Logout\n");
                Console.ResetColor();

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        DepositToOwnAccount(currentUser);
                        break;
                    case "2":
                        TransferToOtherAccount(currentUser);
                        break;
                    case "3":
                        WithdrawFromOwnAccount(currentUser);
                        break;
                    case "4":
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Logged out successfully.\n");
                        Console.ResetColor();
                        return;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid choice. Try again!");
                        Console.ResetColor();
                        break;
                }
            }
        }

        private void DepositToOwnAccount(User user)
        {
            if (!user.Accounts.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No bank accounts found for this user.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Choose account to deposit to:");
            for (int i = 0; i < user.Accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {user.Accounts[i].AccountNumber} (Balance: {user.Accounts[i].Balance})GEL");
            }

            if (int.TryParse(Console.ReadLine(), out int accountIndex) && accountIndex > 0 && accountIndex <= user.Accounts.Count)
            {
                Console.Write("Enter amount to deposit: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                {
                    _bankService.DepositToOwnAccount(user, accountIndex - 1, amount);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid amount.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid account choice.");
                Console.ResetColor();
            }
        }

        private void TransferToOtherAccount(User sender)
        {
            if (!sender.Accounts.Any())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No bank accounts found for this user.");
                Console.ResetColor();
                return;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Choose account to transfer from:");
            Console.ResetColor();
            for (int i = 0; i < sender.Accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {sender.Accounts[i].AccountNumber} (Balance: {sender.Accounts[i].Balance})GEL");
            }

            if (int.TryParse(Console.ReadLine(), out int senderAccountIndex) && senderAccountIndex > 0 && senderAccountIndex <= sender.Accounts.Count)
            {
                Console.Write("Enter recipient's Username: ");
                string? receiverUsername = Console.ReadLine();

                if (string.IsNullOrEmpty(receiverUsername))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Recipient username cannot be empty. Enter username");
                    Console.ResetColor();
                    return;
                }

                User? receiver = _userService.FindUser(receiverUsername);

                if (receiver != null)
                {
                    if (!receiver.Accounts.Any())
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Recipient has no bank accounts.");
                        Console.ResetColor();
                        return;
                    }

                    Console.WriteLine("Choose recipient's account:");
                    for (int i = 0; i < receiver.Accounts.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {receiver.Accounts[i].AccountNumber}");
                    }

                    if (int.TryParse(Console.ReadLine(), out int receiverAccountIndex) && receiverAccountIndex > 0 && receiverAccountIndex <= receiver.Accounts.Count)
                    {
                        Console.Write("Enter amount to transfer: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                        {
                            _bankService.TransferToOtherAccount(sender, senderAccountIndex - 1, receiver, receiverAccountIndex - 1, amount);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid amount.");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid recipient account choice.");
                        Console.ResetColor();
                    }
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Recipient user not found.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor= ConsoleColor.Red;
                Console.WriteLine("Invalid sender account choice.");
                Console.ResetColor();
            }
        }

        private void WithdrawFromOwnAccount(User user)
        {
            if (!user.Accounts.Any())
            {
                Console.ForegroundColor=ConsoleColor.Red;
                Console.WriteLine("No bank accounts found for this user.");
                Console.ResetColor();
                return;
            }

            Console.WriteLine("Choose account to withdraw from:");
            for (int i = 0; i < user.Accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {user.Accounts[i].AccountNumber} (Balance: {user.Accounts[i].Balance})GEL");
            }

            if (int.TryParse(Console.ReadLine(), out int accountIndex) && accountIndex > 0 && accountIndex <= user.Accounts.Count)
            {
                Console.Write("Enter amount to withdraw: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal amount) && amount > 0)
                {
                    _bankService.WithdrawFromOwnAccount(user, accountIndex - 1, amount);
                }
                else
                {
                    Console.ForegroundColor=ConsoleColor.Red;
                    Console.WriteLine("Invalid amount.");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid account choice.");
                Console.ResetColor();
            }
        }
    }
}

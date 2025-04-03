using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                Console.WriteLine("\n1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Register();
                        break;
                    case "2":
                        Login();
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private void Register()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            _userService.Register(username, password);
        }

        private void Login()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            User currentUser = _userService.Login(username, password);

            if (currentUser != null)
            {
                MainMenu(currentUser);
            }
            else
            {
                Console.WriteLine("Incorrect Username or Password.");
            }
        }

        private void MainMenu(User currentUser)
        {
            while (true)
            {
                Console.WriteLine("\nChoose operation:");
                Console.WriteLine("1. Deposit to own account");
                Console.WriteLine("2. Transfer to other account");
                Console.WriteLine("3. Withdraw from own account");
                Console.WriteLine("4. Logout");

                string choice = Console.ReadLine();

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
                        return;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private void DepositToOwnAccount(User user)
        {
            if (!user.Accounts.Any())
            {
                Console.WriteLine("No bank accounts found for this user.");
                return;
            }

            Console.WriteLine("Choose account to deposit to:");
            for (int i = 0; i < user.Accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {user.Accounts[i].AccountNumber} (Balance: {user.Accounts[i].Balance})");
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
                    Console.WriteLine("Invalid amount.");
                }
            }
            else
            {
                Console.WriteLine("Invalid account choice.");
            }
        }

        private void TransferToOtherAccount(User sender)
        {
            if (!sender.Accounts.Any())
            {
                Console.WriteLine("No bank accounts found for this user.");
                return;
            }

            Console.WriteLine("Choose account to transfer from:");
            for (int i = 0; i < sender.Accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {sender.Accounts[i].AccountNumber} (Balance: {sender.Accounts[i].Balance})");
            }

            if (int.TryParse(Console.ReadLine(), out int senderAccountIndex) && senderAccountIndex > 0 && senderAccountIndex <= sender.Accounts.Count)
            {
                Console.Write("Enter recipient's Username: ");
                string receiverUsername = Console.ReadLine();
                User receiver = _userService.FindUser(receiverUsername);

                if (receiver != null)
                {
                    if (!receiver.Accounts.Any())
                    {
                        Console.WriteLine("Recipient has no bank accounts.");
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
                            Console.WriteLine("Invalid amount.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid recipient account choice.");
                    }
                }
                else
                {
                    Console.WriteLine("Recipient user not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid sender account choice.");
            }
        }

        private void WithdrawFromOwnAccount(User user)
        {
            if (!user.Accounts.Any())
            {
                Console.WriteLine("No bank accounts found for this user.");
                return;
            }

            Console.WriteLine("Choose account to withdraw from:");
            for (int i = 0; i < user.Accounts.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {user.Accounts[i].AccountNumber} (Balance: {user.Accounts[i].Balance})");
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
                    Console.WriteLine("Invalid amount.");
                }
            }
            else
            {
                Console.WriteLine("Invalid account choice.");
            }
        }
    }
}

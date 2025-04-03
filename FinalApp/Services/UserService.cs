
using System.Text.Json;
using FinalApp.Models;

namespace FinalApp.Services
{
    public class UserService
    {
        private List<User> _users = new List<User>();
        private const string DataFilePath = @"E:\dev\\c#\FinalApp\FinalApp\bin\Debug\net8.0\Data.json";
        private static int _nextAccountNumberCounter = 1;

        public UserService()
        {
            LoadData();
            
            if (_users.Any() && _users.SelectMany(u => u.Accounts).Any())
            {
                _nextAccountNumberCounter = _users.SelectMany(u => u.Accounts)
                                                 .Max(a => int.Parse(a.AccountNumber.Substring(2))) + 1;
            }
        }

        private void LoadData()
        {
            try
            {
                string json = File.ReadAllText(DataFilePath);
                _users = JsonSerializer.Deserialize<List<User>>(json);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Data.json file not found.");
            }
        }

        public void SaveData()
        {
            string json = JsonSerializer.Serialize(_users);
            File.WriteAllText(DataFilePath, json);
        }

        public void Register(string username, string password)
        {
            if (_users.Any(u => u.Username == username))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("User with this username already exists. Try again!");
                Console.ResetColor();
                return;
            }

            User newUser = new User(username, password);

            
            newUser.Accounts.Add(new BankAccount($"GE{_nextAccountNumberCounter++:D3}", 100.00M)); 
            newUser.Accounts.Add(new BankAccount($"GE{_nextAccountNumberCounter++:D3}", 50.00M));

            _users.Add(newUser);
            SaveData();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Registration successful. Initial bank accounts created.");
            Console.ResetColor();
            Console.WriteLine("\nDo you want to perform another operation?");
        }

        public User Login(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == password);
        }

        public User FindUser(string username)
        {
            return _users.FirstOrDefault(u => u.Username == username);
        }

        public List<User> GetUsers()
        {
            return _users;
        }
    }
}

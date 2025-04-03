

namespace FinalApp.Models
{
    public class BankAccount
    {
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }

        public BankAccount(string accountNumber, decimal balance = 0)
        {
            AccountNumber = accountNumber;
            Balance = balance;
        }
    }
}

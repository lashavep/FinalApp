﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalApp.Models
{
    public class User
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public List<BankAccount> Accounts { get; set; }

        public User(string username, string password)
        {
            Username = username;
            Password = password;
            Accounts = new List<BankAccount>();
        }
    }
}

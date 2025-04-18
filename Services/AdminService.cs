using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystemProject.Models;
using BankSystemProject.Enums;
using BankSystemProject.Interfaces;


namespace BankSystemProject.Services
{
    public class AdminService : IAdminService
    {
        private readonly List<BankUser> _users;
        private readonly AccountService _accountService;

        public delegate void AdminCreatedHandler(BankUser newUser);
        public event AdminCreatedHandler OnUserCreated;

        public delegate void AdminDeletedUserHandler(string username);
        public event AdminDeletedUserHandler OnUserDeleted;

        public delegate void AdminModifiedAccountBalanceHandler(string accountNumber, decimal amount);
        public event AdminModifiedAccountBalanceHandler OnAccountBalanceModified;

        public AdminService(List<BankUser> users, AccountService accountService)
        {
            _users = users;
            _accountService = accountService;
        }

        public bool CreateUser(string username, string password, Roles role)
        {
            if (_users.Any(u => u.Username == username))
            {
                return false;
            }
            else
            {
                var newUser = new BankUser
                {
                    Username = username,
                    Password = password,
                    Roles = new List<Roles> { role }
                };
                _users.Add(newUser);
                OnUserCreated?.Invoke(newUser);
                return true;
            }
        }

        public bool DeleteUser(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user != null)
            {
                _users.Remove(user);
                OnUserDeleted?.Invoke(username);
                return true;
            }
            return false;
        }

        public void ModifyAccontBalance(string accountNumber, decimal amount)
        {
            var balance = _accountService.GetAccountBalance(accountNumber);
            if (balance == 0)
            {
                return;
            }

            if (amount >= 0)
            {
                _accountService.Deposit(accountNumber, amount);
            }
            else
            {
                _accountService.Withdraw(accountNumber, -amount);
            }
            OnAccountBalanceModified?.Invoke(accountNumber, amount);
        }

    }
}

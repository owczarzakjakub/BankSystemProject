using System;
using System.Collections.Generic;
using System.Linq;
using BankSystemProject.Enums;
using BankSystemProject.Models;
using BankSystemProject.Interfaces;


namespace BankSystemProject.Services
{
    public class AccountService : IAccountService
    {
        private readonly List<BankAccount> _accounts;

        public delegate void BalanceChangeHandler(string accountNumber, decimal amount);
        public event BalanceChangeHandler OnBalanceChanged;

        public delegate void WithdrawHandler(string accountNumber, decimal amount, bool success);
        public event WithdrawHandler OnWithdraw;

        public delegate void DepositHandler(string accountNumber, decimal amount);
        public event DepositHandler OnDeposit;

        public BankAccount CreateAccount(Client client)
        {
            var newAccount = new BankAccount
            {
                AccountNumber = Guid.NewGuid().ToString(),
                Balance = 0,
                Owner = client
            };
            _accounts.Add(newAccount);
            return newAccount;
        }
        public bool DeleteAccount(string accountNumber)
        {
            var account = _accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                _accounts.Remove(account);
                return true;
            }
            return false;
        }
        public bool Deposit(string accountNumber, decimal amount)
        {
            var account = _accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account == null || amount <= 0)
            {
                return false;
            }
            account.Balance += amount;
            OnDeposit?.Invoke(accountNumber, amount);
            OnBalanceChanged?.Invoke(accountNumber, amount);
            return true;
        }

        public bool Withdraw(string accountNumber, decimal amount)
        {
            var account = _accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            if (account == null || amount <= 0 || account.Balance < amount)
            {
                return false;
            }
            account.Balance -= amount;
            return true;
        }

        public decimal GetAccountBalance(string accountNumber)
        {
            var account = _accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
            return account?.Balance ?? 0;
        }
        public List<BankAccount> GetAllAccounts()
        {
            return _accounts;
        }
    }
}
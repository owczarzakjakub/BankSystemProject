using System;
using System.Collections.Generic;
using System.Linq;
using BankSystemProject.Enums;
using BankSystemProject.Models;
using BankSystemProject.Interfaces;



namespace BankSystemProject.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly List<Transaction> _transactions = new();
        private readonly AccountService _accountService;

        public void LogTransaction(Transaction transaction)
        {
            _transactions.Add(transaction);
        }    

        public TransactionService(AccountService accountService)
        {
            _accountService = accountService;
        }

        public bool TransferFunds(string fromAccount, string toAccount, decimal amount)
        {
            if(_accountService.Withdraw(fromAccount, amount) && _accountService.Deposit(toAccount, amount))
            {
                LogTransaction(new Transaction
                {
                    FromAccount = fromAccount,
                    ToAccount = toAccount,
                    Amount = amount,
                    TransactionDate = DateTime.Now,
                });
                return true;
            }
            return false;
        }
        public List<Transaction> GetTransactionHistory(string accountNumber)
        {
            return _transactions.Where(t => t.FromAccount == accountNumber || t.ToAccount == accountNumber).ToList();
        }
    }
}
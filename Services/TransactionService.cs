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

        public delegate void TransactioLoggedHandler(Transaction transaction);
        public event TransactioLoggedHandler OnTransactionLogged;

        public delegate void TransactionSuccededHandler(Transaction transaction);
        public event TransactionSuccededHandler OnTransactionSuccess;

        public delegate void TransactionFailedHandler(string fromAccount, string toAccount, decimal amount, string message);
        public event TransactionFailedHandler OnTransactionFailure;


        public TransactionService(AccountService accountService)
        {
            _accountService = accountService;
        }

        public void LogTransaction(Transaction transaction)
        {
            _transactions.Add(transaction);
            OnTransactionLogged?.Invoke(transaction);
        }    
        

        public bool TransferFunds(string fromAccount, string toAccount, decimal amount)
        {
            if(_accountService.Withdraw(fromAccount, amount) && _accountService.Deposit(toAccount, amount))
            {
                var newTransaction = new Transaction
                {
                    FromAccount = fromAccount,
                    ToAccount = toAccount,
                    Amount = amount,
                    TransactionDate = DateTime.Now,
                };
                LogTransaction(newTransaction);
                OnTransactionSuccess(newTransaction);
                return true;
            }
            OnTransactionFailure?.Invoke(fromAccount, toAccount, amount, "Transakcja nieudana z powodu niewystarczaj¹cych srodkow lub nieprawid³owej nazwy konta odbiorcy");
            return false;

        }
        public List<Transaction> GetTransactionHistory(string accountNumber)
        {
            return _transactions.Where(t => t.FromAccount == accountNumber || t.ToAccount == accountNumber).ToList();
        }
    }
}
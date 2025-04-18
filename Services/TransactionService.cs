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
            if (accountService == null)
            {
                throw new ArgumentNullException(nameof(accountService), "AccountService nie mo�e by� null.");
            }
            _accountService = accountService;
        }

        public void LogTransaction(Transaction transaction)
        {
            try
            {
                if (transaction == null)
                {
                    throw new ArgumentNullException(nameof(transaction), "Transakcja nie mo�e by� null.");
                }

                _transactions.Add(transaction);
                OnTransactionLogged?.Invoke(transaction);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wyst�pi� b��d podczas logowania transakcji: {ex.Message}");
            }
        }

        public bool TransferFunds(string fromAccount, string toAccount, decimal amount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fromAccount) || string.IsNullOrWhiteSpace(toAccount))
                {
                    throw new ArgumentException("Numery kont nie mog� by� puste.");
                }

                if (amount <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(amount), "Kwota przelewu musi by� wi�ksza od zera.");
                }

                if (_accountService.Withdraw(fromAccount, amount) && _accountService.Deposit(toAccount, amount))
                {
                    var newTransaction = new Transaction
                    {
                        FromAccount = fromAccount,
                        ToAccount = toAccount,
                        Amount = amount,
                        TransactionDate = DateTime.Now,
                    };
                    LogTransaction(newTransaction);
                    OnTransactionSuccess?.Invoke(newTransaction);
                    return true;
                }

                OnTransactionFailure?.Invoke(fromAccount, toAccount, amount, "Transakcja nieudana z powodu niewystarczaj�cych �rodk�w lub nieprawid�owej nazwy konta odbiorcy.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wyst�pi� b��d podczas transferu �rodk�w: {ex.Message}");
                OnTransactionFailure?.Invoke(fromAccount, toAccount, amount, "Wyst�pi� b��d podczas transferu �rodk�w.");
                return false;
            }
        }

        public List<Transaction> GetTransactionHistory(string accountNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    throw new ArgumentException("Numer konta nie mo�e by� pusty.", nameof(accountNumber));
                }

                return _transactions.Where(t => t.FromAccount == accountNumber || t.ToAccount == accountNumber).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wyst�pi� b��d podczas pobierania historii transakcji: {ex.Message}");
                return new List<Transaction>();
            }
        }
    }
}

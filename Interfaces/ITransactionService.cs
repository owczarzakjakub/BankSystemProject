using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankSystemProject.Models;


namespace BankSystemProject.Interfaces
{
    public interface ITransactionService
    {
        void LogTransaction(Transaction transaction);
        bool TransferFunds(string fromAccount, string toAccount, decimal amount);
        List<Transaction> GetTransactionHistory(string accountNumber);
    }
}
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
        private readonly Dictionary<Client, BankAccount> _bankAccounts = new Dictionary<Client, BankAccount>();

        public delegate void BalanceChangeHandler(string accountNumber, decimal amount);
        public event BalanceChangeHandler OnBalanceChanged;

        public delegate void WithdrawHandler(string accountNumber, decimal amount, bool success);
        public event WithdrawHandler OnWithdraw;

        public delegate void DepositHandler(string accountNumber, decimal amount);
        public event DepositHandler OnDeposit;

        public BankAccount CreateAccount(Client client)
        {
            try
            {
                if (client == null)
                {
                    throw new ArgumentNullException(nameof(client), "Nie można stworzyć konta dla niestniejącego klienta");
                }

                if (_bankAccounts.ContainsKey(client))
                {
                    throw new InvalidOperationException("Klient już posiada konto bankowe.");
                }

                var newAccount = new BankAccount
                {
                    AccountNumber = Guid.NewGuid().ToString(),
                    Balance = 0,
                    Owner = client
                };
                _bankAccounts.Add(client, newAccount);
                return newAccount;
            }

            catch (Exception ex)
            {
                Console.WriteLine("Wystąpił błąd podczas tworzenia konta: " + ex.Message);
                return null;
            }

        }
        public bool DeleteAccount(string accountNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    throw new ArgumentNullException(nameof(accountNumber), "Numer konta nie może być null lub pusty.");
                }

                var account = _bankAccounts.Values.FirstOrDefault(a => a.AccountNumber == accountNumber);
                if (account != null)
                {
                    _bankAccounts.Remove(account.Owner);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas usuwania konta: {ex.Message}");
                return false;
            }
        }

        public bool Deposit(string accountNumber, decimal amount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    throw new ArgumentNullException(nameof(accountNumber), "Numer konta nie może być null lub pusty.");
                }

                if (amount <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(amount), "Kwota wpłaty musi być większa od zera.");
                }

                var account = _bankAccounts.Values.FirstOrDefault(a => a.AccountNumber == accountNumber);
                if (account == null)
                {
                    return false;
                }

                account.Balance += amount;
                OnDeposit?.Invoke(accountNumber, amount);
                OnBalanceChanged?.Invoke(accountNumber, amount);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas wpłaty: {ex.Message}");
                return false;
            }
        }


        public bool Withdraw(string accountNumber, decimal amount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    throw new ArgumentNullException(nameof(accountNumber), "Numer konta nie może być null lub pusty.");
                }

                if (amount <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(amount), "Kwota wypłaty musi być większa od zera.");
                }

                var account = _bankAccounts.Values.FirstOrDefault(a => a.AccountNumber == accountNumber);
                if (account == null)
                {
                    return false;
                }

                if (account.Balance < amount)
                {
                    throw new InvalidOperationException("Saldo konta jest niewystarczające do wykonania wypłaty.");
                }

                account.Balance -= amount;
                OnWithdraw?.Invoke(accountNumber, amount, true);
                OnBalanceChanged?.Invoke(accountNumber, -amount);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas wypłaty: {ex.Message}");
                return false;
            }
        }


        public decimal GetAccountBalance(string accountNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    throw new ArgumentNullException(nameof(accountNumber), "Numer konta nie może być null lub pusty.");
                }

                var account = _bankAccounts.Values.FirstOrDefault(a => a.AccountNumber == accountNumber);
                return account?.Balance ?? 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas pobierania salda konta: {ex.Message}");
                return 0;
            }
        }

        public Dictionary<Client, BankAccount> GetAllAccounts()
        {
            return _bankAccounts;
        }

        public string GetAccountNumber(Client client)
        {
            try
            {
                if (client == null)
                {
                    throw new ArgumentNullException(nameof(client), "Klient nie może być null.");
                }

                if (_bankAccounts.ContainsKey(client))
                {
                    return _bankAccounts[client].AccountNumber;
                }

                throw new InvalidOperationException("Klient nie posiada przypisanego konta.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas pobierania numeru konta: {ex.Message}");
                return null;
            }
        }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using BankSystemProject.Models;
using BankSystemProject.Enums;
using BankSystemProject.Interfaces;

namespace BankSystemProject.Services
{
    public class AdminService : IAdminService
    {
        private readonly AccountService _accountService;
        private readonly Dictionary<Client, BankAccount> _bankAccounts;
        public Operacje_BazyDanych _operacjeBazaDanych;

        public delegate void AdminCreatedHandler(BankUser newUser);
        public event AdminCreatedHandler OnUserCreated;

        public delegate void AdminDeletedUserHandler(string username);
        public event AdminDeletedUserHandler OnUserDeleted;

        public delegate void AdminModifiedAccountBalanceHandler(string accountNumber, decimal amount);
        public event AdminModifiedAccountBalanceHandler OnAccountBalanceModified;

        public AdminService(AccountService accountService, Operacje_BazyDanych operacjeBazaDanych)
        {
            if (accountService == null)
            {
                throw new ArgumentNullException(nameof(accountService), "AccountService nie mo¿e byæ null.");
            }

            if (operacjeBazaDanych == null)
            {
                throw new ArgumentNullException(nameof(operacjeBazaDanych), "Operacje_BazyDanych nie mo¿e byæ null.");
            }

            _accountService = accountService;
            _operacjeBazaDanych = operacjeBazaDanych;
            _bankAccounts = _accountService.GetAllAccounts();
        }

        public bool CreateUser(string username, string password, Roles role)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentException("Nazwa u¿ytkownika i has³o nie mog¹ byæ puste.");
                }

                var existingUsers = _operacjeBazaDanych.ZnajdzRekordy(username);
                if (existingUsers.Count > 0)
                {
                    Console.WriteLine($"U¿ytkownik o nazwie '{username}' ju¿ istnieje.");
                    return false;
                }

                var newUser = new BankUser
                {
                    Username = username,
                    Password = password,
                    Roles = new List<Roles> { role }
                };

                _operacjeBazaDanych.DodajRekord($"{newUser.Username};{newUser.Password};{string.Join(",", newUser.Roles)}");
                OnUserCreated?.Invoke(newUser);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wyst¹pi³ b³¹d podczas tworzenia u¿ytkownika: {ex.Message}");
                return false;
            }
        }

        public bool DeleteUser(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw new ArgumentException("Nazwa u¿ytkownika nie mo¿e byæ pusta.");
                }

                var existingUsers = _operacjeBazaDanych.ZnajdzRekordy(username);

                if (existingUsers.Any(record =>
                {
                    var fields = record.Split(';');
                    return fields.Length > 0 && fields[0] == username;
                }))
                {
                    _operacjeBazaDanych.UsunRekord(username);
                    OnUserDeleted?.Invoke(username);
                    return true;
                }

                Console.WriteLine($"U¿ytkownik o nazwie '{username}' nie istnieje.");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wyst¹pi³ b³¹d podczas usuwania u¿ytkownika: {ex.Message}");
                return false;
            }
        }

        public void ModifyAccontBalance(string accountNumber, decimal amount)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(accountNumber))
                {
                    throw new ArgumentException("Numer konta nie mo¿e byæ pusty.");
                }

                var balance = _accountService.GetAccountBalance(accountNumber);
                if (balance == 0)
                {
                    Console.WriteLine($"Konto o numerze '{accountNumber}' nie istnieje lub ma saldo 0.");
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
            catch (Exception ex)
            {
                Console.WriteLine($"Wyst¹pi³ b³¹d podczas modyfikacji salda konta: {ex.Message}");
            }
        }
    }
}

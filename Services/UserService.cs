using System;
using System.Collections.Generic;
using System.Linq;
using BankSystemProject.Models;
using BankSystemProject.Enums;
using BankSystemProject.Interfaces;

namespace BankSystemProject.Services
{
    public class UserService : IUserService
    {
        private Operacje_BazyDanych _bazaDanych;

        public delegate void UserRegisteredHandler(BankUser user);
        public event UserRegisteredHandler OnUserRegistered;

        public delegate void UserLoginHandler(string username, bool succes);
        public event UserLoginHandler OnUserLogin;

        public UserService(Operacje_BazyDanych bazaDanych)
        {
            if (bazaDanych == null)
            {
                throw new ArgumentNullException(nameof(bazaDanych), "Baza danych nie może być null.");
            }
            _bazaDanych = bazaDanych;
        }

        public bool RegisterUser(string username, string password, string name, string lastName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(lastName))
                {
                    throw new ArgumentException("Wszystkie pola muszą być wypełnione.");
                }

                var existingUsers = _bazaDanych.ZnajdzRekordy(username);
                if (existingUsers.Count > 0)
                {
                    OnUserLogin?.Invoke(username, false);
                    throw new ArgumentException("Użytkownik o podanej nazwie już istnieje.");
                }

                var newUser = new BankUser
                {
                    Username = username,
                    LastName = lastName,
                    Name = name,
                    Password = Operacje_BazyDanych.HashPassword(password),
                    Roles = new List<Roles> { Roles.User }
                };

                _bazaDanych.DodajRekord($"{newUser.Username};{newUser.Password};{newUser.Name};{newUser.LastName};{string.Join(",", newUser.Roles)}");
                OnUserRegistered?.Invoke(newUser);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas rejestracji użytkownika: {ex.Message}");
                return false;
            }
        }

        public BankUser? Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    throw new ArgumentException("Nazwa użytkownika i hasło nie mogą być puste.");
                }

                var hashedPassword = Operacje_BazyDanych.HashPassword(password);
                var records = _bazaDanych.ZnajdzRekordy(username);
                foreach (var record in records)
                {
                    var fields = record.Split(';');
                    if (fields.Length >= 4)
                    {
                        var user = new BankUser
                        {
                            Username = fields[0],
                            Password = fields[1],
                            Name = fields[2],
                            LastName = fields[3]
                            
                        };
                        if (Enum.TryParse<Roles>(fields[4], out Roles role))
                        {
                            user.Roles = new List<Roles> { role };
                        }
                        if (user.Username == username && user.Password == hashedPassword)
                        {
                            OnUserLogin?.Invoke(username, true);
                            return user;
                        }
                    }
                }

                OnUserLogin?.Invoke(username, false);
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas logowania użytkownika: {ex.Message}");
                return null;
            }
        }

        public List<string> GetAllUsers()
        {
            try
            {
                return _bazaDanych.OdczytajRekordy();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas pobierania listy użytkowników: {ex.Message}");
                return new List<string>();
            }
        }
    }
}

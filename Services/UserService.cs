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
        private readonly List<BankUser> _users = new();

        public delegate void UserRegisteredHandler(BankUser user);
        public event UserRegisteredHandler OnUserRegistered;

        public delegate void UserLoginHandler(string username, bool succes);
        public event UserLoginHandler OnUserLogin;

        public UserService() { }

        public bool RegisterUser(string username, string password, string name, string lastName)
        {
            if (_users.Any(u => u.Username == username))
            {
                return false; 
            }
            var newUser = new BankUser
            {
                Username = username,
                LastName = lastName,
                Name = name,
                Password = Operacje_BazyDanych.HashPassword(password),
                Roles = new List<Roles> { Roles.User }

            };
            _users.Add(newUser);
            OnUserRegistered?.Invoke(newUser);
            return true;
        }

        public BankUser? Login(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == Operacje_BazyDanych.HashPassword(password));
            OnUserLogin?.Invoke(username, username != null);
        }

        public List<BankUser> GetAllUsers()
        {
            return _users;
        }

    }
}
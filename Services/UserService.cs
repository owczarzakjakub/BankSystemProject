using System;
using BankSystemProject.Enums;
using System.Collections.Generic;
using System.Linq;
using BankSystemProject.Models;

namespace BankSystemProject.Services
{
    public class UserService
    {
        private readonly List<BankUser> _users = new();

        public UserService()
        {

        }

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
            return true;
        }

        public BankUser? Login(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == Operacje_BazyDanych.HashPassword(password));
        }

        public List<BankUser> GetAllUsers()
        {
            return _users;
        }

    }
}
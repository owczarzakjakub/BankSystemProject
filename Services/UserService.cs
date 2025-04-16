using System;
using BankSystemProject.Enums;
using System.Collections.Generic;
using System.Linq;
using BankSystemProject.Models;

namespace BankSystemProject.Services
{
    public class UserService
    {
        private readonly List<BankUser> _users;

        public UserService()

        public bool RegisterUser(string username, string password)
        {
            if (_users.Any(u => u.Username == username))
            {
                return false; 
            }
            var newUser = new BankUser
            {
                Username = username,
                Password = HashPassword(password),
                Roles = new List<Role> { Role.User }

            };
            _users.Add(newUser);
            return true;
        }

        public BankUser? Login(string username, string password)
        {
            return _users.FirstOrDefault(u => u.Username == username && u.Password == HashPassword(password));
        }

        public List<BankUser> GetAllUsers()
        {
            return _users;
        }

    }
}
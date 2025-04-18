using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankSystemProject.Models;


namespace BankSystemProject.Interfaces
{
    public interface IUserService
    {
        bool RegisterUser(string username, string password, string name, string lastName);
        BankUser Login(string username, string password);
        List<string> GetAllUsers();
    }
}
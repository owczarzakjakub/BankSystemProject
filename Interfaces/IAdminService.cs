using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankSystemProject.Models;


namespace BankSystemProject.Interfaces
{
    public interface IAdminService
    {
        bool CreateUser(string username, string password, Roles role);
        bool DeleteUser(string username);
        void ModifyAccontBalance(string accountNumber, decimal amount);
    }
}
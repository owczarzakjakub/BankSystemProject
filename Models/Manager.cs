using BankSystemProject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemProject.Models
{
    public class Manager : BankUser
    {
        public Manager(string name, string lastName, string username, List<Roles> roles, string password)
        {
            Name = name;
            LastName = lastName;
            Username = username;
            Roles = roles;
            Password = password;
        }
    }
}

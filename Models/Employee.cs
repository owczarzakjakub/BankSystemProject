using BankSystemProject.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemProject.Models
{
    public class Employee : BankUser
    {
        public Employee(string name, string lastName, string username, List<Roles> roles)
        {
            Name = name;
            LastName = lastName;
            Username = username;
            Roles = roles;
        }
    }
}

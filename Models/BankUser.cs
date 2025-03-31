using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemProject.Models
{
    enum Roles
    {
        Admnin,
        User
    }
    internal class BankUser
    {
        public string Username { get; set; }
        private string Password { get; set; }
        public Roles role { get; set; }

        public BankUser(string username, string password, Roles role)
        {
            this.Username = username;
            this.Password = password;
            this.role = role;
        }
        
    }
}

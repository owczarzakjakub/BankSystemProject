using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSystemProject.Models
{
    public class BankAccount
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public Client Owner { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankSystemProject.Models
{
    public class Transaction
    {
        public string TransactionId { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

    }
}
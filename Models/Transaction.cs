using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using namespace BankSystemProject.Models
{
    public class Transaction
    {
        public string TransactionId { get; set; }   
        public string FromaAccount { get; set; }
        public string ToAccount { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }

    }
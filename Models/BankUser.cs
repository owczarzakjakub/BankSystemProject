using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystemProject.Enums;
using System.Text.RegularExpressions;


namespace BankSystemProject.Models
{

    public class BankUser
    {
        private string _name;
        private string _lastName;
        private string _username;
        public List<Roles> Roles;
        public string Password { get; set; }


        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Imie nie moze byc puste.");
                }
                if(Regex.IsMatch(value, @"\d") == true)
                {
                    throw new ArgumentException("Imie nie moze zawierac liczb.");
                }
                if (char.IsLower(value[0]))
                {
                    throw new ArgumentException("Pierwsza litera imienia musi być wielka.");
                }
                _name = value;
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Nazwisko nie moze byc puste.");
                }
                if (Regex.IsMatch(value, @"\d") == true)
                {
                    throw new ArgumentException("Nazwisko nie moze zawierac liczb.");
                }
                if (char.IsLower(value[0]))
                {
                    throw new ArgumentException("Pierwsza litera nazwiska musi być wielka.");
                }
                _lastName = value;
            }
        }
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Nazwa nie moze byc puste.");
                }
                _username = value;
            }
        }
    }
}

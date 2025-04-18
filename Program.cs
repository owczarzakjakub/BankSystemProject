using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystemProject.Models;
using BankSystemProject.Enums;
using BankSystemProject.Services;

namespace BankSystemProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("1");
            var testFile = new Operacje_BazyDanych("C:\\Users\\User\\source\\repos\\BankSystemProject\\Data");
            var userService = new UserService();
            userService.RegisterUser("testUserName", "testPassword", "testFirstName", "testLastName");
            Console.ReadKey();
        }
    }
}

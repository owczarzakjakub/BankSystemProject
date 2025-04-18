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
            var testFile = new Operacje_BazyDanych("C:\\Users\\User\\source\\repos\\BankSystemProject\\Data\\test.txt");
            var userService = new UserService(testFile);
            userService.RegisterUser("YestUserName", "TestPassword", "TestFirstName", "TestLastName");



            //userService.OnUserLogin += (username, success) =>
            //{
            //    if (success)
            //    {
            //        Console.WriteLine($"Użytkownik {username} zalogowany pomyślnie.");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Nieudana próba logowania dla użytkownika {username}.");
            //    }
            //};
            //userService.Login("testUserName", "TestPassword");
            //List<string> usersList = userService.GetAllUsers();
            //foreach (var item in usersList)
            //{
            //    Console.WriteLine(item);
            //}

            //Console.ReadKey();
        }
    }
}

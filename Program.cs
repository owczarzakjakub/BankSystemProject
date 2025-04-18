using System;
using System.Collections.Generic;
using BankSystemProject.Models;
using BankSystemProject.Enums;
using BankSystemProject.Services;

namespace BankSystemProject
{
    internal class Program
    {
        static void Main(string[] args)
        {



            // Inicjalizacja usług
            var operacjeBazaDanych = new Operacje_BazyDanych("C:\\Users\\User\\source\\repos\\BankSystemProject\\Data\\test.txt");
            var userService = new UserService(operacjeBazaDanych);
            var accountService = new AccountService();
            var adminService = new AdminService(accountService, operacjeBazaDanych);
            var rbac = new RBAC();
            BankUser testUser = new BankUser();
            testUser.Username = "testUser";
            testUser.Password = "testPassword";
            testUser.Name = "Jan";
            testUser.LastName = "Kowalski";
            testUser.Roles = new List<Roles> { Roles.User };
            operacjeBazaDanych.DodajRekord($"{testUser.Username};{Operacje_BazyDanych.HashPassword(testUser.Password)};{testUser.Name};{testUser.LastName};{string.Join(",", testUser.Roles)}");
            foreach (var line in operacjeBazaDanych.OdczytajRekordy())
            {
                Console.WriteLine(line);
            }
            Console.ReadKey();

            // Logowanie użytkownika
            Console.WriteLine("Witaj w systemie bankowym!");
            BankUser? loggedInUser = null;

            while (loggedInUser == null)
            {
                Console.Write("Podaj nazwę użytkownika: ");
                string username = Console.ReadLine();
                Console.Write("Podaj hasło: ");
                string password = Console.ReadLine();

                loggedInUser = userService.Login(username, password);


                if (loggedInUser == null)
                {
                    Console.WriteLine("Nieprawidłowa nazwa użytkownika lub hasło. Spróbuj ponownie.");
                }
                foreach(var role in loggedInUser.Roles)
                {
                    Console.WriteLine(role);
                }
                Console.ReadKey();
            }
            

            Console.WriteLine($"Zalogowano jako: {loggedInUser.Username}");

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Wyświetl konta (ViewAccounts)");
                Console.WriteLine("2. Wyświetl transakcje (ViewTransactions)");
                Console.WriteLine("3. Dodaj użytkownika (ManageUsers)");
                Console.WriteLine("4. Wyjdź");

                Console.Write("Wybierz opcję: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        if (rbac.HasPermission(loggedInUser, Permissions.ViewAccounts))
                        {
                            var accounts = accountService.GetAllAccounts();
                            foreach (var account in accounts)
                            {
                                Console.WriteLine($"Konto: {account.Value.AccountNumber}, Saldo: {account.Value.Balance}");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Brak uprawnień do przeglądania kont.");
                        }
                        break;

                    case "2":
                        if (rbac.HasPermission(loggedInUser, Permissions.ViewTransactions))
                        {
                            Console.WriteLine("Wyświetlanie transakcji...");
                        }
                        else
                        {
                            Console.WriteLine("Brak uprawnień do przeglądania transakcji.");
                        }
                        break;

                    case "3":
                        if (rbac.HasPermission(loggedInUser, Permissions.ManageUsers))
                        {
                            Console.Write("Podaj nazwę użytkownika: ");
                            string newUsername = Console.ReadLine();
                            Console.Write("Podaj hasło: ");
                            string newPassword = Console.ReadLine();
                            Console.Write("Podaj rolę (Admin, Manager, Employee, User): ");
                            string roleInput = Console.ReadLine();

                            if (Enum.TryParse(roleInput, out Roles role))
                            {
                                adminService.CreateUser(newUsername, newPassword, role);
                                Console.WriteLine("Użytkownik został dodany.");
                            }
                            else
                            {
                                Console.WriteLine("Nieprawidłowa rola.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Brak uprawnień do zarządzania użytkownikami.");
                        }
                        break;

                    case "4":
                        exit = true;
                        Console.WriteLine("Wylogowano. Do widzenia!");
                        break;

                    default:
                        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                        break;
                }
            }
        }
    }
}


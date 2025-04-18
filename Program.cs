using System;
using System.Collections.Generic;
using BankSystemProject.Enums;
using BankSystemProject.Models;
using BankSystemProject.Services;

namespace BankSystemProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Inicjalizacja usług
                var operacjeBazaDanych = new Operacje_BazyDanych("C:\\Users\\User\\source\\repos\\BankSystemProject\\Data\\test.txt");
                var userService = new UserService(operacjeBazaDanych);
                var accountService = new AccountService();
                var adminService = new AdminService(accountService, operacjeBazaDanych);
                var transactionService = new TransactionService(accountService);
                var rbac = new RBAC();

                // Główne menu
                while (true)
                {
                    Console.WriteLine("Witamy w systemie bankowym!");
                    Console.WriteLine("1. Zaloguj się");
                    Console.WriteLine("2. Zarejestruj nowe konto");
                    Console.Write("Wybierz opcję: ");
                    string mainChoice = Console.ReadLine();

                    if (mainChoice == "1")
                    {
                        BankUser? loggedInUser = Login(userService);
                        if (loggedInUser != null)
                        {
                            ShowUserMenu(loggedInUser, userService, accountService, adminService, transactionService, rbac);
                        }
                    }
                    else if (mainChoice == "2")
                    {
                        RegisterUser(userService);
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił nieoczekiwany błąd: {ex.Message}");
            }
        }

        static BankUser? Login(UserService userService)
        {
            try
            {
                Console.Write("Podaj nazwę użytkownika: ");
                string username = Console.ReadLine();
                Console.Write("Podaj hasło: ");
                string password = Console.ReadLine();

                var user = userService.Login(username, password);
                if (user == null)
                {
                    Console.WriteLine("Nieprawidłowa nazwa użytkownika lub hasło.");
                }
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas logowania: {ex.Message}");
                return null;
            }
        }

        static void RegisterUser(UserService userService)
        {
            try
            {
                Console.Write("Podaj nazwę użytkownika: ");
                string username = Console.ReadLine();
                Console.Write("Podaj hasło: ");
                string password = Console.ReadLine();
                Console.Write("Podaj imię: ");
                string name = Console.ReadLine();
                Console.Write("Podaj nazwisko: ");
                string lastName = Console.ReadLine();

                if (userService.RegisterUser(username, password, name, lastName))
                {
                    Console.WriteLine("Konto zostało pomyślnie zarejestrowane!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Wystąpił błąd podczas rejestracji użytkownika: {ex.Message}");
            }
        }

        static void ShowUserMenu(BankUser user, UserService userService, AccountService accountService, AdminService adminService, TransactionService transactionService, RBAC rbac)
        {
            bool exit = false;
            while (!exit)
            {
                try
                {
                    Console.WriteLine("\nMenu:");
                    Console.WriteLine("1. Wyświetl wszystkich użytkowników");
                    Console.WriteLine("2. Utwórz konto");
                    Console.WriteLine("3. Usuń konto");
                    Console.WriteLine("4. Wpłać środki");
                    Console.WriteLine("5. Wypłać środki");
                    Console.WriteLine("6. Sprawdź saldo konta");
                    Console.WriteLine("7. Wyświetl wszystkie konta");
                    Console.WriteLine("8. Utwórz użytkownika");
                    Console.WriteLine("9. Usuń użytkownika");
                    Console.WriteLine("10. Zmień saldo konta");
                    Console.WriteLine("11. Przelej środki");
                    Console.WriteLine("12. Wyświetl historię transakcji");
                    Console.WriteLine("13. Wyloguj się");
                    Console.Write("Wybierz opcję: ");
                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            if (rbac.HasPermission(user, Permissions.ViewUsers))
                            {
                                var users = userService.GetAllUsers();
                                foreach (var u in users)
                                {
                                    Console.WriteLine(u);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nie masz uprawnień do wyświetlania użytkowników.");
                            }
                            break;

                        case "2":
                            Console.Write("Podaj imię klienta: ");
                            string clientName = Console.ReadLine();
                            Console.Write("Podaj nazwisko klienta: ");
                            string clientLastName = Console.ReadLine();
                            var client = new Client(clientName, clientLastName, user.Username, user.Roles, user.Password);
                            var account = accountService.CreateAccount(client);
                            Console.WriteLine($"Konto zostało utworzone: {account.AccountNumber}");
                            break;

                        case "3":
                            if (rbac.HasPermission(user, Permissions.DeleteAccounts))
                            {
                                Console.Write("Podaj numer konta do usunięcia: ");
                                string accountNumber = Console.ReadLine();
                                if (accountService.DeleteAccount(accountNumber))
                                {
                                    Console.WriteLine("Konto zostało usunięte.");
                                }
                                else
                                {
                                    Console.WriteLine("Nie udało się usunąć konta.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nie masz uprawnień do usuwania kont.");
                            }
                            break;

                        case "4":
                            Console.Write("Podaj numer konta: ");
                            string depositAccount = Console.ReadLine();
                            Console.Write("Podaj kwotę do wpłaty: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount) && depositAmount > 0)
                            {
                                if (accountService.Deposit(depositAccount, depositAmount))
                                {
                                    Console.WriteLine("Wpłata zakończona sukcesem.");
                                }
                                else
                                {
                                    Console.WriteLine("Wpłata nie powiodła się.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nieprawidłowa kwota. Spróbuj ponownie.");
                            }
                            break;

                        case "5":
                            Console.Write("Podaj numer konta: ");
                            string withdrawAccount = Console.ReadLine();
                            Console.Write("Podaj kwotę do wypłaty: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount) && withdrawAmount > 0)
                            {
                                if (accountService.Withdraw(withdrawAccount, withdrawAmount))
                                {
                                    Console.WriteLine("Wypłata zakończona sukcesem.");
                                }
                                else
                                {
                                    Console.WriteLine("Wypłata nie powiodła się.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nieprawidłowa kwota. Spróbuj ponownie.");
                            }
                            break;

                        case "6":
                            Console.Write("Podaj numer konta: ");
                            string balanceAccount = Console.ReadLine();
                            decimal balance = accountService.GetAccountBalance(balanceAccount);
                            Console.WriteLine($"Saldo konta: {balance}");
                            break;

                        case "7":
                            if (rbac.HasPermission(user, Permissions.ViewAccounts))
                            {
                                var accounts = accountService.GetAllAccounts();
                                foreach (var acc in accounts)
                                {
                                    Console.WriteLine($"Konto: {acc.Value.AccountNumber}, Saldo: {acc.Value.Balance}");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nie masz uprawnień do wyświetlania kont.");
                            }
                            break;

                        case "8":
                            if (rbac.HasPermission(user, Permissions.ManageUsers))
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
                                    Console.WriteLine("Użytkownik został utworzony.");
                                }
                                else
                                {
                                    Console.WriteLine("Nieprawidłowa rola.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nie masz uprawnień do tworzenia użytkowników.");
                            }
                            break;

                        case "9":
                            if (rbac.HasPermission(user, Permissions.ManageUsers))
                            {
                                Console.Write("Podaj nazwę użytkownika do usunięcia: ");
                                string deleteUsername = Console.ReadLine();
                                adminService.DeleteUser(deleteUsername);
                                Console.WriteLine("Użytkownik został usunięty.");
                            }
                            else
                            {
                                Console.WriteLine("Nie masz uprawnień do usuwania użytkowników.");
                            }
                            break;

                        case "10":
                            if (rbac.HasPermission(user, Permissions.ModifyAccountBalance))
                            {
                                Console.Write("Podaj numer konta: ");
                                string modifyAccount = Console.ReadLine();
                                Console.Write("Podaj nowe saldo: ");
                                if (decimal.TryParse(Console.ReadLine(), out decimal newBalance))
                                {
                                    adminService.ModifyAccontBalance(modifyAccount, newBalance);
                                    Console.WriteLine("Saldo konta zostało zmienione.");
                                }
                                else
                                {
                                    Console.WriteLine("Nieprawidłowa kwota. Spróbuj ponownie.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nie masz uprawnień do zmiany salda konta.");
                            }
                            break;

                        case "11":
                            Console.Write("Numer twojego konta: ");
                            string fromAccount = Console.ReadLine();
                            Console.Write("Podaj numer konta odbiorcy: ");
                            string toAccount = Console.ReadLine();
                            Console.Write("Podaj kwotę do przelania: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal transferAmount) && transferAmount > 0)
                            {
                                if (transactionService.TransferFunds(fromAccount, toAccount, transferAmount))
                                {
                                    Console.WriteLine("Przelew zakończony sukcesem.");
                                }
                                else
                                {
                                    Console.WriteLine("Przelew nie powiódł się.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nieprawidłowa kwota. Spróbuj ponownie.");
                            }
                            break;

                        case "12":
                            Console.Write("Podaj numer konta: ");
                            string historyAccount = Console.ReadLine();
                            var transactions = transactionService.GetTransactionHistory(historyAccount);
                            if (transactions.Count == 0)
                            {
                                Console.WriteLine("Brak historii transakcji.");
                            }
                            else
                            {
                                foreach (var transaction in transactions)
                                {
                                    Console.WriteLine($"Transakcja: {transaction.TransactionId}, Od: {transaction.FromAccount}, Do: {transaction.ToAccount}, Kwota: {transaction.Amount}, Data: {transaction.TransactionDate}");
                                }
                            }
                            break;

                        case "13":
                            exit = true;
                            Console.WriteLine("Wylogowano. Do widzenia!");
                            break;

                        default:
                            Console.WriteLine("Nieprawidłowa opcja. Spróbuj ponownie.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Wystąpił błąd: {ex.Message}");
                }
            }
        }
    }
}

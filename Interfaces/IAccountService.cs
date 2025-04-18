using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BankSystemProject.Models;


namespace BankSystemProject.Interfaces
{
	public interface IAccountService
	{
		BankAccount CreateAccount(Client client);
		bool DeleteAccount(string accountNumber);
		bool Deposit(string accountNumber, decimal amount);
		bool Withdraw(string accountNumber, decimal amount);
		decimal GetAccountBalance(string accountNumber);
		List<BankAccount> GetAllAccounts();
	}
}
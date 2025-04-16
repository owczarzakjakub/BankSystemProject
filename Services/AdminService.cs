using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankSystemProject.Models;
using BankSystemProject.Enums;


using namespace BankSystemProject.Services
{
    public class AdminService
    {
        private readonly List<BankUser> _users;
        private readonly AccountService _accountService;

        public AdminService(List<BankUser> users, AccountService accountService)
		{
			 _users = users;
            _accountService = accountService;
		}

        public bool CreateUser(string username, string password, Role role)
        {
            if(_users.Any(u => u.Username == username))
            {
                return false;
            }
            else
            {
                _users.Add(new BankUser
				{
					Username = username,
					Password = password,
					Role = role
				});
                return true;
			}
        }

        public bool DeleteUser(string username)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
			if (user != null)
			{
				_users.Remove(user);
				return true;
			}
		}

        public void ModifyAccontBalance(string accountNumber, decimal amount)
        {
            var balance = _accountService.GetAccountBalance(accountNumber);
            if(balance == 0)
            {
                return;
            }

            if(amount >= 0)
            {
                _accountService.Deposit(accountNumber, amount);
			}
			else
			{
				_accountService.Withdraw(accountNumber, -amount);
			}
	} 
      
}

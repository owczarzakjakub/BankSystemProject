using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankSystemProject.Enums
{

    public enum Permissions
    {
        Read,
        Write,
        Delete,
        ManageUsers,
        ViewUsers,
        ViewAccounts,
        ModifyAccountBalance,
        DeleteAccounts,
        ViewTransactions,
        InitiateTransactions,
        ManageRoles,
        ManagePermissions
    }
}

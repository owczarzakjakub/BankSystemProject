using System;
using System.Collections.Generic;
using BankSystemProject.Enums;

namespace BankSystemProject.Models
{
    public class RBAC
    {
        private readonly Dictionary<Roles, List<Permissions>> _permissionsForRoles;

        public RBAC()
        {
            _permissionsForRoles = new Dictionary<Roles, List<Permissions>>
            {
                { Roles.Admin, new List<Permissions>
                    {
                        Permissions.Read,
                        Permissions.Write,
                        Permissions.Delete,
                        Permissions.ManageUsers,
                        Permissions.ViewUsers,
                        Permissions.ViewAccounts,
                        Permissions.ModifyAccountBalance,
                        Permissions.DeleteAccounts,
                        Permissions.ViewTransactions,
                        Permissions.InitiateTransactions,
                        Permissions.ManageRoles,
                        Permissions.ManagePermissions
                    }
                },
                { Roles.Manager, new List<Permissions>
                    {
                        Permissions.Read,
                        Permissions.Write,
                        Permissions.ViewUsers,
                        Permissions.ViewAccounts,
                        Permissions.ModifyAccountBalance,
                        Permissions.ViewTransactions
                    }
                },
                { Roles.Employee, new List<Permissions>
                    {
                        Permissions.Read,
                        Permissions.ViewTransactions,
                        Permissions.InitiateTransactions
                    }
                },
                { Roles.User, new List<Permissions>
                    {
                        Permissions.Read,
                        Permissions.ViewTransactions
                    }
                }
            };
        }

        public bool HasPermission(BankUser user, Permissions permission)
        {
            if (user == null || user.Roles == null)
            {
                return false;
            }

            foreach (var role in user.Roles)
            {
                if (_permissionsForRoles.ContainsKey(role) && _permissionsForRoles[role].Contains(permission))
                {
                    return true;
                }
            }
            return false;
        }

    }
}

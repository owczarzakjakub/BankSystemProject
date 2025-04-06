using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                { Roles.Admin, new List<Permissions>{ Permissions.Read, Permissions.Write, Permissions.Delete, Permissions.ManageUsers} },
                {Roles.Manager, new List<Permissions>{ Permissions.Read, Permissions.Write} },
                {Roles.User, new List<Permissions>{ Permissions.Read} }
            };


            
        }

        public bool HasPermission(BankUser user, Permissions permission)
        {
            foreach (var role in user.Roles)
            {   
                if(_permissionsForRoles.ContainsKey(role) && _permissionsForRoles[role].Contains(permission))
                {
                    return true;
                }
            }
            return false; //!!! trzeba ponziej zmodyfikowac rbac jak dogadamy jakie sa permisje
        }

    }
}

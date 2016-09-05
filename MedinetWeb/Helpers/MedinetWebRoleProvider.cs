using MedinetWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MedinetWeb.Helpers
{
    public class MedinetWebRoleProvider : RoleProvider
    {
        private AccesoRepository repository;

        public MedinetWebRoleProvider()
        {
            this.repository = new AccesoRepository();
        }

        public override bool IsUserInRole(string userName, string roleName)
        {
            MW_Usuarios user = repository.GetUser(userName);
            MW_Roles role = repository.GetRole(roleName);
 
            if (!repository.UserExists(user))
                return false;
            if (!repository.RoleExists(role))
                return false;

            return repository.UserRoleExists(user, role);
        }

        public override string[] GetRolesForUser(string userName)
        {
            List<MW_Roles> roles = this.repository.GetRolesForUser(userName).ToList();

            foreach (MW_Roles role in roles)
            {
                if (!this.repository.RoleExists(role))
                    return new string[] { string.Empty };
            }

            List<string> nameRoles = new List<string>();
            foreach (MW_Roles role in roles)
                nameRoles.Add(role.TX_Rol);

            return nameRoles.ToArray();
        }

        #region Metodos no implementados
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
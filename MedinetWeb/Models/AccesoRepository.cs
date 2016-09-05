using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedinetWeb.Models
{
    public class AccesoRepository
    {
        #region Variables
 
        private MedinetWebEntities entities = new MedinetWebEntities();

        private const string MissingRole = "Rol no existe";
        private const string MissingUser = "Usuario no existe";
        //private const string TooManyUser = "User already exists";
        //private const string TooManyRole = "Role already exists";
        //private const string AssignedRole = "Cannot delete a role with assigned users";
 
        #endregion
 
        #region Properties
 
        public int NumberOfUsers
        {
            get
            {
                return this.entities.MW_Usuarios.Count();
            }
        }
 
        public int NumberOfRoles
        {
            get
            {
                return this.entities.MW_Roles.Count();
            }
        }
 
        #endregion
 
        #region Constructors

        public AccesoRepository()
        {
            this.entities = new MedinetWebEntities();
        }
 
        #endregion
 
        #region Query Methods
        
        /// <summary>
        /// Devuelve todos los usuarios excepto los visitadores, para que dichos usuarios no puedan acceder a MedinetWeb.
        /// </summary>
        /// <returns>IQueryable<MW_Usuarios></returns>
        public IQueryable<MW_Usuarios> GetAllUsers()
        {
            return from usuarios in entities.MW_Usuarios
                   join usuariosRoles in entities.MW_UsuariosRoles
                   on usuarios.ID_Usuario equals usuariosRoles.ID_Usuario
                   where usuariosRoles.ID_Rol != 5
                   select usuarios;

            //return from user in entities.MW_Usuarios
            //       orderby user.TX_Usuario
            //       select user;
        }

        public MW_Usuarios GetUser(int id)
        {
            return entities.MW_Usuarios.SingleOrDefault(user => user.ID_Usuario == id);
        }

        public MW_Usuarios GetUser(string userName)
        {
            return entities.MW_Usuarios.SingleOrDefault(user => user.TX_Usuario == userName);
        }

        public IQueryable<MW_Usuarios> GetUsersForRole(string roleName)
        {
            return GetUsersForRole(GetRole(roleName));
        }

        public IQueryable<MW_Usuarios> GetUsersForRole(int id)
        {
            return GetUsersForRole(GetRole(id));
        }

        public IQueryable<MW_Usuarios> GetUsersForRole(MW_Roles role)
        {
            if (!RoleExists(role))
                throw new ArgumentException(MissingRole);

            return from usuarios in entities.MW_Usuarios
                   join usuariosRoles in entities.MW_UsuariosRoles
                   on usuarios.ID_Usuario equals usuariosRoles.ID_Usuario 
                   where usuariosRoles.ID_Rol == role.ID_Rol
                   select usuarios;
        }
        
        public IQueryable<MW_Roles> GetAllRoles()
        {
            return from role in entities.MW_Roles
                   orderby role.TX_Rol
                   select role;
        }

        public MW_Roles GetRole(int id)
        {
            return entities.MW_Roles.SingleOrDefault(role => role.ID_Rol == id);
        }

        public MW_Roles GetRole(string name)
        {
            return entities.MW_Roles.SingleOrDefault(role => role.TX_Rol == name);
        }

        public IQueryable<MW_Roles> GetRolesForUser(string userName)
        {
            return GetRolesForUser(GetUser(userName));
        }

        public IQueryable<MW_Roles> GetRolesForUser(int id)
        {
            return GetRolesForUser(GetUser(id));
        }

        public IQueryable<MW_Roles> GetRolesForUser(MW_Usuarios user)
        {
            if (!UserExists(user))
                throw new ArgumentException(MissingUser);

            return from roles in entities.MW_Roles
                   join usuariosRoles in entities.MW_UsuariosRoles
                   on roles.ID_Rol equals usuariosRoles.ID_Rol
                   where usuariosRoles.ID_Usuario == user.ID_Usuario
                   select roles;
        }

        #endregion
 
        #region Insert/Delete
 
        //private void AddUser(MW_Usuarios user)
        //{
        //    if (UserExists(user))
        //        throw new ArgumentException(TooManyUser);

        //    entities.MW_Usuarios.AddObject(user);
        //}
 
        //public void CreateUser(string username, string name, string password, string email, string roleName)
        //{
        //    MW_Roles role = GetRole(roleName);
 
        //    if (string.IsNullOrEmpty(username.Trim()))
        //        throw new ArgumentException("The user name provided is invalid. Please check the value and try again.");
        //    if (string.IsNullOrEmpty(name.Trim()))
        //        throw new ArgumentException("The name provided is invalid. Please check the value and try again.");
        //    if (string.IsNullOrEmpty(password.Trim()))
        //        throw new ArgumentException("The password provided is invalid. Please enter a valid password value.");
        //    if (string.IsNullOrEmpty(email.Trim()))
        //        throw new ArgumentException("The e-mail address provided is invalid. Please check the value and try again.");
        //    if (!RoleExists(role))
        //        throw new ArgumentException("The role selected for this user does not exist! Contact an administrator!");
        //    if (this.entities.MW_Usuarios.Any(user => user.TX_Nombre == username))
        //        throw new ArgumentException("Username already exists. Please enter a different user name.");

        //    MW_Usuarios newUser = new MW_Usuarios()
        //    {
        //        TX_Nombre = username,
        //        TX_Apellido = name,
        //        TX_Clave = FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "md5"),
        //        TX_Email1 = email
        //    };
 
        //    try
        //    {
        //        AddUser(newUser);
        //    }
        //    catch (ArgumentException ae)
        //    {
        //        throw ae;
        //    }
        //    catch (Exception e)
        //    {
        //        throw new ArgumentException("The authentication provider returned an error. Please verify your entry and try again. " +
        //            "If the problem persists, please contact your system administrator.");
        //    }
 
        //    // Immediately persist the user data
        //    Save();
        //}

        //public void DeleteUser(MW_Usuarios user)
        //{
        //    if (!UserExists(user))
        //        throw new ArgumentException(MissingUser);

        //    entities.MW_Usuarios.DeleteObject(user);
        //}
 
        //public void DeleteUser(string userName)
        //{
        //    DeleteUser(GetUser(userName));
        //}

        //public void AddRole(MW_Roles role)
        //{
        //    if (RoleExists(role))
        //        throw new ArgumentException(TooManyRole);

        //    entities.MW_Roles.AddObject(role);
        //}
 
        //public void AddRole(string roleName)
        //{
        //    MW_Roles role = new MW_Roles()
        //    {
        //        TX_Rol = roleName
        //    };
 
        //    AddRole(role);
        //}

        //public void DeleteRole(MW_Roles role)
        //{
        //    if (!RoleExists(role))
        //        throw new ArgumentException(MissingRole);
 
        //    if (GetUsersForRole(role).Count() > 0)
        //        throw new ArgumentException(AssignedRole);
 
        //    entities.MW_Roles.DeleteObject(role);
        //}

        //public void DeleteRole(string roleName)
        //{
        //    DeleteRole(GetRole(roleName));
        //}
 
        #endregion
 
        #region Persistence
 
        public void Save()
        {
            entities.SaveChanges();
        }
 
        #endregion
 
        #region Helper Methods

        public bool UserExists(MW_Usuarios user)
        {
            if (user == null)
                return false;

            return (entities.MW_Usuarios.SingleOrDefault(u => u.ID_Usuario == user.ID_Usuario) != null);
        }
 
        public bool RoleExists(MW_Roles role)
        {
            if (role == null)
                return false;

            return (entities.MW_Roles.SingleOrDefault(r => r.ID_Rol == role.ID_Rol) != null);
        }

        public bool UserRoleExists(MW_Usuarios user, MW_Roles role)
        {
            if (user == null)
                return false;

            if (role == null)
                return false;

            return (entities.MW_UsuariosRoles.SingleOrDefault(r => r.ID_Usuario == user.ID_Usuario && r.ID_Rol == role.ID_Rol) != null);
        }

        #endregion
    }
}
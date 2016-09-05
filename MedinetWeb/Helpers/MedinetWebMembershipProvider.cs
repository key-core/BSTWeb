using MedinetWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MedinetWeb.Helpers
{
    public class MedinetWebMembershipProvider : MembershipProvider
    {
        private AccesoRepository repository;

        public override int MinRequiredPasswordLength
        {
            get
            {
                return 6;
            }
        }

        public MedinetWebMembershipProvider()
        {
            this.repository = new AccesoRepository();
        }

        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(password.Trim()) || string.IsNullOrEmpty(username.Trim()))
                return false;

            string hash;
            if (password.Length > 30)
            {
                hash = password.Trim();
            }
            else
            {
                hash = FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "md5");
            }
 
            return this.repository.GetAllUsers().Any(user => (user.TX_Usuario == username) && (user.TX_Clave == hash));
        }
 
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!ValidateUser(username, oldPassword) || string.IsNullOrEmpty(newPassword.Trim()))
                return false;
 
            MW_Usuarios user = repository.GetUser(username);
            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword.Trim(), "md5");
            //string hash = newPassword.Trim();
 
            user.TX_Clave = hash;
            repository.Save();
 
            return true;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            MW_Usuarios user = repository.GetUser(username);
            if (user != null)
            {
                MembershipUser memUser = new MembershipUser("MedinetWebMembershipProvider",
                                               username, user.ID_Usuario, user.TX_Email1,
                                               string.Empty, string.Empty,
                                               true, false, DateTime.MinValue,
                                               DateTime.MinValue,
                                               DateTime.MinValue,
                                               DateTime.Now, DateTime.Now);
                return memUser;
            }
            return null;
        }

        #region Metodos no implementados
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

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        } 
        #endregion
    }
}
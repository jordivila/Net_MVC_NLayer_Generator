using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
using $customNamespace$.Models;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Configuration.ConnectionProviders;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Membership;
using $customNamespace$.Resources.Account;
using $customNamespace$.Resources.UserAdministration;

namespace $customNamespace$.DAL.MembershipServices
{
    public class MembershipDAL : BaseDAL,  IMembershipDAL
    {
        public MembershipDAL()
        {

        }
        public override void Dispose()
        {
            base.Dispose();
        }

        public virtual DataResultUserSearch GetUserList(DataFilterUserList filter)
        {
            Database db = DatabaseFactory.CreateDatabase(Info.GetDatabaseName($customNamespace$.Models.Configuration.ApplicationConfiguration.DatabaseNames.Membership));
            DbCommand cmd = db.GetStoredProcCommand("aspnet_Membership_FindUsersByFilter");
            db.AddInParameter(cmd, "@applicationName", DbType.String, Membership.ApplicationName);
            db.AddInParameter(cmd, "@MinutesSinceLastInActive", DbType.Int32, FormsAuthentication.Timeout.Minutes);
            db.AddInParameter(cmd, "@CurrentTimeUtc", DbType.DateTime, DateTime.UtcNow);
            db.AddInParameter(cmd, "@filter", DbType.Xml, baseModel.Serialize(filter).DocumentElement.OuterXml);

            DataResultUserList pagedList = (DataResultUserList)this.ExecuteReaderForPagedResult<MembershipUserWrapper>(
                                                                        new DataResultUserList()
                                                                        {
                                                                            PageSize = filter.PageSize
                                                                            ,Page = filter.Page
                                                                        }, db, null, cmd);

            pagedList.Data.ForEach(x => x.IsOnline = (DateTime.UtcNow.Subtract(x.LastActivityDate).TotalSeconds < FormsAuthentication.Timeout.TotalSeconds));

            return new DataResultUserSearch() { Data = pagedList };
        }
        public virtual DataResultUserCreateResult CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, string activateFormVirtualPath)
        {
            bool isApproved = false; // we will send an email with token

            MembershipCreateStatus status;

            if (string.IsNullOrEmpty(passwordQuestion))
            {
                Membership.CreateUser(username, password, email, null, null, isApproved, out status);
            }
            else
            {
                Membership.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, out status);
            }

            return new DataResultUserCreateResult() { Data = new CreatedAccountResultModel() { CreateStatus = status } };
        }
        public virtual DataResultBoolean DeleteUser(string username, bool deleteAllRelatedData)
        {
            DataResultUser resultUser = this.GetUserByName(username, false);
            if (resultUser.IsValid)
            {
                if (resultUser.Data != null)
                {
                    resultUser.Data.IsApproved = false;
                    return this.UpdateUser(resultUser.Data);
                }
                else
                {
                    return new DataResultBoolean()
                    {
                        IsValid = false,
                        MessageType = DataResultMessageType.Warning,
                        Data = false,
                        Message = Resources.UserAdministration.UserAdminTexts.UserNotFound
                    };
                }
            }
            else
            {
                return new DataResultBoolean()
                {
                    IsValid = false,
                    MessageType = DataResultMessageType.Warning,
                    Data = false,
                    Message = Resources.UserAdministration.UserAdminTexts.UserNotFound
                };
            }
        }
        public virtual DataResultUser GetUserByGuid(object providerUserKey, bool userIsOnline)
        {
            return new DataResultUser() {
                Data = new MembershipUserWrapper(Membership.GetUser(providerUserKey, userIsOnline))
                , IsValid = true
                , Message = string.Empty
                , MessageType = DataResultMessageType.Success
            };
        }
        public virtual DataResultUser GetUserByName(string username, bool userIsOnline)
        {
            // DO NOT USE CACHING: OTHERWISE UPDATING LastTimeActive WILL NOT BE EXECUTED
            MembershipUser user = Membership.GetUser(username, userIsOnline);
            MembershipUserWrapper resultUser = null;
            if (user != null)
            {
                resultUser = new MembershipUserWrapper(user);
            }
            return new DataResultUser() { Data = resultUser };
        }
        public virtual DataResultBoolean UpdateUser(MembershipUserWrapper user)
        {
            Membership.UpdateUser(user.GetMembershipUser());
            return new DataResultBoolean()
            {
                Data = true,
                Message = Resources.UserAdministration.UserAdminTexts.UserSaved
            };
        }
        public virtual DataResultBoolean ChangePassword(string username, string oldPassword, string newPassword, string newPasswordConfirm)
        {
            bool changed = Membership.GetUser(username).ChangePassword(oldPassword, newPassword);
            return new DataResultBoolean()
            {
                IsValid = changed,
                Data = changed,
                Message = changed ? Resources.Account.AccountResources.CantAccessYourAccount_PasswordChanged : Resources.Account.AccountResources.PasswordCouldNotBeChanged
            };
        }
        public virtual DataResultBoolean UnlockUser(string userName)
        {
            return new DataResultBoolean()
            {
                Data = Membership.GetUser(userName).UnlockUser()
            };
        }
        public virtual DataResultProviderSettings Settings()
        {
            MembershipProviderSettings settings = new MembershipProviderSettings();
            settings.ApplicationName = Membership.ApplicationName;
            settings.EnablePasswordReset = Membership.EnablePasswordReset;
            settings.EnablePasswordRetrieval = Membership.EnablePasswordRetrieval;
            settings.MaxInvalidPasswordAttempts = Membership.MaxInvalidPasswordAttempts;
            settings.MinRequiredNonAlphanumericCharacters = Membership.MinRequiredNonAlphanumericCharacters;
            settings.MinRequiredPasswordLength = Membership.MinRequiredPasswordLength;
            settings.PasswordAttemptWindow = Membership.PasswordAttemptWindow;
            settings.PasswordFormat = Membership.Provider.PasswordFormat;
            settings.PasswordStrengthRegularExpression = Membership.PasswordStrengthRegularExpression;
            settings.RequiresQuestionAndAnswer = Membership.RequiresQuestionAndAnswer;
            settings.RequiresUniqueEmail = Membership.Provider.RequiresUniqueEmail;
            return new DataResultProviderSettings() { 
                Data = settings
            };
        }
        public virtual DataResultUserCantAccess CantAccessYourAccount(string activateFormVirtualPath, string email)
        {
            //DataResultUserCantAccess result;
            DataResultUserCantAccess result;
            MembershipUserCollection usersFound = Membership.FindUsersByEmail(email);
            MembershipUser[] usersResult = new MembershipUser[usersFound.Count];
            usersFound.CopyTo(usersResult, 0);

            List<MembershipUserWrapper> users = (from u in usersResult select new MembershipUserWrapper(u)).ToList();

            if (users.Count > 1)
            {
                throw new Exception(string.Format("Duplicated emails found: {0}", email));
            }

            if (users.Count == 1)
            {
                MembershipUserWrapper user = (MembershipUserWrapper)users.First();
                CantAccessMyAccountModel resultModel = new CantAccessMyAccountModel();
                resultModel.User = user;

                result = new DataResultUserCantAccess() {
                    Data = resultModel,
                    Message = Resources.Account.AccountResources.CantAccessYourAccount_EmailSent
                };
            }
            else
            {
                result = new DataResultUserCantAccess()
                {
                    IsValid = false,
                    Message = Resources.Account.AccountResources.CantAccessYourAccount_EmailNotFound,
                    MessageType = DataResultMessageType.Error
                };
            }
            return result;
        }
        public virtual DataResultUserCantAccess ResetPassword(Guid guid, string newPassword, string confirmNewPassword)
        {
            throw new NotImplementedException();
        }
        public virtual DataResultUserCantAccess ResetPassword(MembershipUserWrapper user, string newPassword, string confirmNewPassword)
        {
            string currentPassword = user.GetMembershipUser().ResetPassword();
            bool result = user.GetMembershipUser().ChangePassword(currentPassword, newPassword);
            CantAccessMyAccountModel resultModel = new CantAccessMyAccountModel();
            resultModel.User = user;
            return new DataResultUserCantAccess() { 
                 Data = resultModel,
                 IsValid = result,
                 Message = AccountResources.CantAccessYourAccount_PasswordChanged
            };
        }
        public virtual DataResultBoolean ValidateUser(string userName, string password)
        {
            bool isValid = Membership.ValidateUser(userName, password);
            return new DataResultBoolean()
            {
                Data = isValid
            };
        }
        public virtual DataResultUserActivate ActivateAccount(MembershipUserWrapper user, Guid activateUserToken)
        {
            MembershipUser userMembership = user.GetMembershipUser();
            userMembership.IsApproved = true;
            Membership.UpdateUser(userMembership);
            return new DataResultUserActivate()
            {
                IsValid = true,
                Message = UserAdminTexts.UserAccountActivated,
                Data = new AccountActivationModel()
                {
                    ActivateUserToken = activateUserToken,
                    User = new MembershipUserWrapper(userMembership)

                }
            };
        }
        public virtual DataResultUserActivate ActivateAccount(Guid activateUserToken)
        {
            // BL uses this other method DataResultUserActivate ActivateAccount(MembershipUserWrapper user, Guid activateUserToken)
            throw new NotImplementedException();
        }
    }
}
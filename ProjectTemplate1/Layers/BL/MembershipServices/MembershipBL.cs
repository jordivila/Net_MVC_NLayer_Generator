using System;
using System.Linq;
using System.Net.Mail;
using System.Transactions;
using System.Web.Security;
using Microsoft.Practices.Unity;
using $safeprojectname$.AuthenticationServices;
using $safeprojectname$.Mailing;
using $safeprojectname$.TokenTemporaryPersistenceServices;
using $customNamespace$.DAL.MembershipServices;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Configuration;
using $customNamespace$.Models.DataAnnotationsAttributes;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Membership;
using $customNamespace$.Models.Profile;
using $customNamespace$.Models.TokenPersistence;
using $customNamespace$.Models.Unity;
using $customNamespace$.Resources.Account;
using $customNamespace$.Resources.UserAdministration;

namespace $safeprojectname$.MembershipServices
{
    public interface IMembershipBL : IProviderMembership
    {
        bool ValidatePasswordStrength(string password);
    }

    public class MembershipBL : BaseBL, IMembershipBL
    {
        private IMembershipDAL _dal;
        public MembershipBL()
        {
            _dal = DependencyFactory.Resolve<IMembershipDAL>();
        }
        public override void Dispose()
        {
            base.Dispose();

            if (this._dal != null)
            {
                this._dal.Dispose();
            }
        }
        private FormsAuthenticationTicket ForceAuthentication(string UserName)
        {
            //try to authenticate user avoiding user to inert login credentials again
            IAuthenticationBL blAuth = new AuthenticationBL();
            FormsAuthenticationTicket ticket = blAuth.SetTicket(UserName);
            blAuth.Dispose();
            return ticket;
        }
        public DataResultUserCantAccess CantAccessYourAccount(string activateFormVirtualPath, string email)
        {
            ITokenTemporaryPersistenceBL tokenServices = null;
            DataResultUserCantAccess dalResult;
            try
            {
                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required,
                                                new TransactionOptions(){
                                                    IsolationLevel = IsolationLevel.ReadUncommitted
                                                }))
                {
                    dalResult = this._dal.CantAccessYourAccount(activateFormVirtualPath, email);
                    if (dalResult.IsValid)
                    {
                        tokenServices = new TokenTemporaryPersistenceBL();
                        TokenTemporaryPersistenceServiceItem token = new TokenTemporaryPersistenceServiceItem(dalResult.Data.User);
                        tokenServices.Insert(token);
                        dalResult.Data.ChangePasswordToken = token.Token;

                        MailingHelper.Send(delegate()
                        {
                            MailMessage mail = new MailMessage();
                            mail.From = new MailAddress(MailingHelper.MailingConfig.SupportTeamEmailAddress);
                            mail.Bcc.Add(new MailAddress(dalResult.Data.User.Email));
                            mail.Subject = AccountResources.CantAccessYourAccount_EmailTitle;
                            mail.Body = string.Format(AccountResources.CantAccessYourAccount_Email,
                                                                        new Uri(string.Format("{0}://{1}/{2}/{3}",
                                                                                                            ApplicationConfiguration.DomainInfoSettingsSection.SecurityProtocol,
                                                                                                            ApplicationConfiguration.DomainInfoSettingsSection.DomainName,
                                                                                                            activateFormVirtualPath,
                                                                                                            dalResult.Data.ChangePasswordToken.ToString())),
                                                                        MailingHelper.DomainConfig.DomainName);
                            return mail;
                        });

                        trans.Complete();
                    }
                    return dalResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (tokenServices != null)
                {
                    tokenServices.Dispose();
                }
            }
        }
        public DataResultUserCantAccess ResetPassword(Guid guid, string newPassword, string confirmNewPassword)
        {
            DataResultUserCantAccess result;

            if (!this.ValidatePasswordStrength(newPassword))
            {
                result = new DataResultUserCantAccess()
                {
                    IsValid = false,
                    Message = Resources.Account.AccountResources.InvalidPassword,
                    MessageType = DataResultMessageType.Error
                };
            }
            else
            {
                if (!newPassword.Equals(confirmNewPassword))
                {
                    result = new DataResultUserCantAccess()
                    {
                        IsValid = false,
                        Message = Resources.Account.AccountResources.NewPasswordConfirmError,
                        MessageType = DataResultMessageType.Error
                    };

                }
                else
                {
                    ITokenTemporaryPersistenceBL tokenServices = null;
                    try
                    {
                        tokenServices = new TokenTemporaryPersistenceBL();
                        object tokenTempItem = tokenServices.GetById(guid);
                        if (tokenTempItem == null)
                        {
                            result = new DataResultUserCantAccess()
                            {
                                IsValid = false,
                                Message = AccountResources.CantAccessYourAccount_TokenExpired,
                                MessageType = DataResultMessageType.Error
                            };
                        }
                        else
                        {
                            using (TransactionScope trans = new TransactionScope())
                            {
                                MembershipUserWrapper userChangingPassword = (MembershipUserWrapper)((TokenTemporaryPersistenceServiceItem)tokenTempItem).TokenObject;
                                result = this._dal.ResetPassword(userChangingPassword, newPassword, confirmNewPassword);
                                if (result.IsValid)
                                {
                                    MailingHelper.Send(delegate()
                                    {
                                        MailMessage mail = new MailMessage();
                                        mail.From = new MailAddress(MailingHelper.MailingConfig.SupportTeamEmailAddress);
                                        mail.Bcc.Add(new MailAddress(result.Data.User.Email));
                                        mail.Subject = string.Format(AccountResources.ChangePassword_EmailTitle, MailingHelper.DomainConfig.DomainName);
                                        mail.Body = string.Format(AccountResources.ChangePassword_EmailBody, MailingHelper.DomainConfig.DomainName);
                                        return mail;
                                    });
                                    tokenServices.Delete(new TokenTemporaryPersistenceServiceItem() { Token = guid });
                                    trans.Complete();
                                    this.ForceAuthentication(userChangingPassword.UserName);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (tokenServices != null)
                        {
                            tokenServices.Dispose();
                        }
                    }
                }
            }
            return result;
        }
        public DataResultBoolean ChangePassword(string username, string oldPassword, string newPassword, string newPasswordConfirm)
        {
            if (!newPassword.Equals(newPasswordConfirm))
            {
                return new DataResultBoolean()
                {
                    IsValid = false,
                    Message = AccountResources.NewPasswordConfirmError,
                    MessageType = DataResultMessageType.Error,
                    Data = false
                };
            }

            if (!this.ValidatePasswordStrength(newPassword))
            {
                return new DataResultBoolean()
                {
                    IsValid = false,
                    Message = AccountResources.InvalidPassword,
                    MessageType = DataResultMessageType.Error,
                    Data = false
                };
            }



            return this._dal.ChangePassword(username, oldPassword, newPassword, newPasswordConfirm);
        }
        public DataResultUserCreateResult CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, string activateFormVirtualPath)
        {
            DataResultUserCreateResult result;
            EmailAttribute emailDataAnnotations = new EmailAttribute();
            if (!emailDataAnnotations.IsValid(email))
            {
                //result = new DataResultUserCreateResult(new CreatedAccountResultModel(MembershipCreateStatus.InvalidEmail));
                result = new DataResultUserCreateResult()
                {
                    IsValid = true,
                    MessageType = DataResultMessageType.Error,
                    Data = new CreatedAccountResultModel(MembershipCreateStatus.InvalidEmail)
                };
            }
            else
            {
                if (this.ValidatePasswordStrength(password))
                {

                    using (TransactionScope trans = new TransactionScope())
                    {
                        result = this._dal.CreateUser(username, password, email, passwordQuestion, passwordAnswer, activateFormVirtualPath);

                        if (result.Data.CreateStatus == MembershipCreateStatus.Success)
                        {
                            MembershipUserWrapper newUser = _dal.GetUserByName(username, false).Data;

                            IRoleAdminBL _roleBl = new RoleAdminBL();
                            DataResultBoolean newUserRole = _roleBl.AddToRoles(newUser.UserName, new string[1] { SiteRoles.Guest.ToString() });
                            _roleBl.Dispose();

                            ProfileBL _profileBL = new ProfileBL();
                            UserProfileModel usrProfile = _profileBL.Create(username).Data;
                            _profileBL.Dispose();

                            ITokenTemporaryPersistenceBL _tokenServices = new TokenTemporaryPersistenceBL();
                            TokenTemporaryPersistenceServiceItem token = new TokenTemporaryPersistenceServiceItem(newUser);
                            _tokenServices.Insert(token);
                            result.Data.ActivateUserToken = token.Token;
                            _tokenServices.Dispose();

                            if (newUserRole.Data)
                            {

                                MailingHelper.Send(delegate()
                                {
                                    MailMessage mail = new MailMessage();
                                    mail.From = new MailAddress(MailingHelper.MailingConfig.SupportTeamEmailAddress);
                                    mail.Bcc.Add(new MailAddress(newUser.Email));
                                    mail.Subject = string.Format(AccountResources.CreateNewAccount_EmailSubject, MailingHelper.DomainConfig.DomainName);
                                    mail.Body = string.Format(AccountResources.CreateNewAccount_EmailBody,
                                                                                MailingHelper.DomainConfig.DomainName,
                                                                                new Uri(string.Format("{0}://{1}/{2}/{3}",
                                                                                                                        ApplicationConfiguration.DomainInfoSettingsSection.SecurityProtocol,
                                                                                                                        ApplicationConfiguration.DomainInfoSettingsSection.DomainName,
                                                                                                                        activateFormVirtualPath.ToString(),
                                                                                                                        result.Data.ActivateUserToken)));
                                    return mail;
                                });

                                trans.Complete();
                            }
                            else
                            {
                                trans.Dispose();
                            }
                        }
                        else
                        {
                            trans.Dispose();
                        }
                    }
                }
                else
                {
                    result = new DataResultUserCreateResult()
                    {
                        IsValid = true,
                        MessageType = DataResultMessageType.Error,
                        Data = new CreatedAccountResultModel(MembershipCreateStatus.InvalidPassword)
                    };

                }
            }
            return result;
        }
        public DataResultBoolean DeleteUser(string username, bool deleteAllRelatedData)
        {
            return this._dal.DeleteUser(username, deleteAllRelatedData);
        }
        public DataResultUser GetUserByGuid(object providerUserKey, bool userIsOnline)
        {
            return this._dal.GetUserByGuid(providerUserKey, userIsOnline);
        }
        public DataResultUser GetUserByName(string username, bool userIsOnline)
        {
            return this._dal.GetUserByName(username, userIsOnline);
        }
        public DataResultUserSearch GetUserList(DataFilterUserList filter)
        {
            TimeSpan timeout = FormsAuthentication.Timeout;
            DataResultUserSearch result = this._dal.GetUserList(filter);
            return result;
        }
        public DataResultProviderSettings Settings()
        {
            return this._dal.Settings();
        }
        public DataResultBoolean UnlockUser(string userName)
        {
            DataResultBoolean result = this._dal.UnlockUser(userName);
            if (result.Data == true)
            {
                result.Message = Resources.UserAdministration.UserAdminTexts.UserUnlocked;
                result.MessageType = DataResultMessageType.Success;
            }
            else
            {
                result.Message = Resources.UserAdministration.UserAdminTexts.UserCouldNotBeUnlocked;
                result.MessageType = DataResultMessageType.Error;
            }
            return result;
        }
        public DataResultBoolean UpdateUser(MembershipUserWrapper user)
        {
            // UserEntity.Email should be Readonly. 
            // Otherwise token should be sent to email address on update in order to verify its ownership
            MembershipUserWrapper userCurrent = this.GetUserByGuid(user.ProviderUserKey, false).Data;
            user.Email = userCurrent.Email;
            // Keep in mind if users are allow to modify their email address:
            // 1.- Verify no duplicates email address are saved
            // 2.- they should re-activate its account by a token sent to their email address
            return this._dal.UpdateUser(user);
        }
        public bool ValidatePasswordStrength(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            MembershipProviderSettings memebrshipSettings = this.Settings().Data;

            int nonAlphaNumericCharacters = (from c in password.ToCharArray()
                                             where !char.IsLetterOrDigit(c)
                                             select c).Count();

            return ((nonAlphaNumericCharacters >= memebrshipSettings.MinRequiredNonAlphanumericCharacters)
                        &&
                        (password.Trim().Length >= memebrshipSettings.MinRequiredPasswordLength));
        }
        public DataResultBoolean ValidateUser(string userName, string passWord)
        {
            return this._dal.ValidateUser(userName, passWord);
        }
        public DataResultUserActivate ActivateAccount(Guid activateUserToken)
        {
            DataResultUserActivate result = null;
            ITokenTemporaryPersistenceBL _tokenServices = new TokenTemporaryPersistenceBL();
            TokenTemporaryPersistenceServiceItem _tokenItem = _tokenServices.GetById(activateUserToken);

            if (_tokenItem == null)
            {
                result = new DataResultUserActivate()
                {
                    IsValid = false,
                    Message = UserAdminTexts.ActivationTokenExpired,
                    Data = new AccountActivationModel()
                     {
                         ActivateUserToken = activateUserToken
                     }
                };
            }
            else
            {
                using (TransactionScope trans = new TransactionScope())
                {
                    try
                    {
                        result = this._dal.ActivateAccount((MembershipUserWrapper)_tokenItem.TokenObject, activateUserToken);

                        _tokenServices.Delete(new TokenTemporaryPersistenceServiceItem() { Token = activateUserToken });

                        trans.Complete();

                        this.ForceAuthentication(((MembershipUserWrapper)_tokenItem.TokenObject).UserName);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        _tokenServices.Dispose();
                    }
                }
            }
            return result;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Membership;
using $customNamespace$.Resources.Account;
using $customNamespace$.Resources.UserAdministration;

namespace $safeprojectname$.MembershipServices
{
    public class MembershipDALMock : IMembershipDAL
    {
        public static object LockObject = new object();
        public static Dictionary<string, MembershipUserWrapper> source = new Dictionary<string, MembershipUserWrapper>();
        public static Dictionary<string, string> sourcePassword = new Dictionary<string, string>();
        public static Dictionary<string, int> sourcePasswordChangeAttempts = new Dictionary<string, int>();

        public DataResultUserSearch GetUserList(DataFilterUserList filter)
        {
            int totalUsers = 0;
            DataResultUserList pagedList = null;

            lock (LockObject)
            {
                #region Search Users
                List<MembershipUserWrapper> usersList = source.Values.Select(x => x).ToList();

                if (filter.Approved != null)
                {
                    usersList = usersList.Where(x => x.IsApproved == filter.Approved).ToList();
                }

                if (filter.CreationDateFrom != null)
                {
                    usersList = usersList.Where(x => new DateTime(x.CreateDate.Year,
                                                                                                        x.CreateDate.Month,
                                                                                                        x.CreateDate.Day)
                                                                                                        >=
                                                                            new DateTime(filter.CreationDateFrom.Value.Year,
                                                                                                    filter.CreationDateFrom.Value.Month,
                                                                                                    filter.CreationDateFrom.Value.Day)).ToList();
                }

                if (filter.CreationDateTo != null)
                {
                    usersList = usersList.Where(x => new DateTime(x.CreateDate.Year,
                                                                                                        x.CreateDate.Month,
                                                                                                        x.CreateDate.Day)
                                                                                                        <=
                                                                            new DateTime(filter.CreationDateTo.Value.Year,
                                                                                                    filter.CreationDateTo.Value.Month,
                                                                                                    filter.CreationDateTo.Value.Day)).ToList();
                }

                if (!string.IsNullOrEmpty(filter.Email))
                {
                    usersList = usersList.Where(x => x.Email.Contains(filter.Email)).ToList();
                }

                //if (filter.IsInRoleName.Count() > 0)
                //{
                //filtrar los usuarios por role
                List<string> usersInRoles = new List<string>();
                foreach (string item in filter.IsInRoleName)
                {
                    usersInRoles.AddRange(Roles.GetUsersInRole(item));
                }
                usersList = usersList.Where(x => usersInRoles.Contains(x.UserName)).ToList();
                //}

                // activar el filtro "IsOnline" es mas molesto que otra cosa. Acaba por entorpecer la experiencia del usuario
                //if (filter.IsOnline != null)
                //{
                //    usersList = usersList.Where(x => x.IsOnline == filter.IsOnline).ToList();
                //}

                if (filter.Locked != null)
                {
                    usersList = usersList.Where(x => x.IsLockedOut == filter.Locked).ToList();
                }

                //if (!string.IsNullOrEmpty(filter.UserName))
                //{
                //usersList = usersList.Where(x => x.UserName.Contains(filter.UserName)).ToList();
                //}

                totalUsers = usersList.Count;
                filter.Page = filter.Page == null ? 0 : filter.Page;

                List<MembershipUserWrapper> pagedUserList = new List<MembershipUserWrapper>();
                for (int pageFrom = ((int)filter.Page * filter.PageSize); pageFrom < ((filter.Page * filter.PageSize) + filter.PageSize); pageFrom++)
                {
                    if (usersList.Count > pageFrom)
                    {
                        pagedUserList.Add(usersList[pageFrom]);
                    }
                }

                pagedList = new DataResultUserList()
                {
                    Data = pagedUserList,
                    PageSize = filter.PageSize,
                    Page = filter.Page,
                    SortAscending = filter.SortAscending,
                    SortBy = filter.SortBy,
                    TotalPages = totalUsers % filter.PageSize > 0 ? (totalUsers / filter.PageSize) + 1 : totalUsers / filter.PageSize,
                    TotalRows = totalUsers
                };
                #endregion
            }

            return new DataResultUserSearch() { Data = pagedList };
        }
        public DataResultUserCreateResult CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, string activateFormVirtualPath)
        {
            //bool isApproved = false; // we will send an email with token

            MembershipCreateStatus status;

            //if (string.IsNullOrEmpty(passwordQuestion))
            //{
            //    Membership.CreateUser(username, password, email, null, null, isApproved, out status);
            //}
            //else
            //{
            //    Membership.CreateUser(username, password, email, passwordQuestion, passwordAnswer, isApproved, out status);
            //}

            MembershipUserWrapper user = new MembershipUserWrapper()
            {
                CreateDate = DateTime.Now,
                Email = email,
                IsApproved = false,
                IsLockedOut = false,
                IsOnline = false,
                LastActivityDate = DateTime.Now,
                LastLockoutDate = DateTime.MinValue,
                LastLoginDate = DateTime.MinValue,
                LastPasswordChangedDate = DateTime.MinValue,
                ProviderName = Membership.Provider.Name,
                PasswordQuestion = passwordQuestion,
                ProviderUserKey = Guid.NewGuid(),
                UserName = username,
                Comment = string.Empty
            };

            if (source.ContainsKey(username))
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }
            else
            {
                source.Add(username, user);
                sourcePassword.Add(username, password);
                sourcePasswordChangeAttempts.Add(username, 0);
                status = MembershipCreateStatus.Success;
            }

            return new DataResultUserCreateResult() { Data = new CreatedAccountResultModel() { CreateStatus = status } };
        }
        public DataResultUser GetUserByGuid(object providerUserKey, bool userIsOnline)
        {
            MembershipUserWrapper resultUser = null;
            lock (LockObject)
            {
                List<MembershipUserWrapper> user = source.Values.Where(x => x.ProviderUserKey == Guid.Parse(providerUserKey.ToString())).ToList();
                if (user.Count > 0)
                {
                    if (userIsOnline)
                    {
                        user.First().LastActivityDate = DateTime.Now;
                    }
                    resultUser = user.First();
                }
                return new DataResultUser() { Data = resultUser };
            }
        }
        public DataResultUser GetUserByName(string username, bool userIsOnline)
        {
            MembershipUserWrapper resultUser = null;
            lock (LockObject)
            {
                List<MembershipUserWrapper> user = source.Values.Where(x => x.UserName == username).ToList();
                if (user.Count > 0)
                {
                    if (userIsOnline)
                    {
                        user.First().LastActivityDate = DateTime.Now;
                    }
                    resultUser = user.First();
                }
                return new DataResultUser() { Data = resultUser };
            }
        }
        public DataResultBoolean UpdateUser(MembershipUserWrapper user)
        {
            source[user.UserName] = user;

            return new DataResultBoolean()
            {
                Data = true,
                Message = Resources.UserAdministration.UserAdminTexts.UserSaved
            };
        }
        public DataResultBoolean ChangePassword(string username, string oldPassword, string newPassword, string newPasswordConfirm)
        {
            bool changed = false;

            lock (LockObject)
            {
                if (sourcePassword[username] == oldPassword)
                {
                    sourcePassword[username] = newPassword;
                    sourcePasswordChangeAttempts[username] = 0;
                    changed = true;
                }
                else
                {
                    // Add Invalid Password Attemp and block user in case MaxAttemps has been raised
                    sourcePasswordChangeAttempts[username] = sourcePasswordChangeAttempts[username] + 1;
                    if (this.Settings().Data.MaxInvalidPasswordAttempts < sourcePasswordChangeAttempts[username])
                    {
                        source[username].IsLockedOut = true;
                        source[username].LastLockoutDate = DateTime.Now;
                    }
                }
            }

            return new DataResultBoolean()
            {
                IsValid = changed,
                Data = changed,
                Message = changed ? Resources.Account.AccountResources.CantAccessYourAccount_PasswordChanged : Resources.Account.AccountResources.PasswordCouldNotBeChanged
            };
        }
        public DataResultBoolean UnlockUser(string userName)
        {
            source[userName].IsLockedOut = false;

            return new DataResultBoolean()
            {
                Data = true
            };
        }
        public DataResultUserCantAccess CantAccessYourAccount(string activateFormVirtualPath, string email)
        {
            //DataResultUserCantAccess result;
            DataResultUserCantAccess result;
            List<MembershipUserWrapper> users = source.Where(x=>x.Value.Email == email).Select(x=>x.Value).ToList();

            if (users.Count > 1)
            {
                throw new Exception(string.Format("Duplicated emails found: {0}", email));
            }

            if (users.Count == 1)
            {
                MembershipUserWrapper user = (MembershipUserWrapper)users.First();
                CantAccessMyAccountModel resultModel = new CantAccessMyAccountModel();
                resultModel.User = user;

                result = new DataResultUserCantAccess()
                {
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
        public DataResultUserCantAccess ResetPassword(Guid guid, string newPassword, string confirmNewPassword)
        {
            throw new NotImplementedException();
        }
        public DataResultUserCantAccess ResetPassword(MembershipUserWrapper user, string newPassword, string confirmNewPassword)
        {

            bool result = true;
            sourcePassword[user.UserName] = newPassword;
            CantAccessMyAccountModel resultModel = new CantAccessMyAccountModel();
            resultModel.User = user;
            return new DataResultUserCantAccess()
            {
                Data = resultModel,
                IsValid = result,
                Message = AccountResources.CantAccessYourAccount_PasswordChanged
            };
        }
        public DataResultBoolean ValidateUser(string userName, string password)
        {
            if (!sourcePassword.ContainsKey(userName))
            {
                return new DataResultBoolean()
                {
                    Data = false
                };
            }
            else
            {

                bool isValid = (sourcePassword[userName] == password) && (source[userName].IsApproved == true);
                return new DataResultBoolean()
                {
                    Data = isValid
                };
            }
        }
        public DataResultUserActivate ActivateAccount(MembershipUserWrapper user, Guid activateUserToken)
        {
            source[user.UserName].IsApproved = true;
            return new DataResultUserActivate()
            {
                IsValid = true,
                Message = UserAdminTexts.UserAccountActivated,
                Data = new AccountActivationModel()
                {
                    ActivateUserToken = activateUserToken,
                    User = source[user.UserName]
                }
            };
        }
        public DataResultUserActivate ActivateAccount(Guid activateUserToken)
        {
            // BL uses this other method DataResultUserActivate ActivateAccount(MembershipUserWrapper user, Guid activateUserToken)
            throw new NotImplementedException();
        }
        public DataResultBoolean DeleteUser(string username, bool deleteAllRelatedData)
        {
            bool result = false;

            lock (LockObject)
            {
                result = source.Remove(username);
            }

            return new DataResultBoolean()
            {
                Data = result,
                IsValid = true,
                Message = string.Empty,
                MessageType = result ? DataResultMessageType.Success : DataResultMessageType.Error
            };
        }
        public DataResultProviderSettings Settings()
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
            return new DataResultProviderSettings()
            {
                Data = settings
            };
        }
        public void Dispose()
        {
            
        }
    }
}

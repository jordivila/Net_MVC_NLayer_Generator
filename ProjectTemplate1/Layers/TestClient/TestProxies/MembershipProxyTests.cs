using System;
using System.Threading;
using System.Web.Security;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Membership;
using $customNamespace$.Models.Roles;
using $customNamespace$.Models.Unity;
using $customNamespace$.Resources.Account;
using $customNamespace$.Tests.Common.Proxies;

namespace $safeprojectname$.TestProxies
{
    [TestClass]
    public class MembershipUnitTests : TestProxyBase
    {
        private static IProviderMembership _memberShipeServices;
        private static IProviderRoleManager _rolesServices;

        private static Guid _userGuid;
        private static string _userName;
        private static string _userEmail;
        private static string _userPwd;
        private static string _userInvalidPassword;

        public MembershipUnitTests()
        {

        }

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            _userGuid = Guid.NewGuid();
            _userName = _userGuid.ToString();
            _userEmail = string.Format("{0}@gmail.com", _userName.Replace("-", string.Empty));
            _userPwd = "1*dk**_=lsdk/()078909";
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            //_memberShipeServices.Dispose();
            //_rolesServices.Dispose();
            //_container.Dispose();
        }

        [TestInitialize()]
        public override void MyTestInitialize()
        {
            base.MyTestInitialize();

            _memberShipeServices = DependencyFactory.Resolve<IProviderMembership>();
            _rolesServices = DependencyFactory.Resolve<IProviderRoleManager>();


            MembershipProviderSettings settings = _memberShipeServices.Settings().Data;
            int nPasswordCharactersLength = settings.MinRequiredPasswordLength;
            int nPasswordNonAlphaNumericCharacters = settings.MinRequiredNonAlphanumericCharacters;
            _userInvalidPassword = string.Empty.PadRight(nPasswordCharactersLength - 1, '8');

        }

        [TestCleanup()]
        public void MyTestCleanup()
        {
            _memberShipeServices.Dispose();
            _rolesServices.Dispose();

        }

        [TestMethod]
        public void MembershipUnitTests_ShouldPass()
        {
            this.CreateUserTest();
            this.CantAccessTest();
            this.ChangePasswordTest();
            this.GetUserTests();
            this.UpdateUserTests();
            this.UnlockUserTests();
            this.DeleteUserTest();
        }

        public void CreateUserTest()
        {
            //Test invalida email address
            DataResultUserCreateResult createdUserStatusInvalidEmail = _memberShipeServices.CreateUser(_userName, _userPwd, "invalidaEmailAddress", string.Empty, string.Empty, "/useraccount/activate");
            Assert.AreEqual(createdUserStatusInvalidEmail.Data.CreateStatus, MembershipCreateStatus.InvalidEmail);

            //Test invalid password
            DataResultUserCreateResult createdUserStatusInvalidPAssword = _memberShipeServices.CreateUser(_userName, string.Empty, _userEmail, string.Empty, string.Empty, "/useraccount/activate");
            Assert.AreEqual(createdUserStatusInvalidPAssword.Data.CreateStatus, MembershipCreateStatus.InvalidPassword);

            //Test success
            DataResultUserCreateResult createdUserStatusSucceed = _memberShipeServices.CreateUser(_userName, _userPwd, _userEmail, string.Empty, string.Empty, "/useraccount/activate");
            Assert.AreEqual(createdUserStatusSucceed.Data.CreateStatus, MembershipCreateStatus.Success);
            DataResultBoolean resultNewUserIsGuest = _rolesServices.IsInRole(_userName, SiteRoles.Guest.ToString());
            Assert.AreEqual(resultNewUserIsGuest.Data, true);

            //Test duplicated username
            DataResultUserCreateResult createdUserStatusDuplicateUserName = _memberShipeServices.CreateUser(_userName, _userPwd, "123" + _userEmail, string.Empty, string.Empty, "/useraccount/activate");
            Assert.AreEqual(createdUserStatusDuplicateUserName.Data.CreateStatus, MembershipCreateStatus.DuplicateUserName);

            ////Test duplicated email
            ////As far as we treat userName & email as a unique field this test will not run
            //DataResultUserCreateResult createdUserStatusDuplicateEmail = _memberShipeServices.CreateUser("123" + _userName, _userPwd, _userEmail, string.Empty, string.Empty, "/useraccount/activate");
            //Assert.AreEqual(createdUserStatusDuplicateEmail.Data.CreateStatus, MembershipCreateStatus.DuplicateEmail);            
        }
        public void CantAccessTest()
        {
            //Test unexisting email address
            DataResultUserCantAccess cantAccessTestFailure = _memberShipeServices.CantAccessYourAccount("/somehost/someUri/", "someUnexistingEmail_qweyiuweyqwieuye@gmail.com");
            Assert.AreEqual(cantAccessTestFailure.IsValid, false);
            Assert.AreEqual(cantAccessTestFailure.Message, AccountResources.CantAccessYourAccount_EmailNotFound);

            //Test success 
            DataResultUserCantAccess cantAccessTestSuccess = _memberShipeServices.CantAccessYourAccount("/somehost/someUri/", _userEmail);
            Assert.AreEqual(cantAccessTestSuccess.IsValid, true);
            Assert.AreEqual(cantAccessTestSuccess.Message, AccountResources.CantAccessYourAccount_EmailSent);

            //Test invalid cnfirm password
            DataResultUserCantAccess cantAccessTestTokenFailure = _memberShipeServices.ResetPassword(cantAccessTestSuccess.Data.ChangePasswordToken, _userPwd, "someDIFFERENTPassword");
            Assert.AreEqual(cantAccessTestTokenFailure.IsValid, false);
            Assert.AreEqual(cantAccessTestTokenFailure.Message, AccountResources.NewPasswordConfirmError);

            //Test unexisting security token
            DataResultUserCantAccess cantAccessTestTokenFailureII = _memberShipeServices.ResetPassword(Guid.NewGuid(), _userPwd, _userPwd);
            Assert.AreEqual(cantAccessTestTokenFailureII.IsValid, false);
            Assert.AreEqual(cantAccessTestTokenFailureII.Message, AccountResources.CantAccessYourAccount_TokenExpired);

            //Test invalid password
            DataResultUserCantAccess cantAccessTestTokenFailureIII = _memberShipeServices.ResetPassword(cantAccessTestSuccess.Data.ChangePasswordToken, _userInvalidPassword, _userInvalidPassword);
            Assert.AreEqual(cantAccessTestTokenFailureIII.IsValid, false);
            Assert.AreEqual(cantAccessTestTokenFailureIII.Message, AccountResources.InvalidPassword);

            //Test success
            //using (new OperationContextScope((IContextChannel)_channel))
            //{
            DataResultUserCantAccess cantAccessTestTokenSuccess = _memberShipeServices.ResetPassword(cantAccessTestSuccess.Data.ChangePasswordToken, _userPwd, _userPwd);
            Assert.AreEqual(cantAccessTestTokenSuccess.IsValid, true);
            Assert.AreEqual(cantAccessTestTokenSuccess.Message, AccountResources.CantAccessYourAccount_PasswordChanged);
            //}
        }
        public void ChangePasswordTest()
        {
            //test password do not match
            DataResultBoolean ChangePasswordFailure = _memberShipeServices.ChangePassword(_userName, _userPwd, _userPwd, string.Empty);
            Assert.AreEqual(ChangePasswordFailure.IsValid, false);
            Assert.AreEqual(ChangePasswordFailure.Message, AccountResources.NewPasswordConfirmError);

            //invalid password
            DataResultBoolean ChangePasswordFailureII = _memberShipeServices.ChangePassword(_userName, _userPwd, _userInvalidPassword, _userInvalidPassword);
            Assert.AreEqual(ChangePasswordFailureII.IsValid, false);
            Assert.AreEqual(ChangePasswordFailureII.Message, AccountResources.InvalidPassword);

            //wrong old password 
            DataResultBoolean ChangePasswordFailureIII = _memberShipeServices.ChangePassword(_userName, _userInvalidPassword, _userPwd, _userPwd);
            Assert.AreEqual(ChangePasswordFailureIII.IsValid, false);
            Assert.AreEqual(ChangePasswordFailureIII.Message, AccountResources.PasswordCouldNotBeChanged);

            //success
            DataResultBoolean ChangePasswordFailureIV = _memberShipeServices.ChangePassword(_userName, _userPwd, _userPwd, _userPwd);
            Assert.AreEqual(ChangePasswordFailureIV.IsValid, true);
            Assert.AreEqual(ChangePasswordFailureIV.Message, AccountResources.CantAccessYourAccount_PasswordChanged);
        }
        public void GetUserTests()
        {
            //test correct user is retrieved by username
            DataResultUser userResultI = _memberShipeServices.GetUserByName(_userName, false);
            Assert.AreEqual(userResultI.IsValid, true);
            Assert.AreEqual(userResultI.Data.UserName, _userName);

            Thread.Sleep(200);

            //update last activity by username
            DataResultUser userResultII = _memberShipeServices.GetUserByName(_userName, true);

            userResultII = _memberShipeServices.GetUserByName(_userName, true);
            Assert.AreEqual(userResultII.IsValid, true);
            Assert.AreEqual(userResultII.Data.UserName, _userName);
            Assert.AreEqual(userResultII.Data.LastActivityDate > userResultI.Data.LastActivityDate, true);

            //test correct user is retrieved by user Key
            object providerUserKey = userResultII.Data.ProviderUserKey;
            DataResultUser userResultIII = _memberShipeServices.GetUserByGuid(providerUserKey, false);
            Assert.AreEqual(userResultIII.IsValid, true);
            Assert.AreEqual(userResultIII.Data.UserName, _userName);

            Thread.Sleep(200);

            //test update last activty by user Key
            DataResultUser userResultIV = _memberShipeServices.GetUserByName(_userName, true);
            userResultIV = _memberShipeServices.GetUserByName(_userName, true);
            Assert.AreEqual(userResultIV.IsValid, true);
            Assert.AreEqual(userResultIV.Data.UserName, _userName);
            Assert.AreEqual(userResultIV.Data.LastActivityDate > userResultIII.Data.LastActivityDate, true);
        }
        public void UpdateUserTests()
        {
            DataResultUser userResult = _memberShipeServices.GetUserByName(_userName, true);
            userResult.Data.Comment = "new comment";
            userResult.Data.IsApproved = !userResult.Data.IsApproved;

            DataResultBoolean userUpdateResultFailure = _memberShipeServices.UpdateUser(userResult.Data);
            Assert.AreEqual(userUpdateResultFailure.IsValid, true);
            Assert.AreEqual(userUpdateResultFailure.Message, Resources.UserAdministration.UserAdminTexts.UserSaved);

            DataResultUser userResultII = _memberShipeServices.GetUserByName(_userName, true);
            Assert.AreEqual(userResultII.Data.Comment, userResult.Data.Comment);
            Assert.AreEqual(userResultII.Data.IsApproved, userResult.Data.IsApproved);
        }
        public void UnlockUserTests()
        {
            MembershipProviderSettings mSettings = _memberShipeServices.Settings().Data;
            MembershipUserWrapper userResultBefore = _memberShipeServices.GetUserByName(_userName, false).Data;
            for (int i = 0; i < mSettings.MaxInvalidPasswordAttempts + 1; i++)
            {
                DataResultBoolean resultChangePassword = _memberShipeServices.ChangePassword(userResultBefore.UserName, _userInvalidPassword, _userPwd, _userPwd);
            }

            MembershipUserWrapper userResultAfter = _memberShipeServices.GetUserByName(_userName, false).Data;
            Assert.AreEqual(userResultAfter.IsLockedOut, true);

            DataResultBoolean resultUnlockUser = _memberShipeServices.UnlockUser(userResultAfter.UserName);
            MembershipUserWrapper userResultAfterUnlock = _memberShipeServices.GetUserByName(_userName, false).Data;
            Assert.AreEqual(userResultAfterUnlock.IsLockedOut, false);
        }
        //[TestMethod]
        //public void GetUserListTests()
        //{

        //    Func<bool?,string[], bool?, string, UserEntity> createUserTemp = delegate (bool? approved, string[] isInRoleName, bool? locked, string userNameFilterValue)
        //    {
        //        Thread.Sleep(1);
        //        Guid userGuid = Guid.NewGuid();
        //        string userName = userGuid.ToString();
        //        string userEmail = string.Format("{0}@gmail.com", userName.Replace("-", string.Empty));
        //        string userPwd = "1*dk**_=lsdk/()078909";

        //        _memberShipeServices.CreateUser(userName, userPwd, userEmail, string.Empty, string.Empty);
        //        UserEntity userTemp = _memberShipeServices.GetUser(userName, false).Data;
        //        return userTemp;
        //    };

        //    // Test filter by single value "Unapproved"
        //    UserEntity unapprovedUser = createUserTemp(false, new string[0], null, string.Empty);
        //    UserListFilterModel filterUnApproved = new UserListFilterModel();
        //    Assert.AreEqual(_memberShipeServices.GetUserList(filterUnApproved, 0, Int32.MaxValue - 1).Data.Users.Where(x => x.UserName == unapprovedUser.UserName).Count() == 1, true);

        //    // WARNING !!! EL TEST ENTRA EN TRANSACTION LOCK !!!!!

        //}
        public void DeleteUserTest()
        {
            DataResultBoolean resultDelete = _memberShipeServices.DeleteUser(_userName, true);
            Assert.AreEqual(resultDelete.Data, true);
        }

    }
}

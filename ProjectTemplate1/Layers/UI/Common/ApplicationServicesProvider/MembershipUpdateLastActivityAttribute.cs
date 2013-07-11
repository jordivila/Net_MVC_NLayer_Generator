using System;
using System.Web.Mvc;
using $customNamespace$.Models.Membership;
using $customNamespace$.Models.UserRequestModel;
using $safeprojectname$.Areas.UserAccount.Controllers;

namespace $safeprojectname$.Common.AspNetApplicationServices
{
    public class MembershipUpdateLastActivityActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (filterContext.Controller.GetType() == typeof(UserAccountBarController))
                {
                    if (MvcApplication.UserRequest.UserIsLoggedIn)
                    {
                        MembershipUserWrapper user = (MvcApplication.UserRequest as IUserRequestClientModel).UserMembership_GetAndUpdateActivity;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
        
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
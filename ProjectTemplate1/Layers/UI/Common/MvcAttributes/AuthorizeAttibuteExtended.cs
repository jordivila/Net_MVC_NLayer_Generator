using System;
using System.Web;
using System.Web.Mvc;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.UserRequestModel;
using $customNamespace$.UI.Web.Areas.Error;

namespace $customNamespace$.UI.Web.Common.Mvc.Attributes
{
    /// <summary>
    /// Custom Authorize Attibute allows an Enum to be used as Role declarations.
    /// 
    /// This line allow an action to be executed by more than one Role at a time
    /// <example>
    /// [CustomAuthorizeAttribute(Roles = Config.UI.Mvc.SiteRoles.Administrator | 
    ///                                                            Config.UI.Mvc.SiteRoles.Guest | 
    ///                                                            Config.UI.Mvc.SiteRoles.Manager)] 
    /// </example>
    /// </summary>
    public class AuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public SiteRoles Roles;

        protected void HandleUnauthorizedRequest(ref AuthorizationContext filterContext)
        {
            UrlHelper urlHelper = null;
            if (filterContext.Controller is Controller)
            {
                urlHelper = ((Controller)filterContext.Controller).Url;
            }
            else
            {
                urlHelper = new UrlHelper(filterContext.RequestContext);
            }

            bool isAjaxRequest = filterContext.RequestContext.HttpContext.Request.IsAjaxRequest();

            Func<string, JsonResult> setResult = delegate(string msg)
            {
                DataResultBoolean response = new DataResultBoolean()
                {
                    IsValid = false,
                    Message = msg,
                    MessageType = DataResultMessageType.Error,
                    Data = false
                };
                JsonResult result = new JsonResult();
                result.Data = response;
                return result;
            };

            if (!this.CheckSessionExpired(filterContext.HttpContext))
            {
                if (isAjaxRequest)
                {
                    filterContext.Result = setResult($customNamespace$.Resources.General.GeneralTexts.SessionExpired);
                }
                else
                {
                    filterContext.Result = new RedirectResult(ErrorUrlHelper.SessionExpired(urlHelper));
                }
            }

            // break execution in case a result has been already set
            if (filterContext.Result != null)
                return;

            if (!this.CheckIsAutohrized(filterContext.HttpContext))
            {
                if (isAjaxRequest)
                {
                    filterContext.Result = setResult($customNamespace$.Resources.General.GeneralTexts.PermissionDenied);
                }
                else
                {
                    filterContext.Result = new RedirectResult(ErrorUrlHelper.UnAuthorized(urlHelper));
                }
            }
        }

        private bool CheckSessionExpired(HttpContextBase ctx)
        {
            bool isAuth = true;
            // check if session is supported
            if (ctx.Session != null)
            {
                // check if a new session id was generated
                if (ctx.Session.IsNewSession)
                {
                    // If it says it is a new session, but an existing cookie exists, then it must have timed out
                    string sessionCookie = ctx.Request.Headers["Cookie"];
                    if ((null != sessionCookie) && (sessionCookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        isAuth = false;
                    }
                }
            }
            return isAuth;
        }

        private bool CheckIsAutohrized(HttpContextBase httpContext)
        {
            bool result = false;

            //IUserRequestModel<HttpContext, HttpCookieCollection> userRequest = null;



            try
            {
                //userRequest = DependencyFactory.Unity.Resolve<IUserRequestModel<HttpContext, HttpCookieCollection>>();
                if (!MvcApplication.UserRequest.UserIsLoggedIn)
                {
                    result = false;
                }
                else
                {
                    if (Roles > 0)
                    {
                        string[] userroles = MvcApplication.UserRequest.UserRoles;

                        foreach (string userrole in userroles)
                        {

                            SiteRoles role;

                            bool parseSucceed = Enum.TryParse<SiteRoles>(userrole, out role);

                            if (parseSucceed)
                            {
                                if ((Roles & role) == role)
                                {
                                    result = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        // Or validate Users
                        result = true;
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //if (userRequest != null)
                //{
                //userRequest.Dispose();
                //}
            }
        }

        protected bool IsAuthorize(HttpContextBase httpContext)
        {
            return this.CheckSessionExpired(httpContext) && this.CheckIsAutohrized(httpContext);
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
            {
                throw new ArgumentNullException("filterContext");
            }

            if (this.IsAuthorize(filterContext.HttpContext))
            {
                //filterContext.Result = new HttpStatusCodeResult(200);
            }
            else
            {
                this.HandleUnauthorizedRequest(ref filterContext);
                filterContext.Result = filterContext.Result;
            }
            return;
        }
    }
}
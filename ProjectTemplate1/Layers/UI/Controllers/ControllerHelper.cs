using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using $customNamespace$.Models.UserRequestModel;

namespace $customNamespace$.UI.Web.Controllers
{
    public static class ControllerHelper
    {
        //private static IUserRequestModel<HttpContext, HttpCookieCollection> _userRequest = null;
        //public static IUserRequestModel<HttpContext, HttpCookieCollection> UserRequest
        //{
        //    get
        //    { 
        //        if(_userRequest==null)
        //        {
        //            _userRequest = DependencyFactory.Unity.Resolve<IUserRequestModel<HttpContext, HttpCookieCollection>>();
        //        }
        //        return _userRequest;
        //    }
        //}
        public static bool IsUserIntendedController(Type controllerType)
        {
            bool isUserIntendedController = false;
            if (controllerType != null)
            {
                if (typeof(IControllerWithClientResources).IsAssignableFrom(controllerType))
                {
                    isUserIntendedController = true;
                }
            }
            return isUserIntendedController;
        }

        public static Type GetControllerTypeForCurrentRequest()
        {
            if (MvcApplication.UserRequest.Context.Items[UserRequestModel_Keys.UserContextControllerTypeKey] == null)
            {
                RouteData rd = RouteTable.Routes.GetRouteData(HttpContext.Current.Request.RequestContext.HttpContext);
                if (rd != null)
                {
                    string controllerName = RouteTable.Routes.GetRouteData(HttpContext.Current.Request.RequestContext.HttpContext).Values["controller"].ToString();
                    Type controllerType = ((CustomControllerFactory)ControllerBuilder.Current.GetControllerFactory()).GetControllerTypeByRequest(HttpContext.Current.Request.RequestContext, controllerName);
                    MvcApplication.UserRequest.Context.Items[UserRequestModel_Keys.UserContextControllerTypeKey] = controllerType;
                }
                else
                {
                    //return null;
                }
            }
            return (Type)MvcApplication.UserRequest.Context.Items[UserRequestModel_Keys.UserContextControllerTypeKey];
        }

        public static HttpVerbs RequestType(this ControllerBase controller)
        {
            return ControllerHelper.RequestType(controller.ControllerContext);
        }

        public static HttpVerbs RequestType(ControllerContext controllerContext)
        {
            string requestVerb = controllerContext.RequestContext.HttpContext.Request.RequestType.ToUpper();
            HttpVerbs result;
            switch (requestVerb)
            {
                case "DELETE":
                    result = HttpVerbs.Delete;
                    break;
                case "GET":
                    result = HttpVerbs.Get;
                    break;
                case "POST":
                    result = HttpVerbs.Post;
                    break;
                case "PUT":
                    result = HttpVerbs.Put;
                    break;
                case "HEAD":
                    result = HttpVerbs.Head;
                    break;
                default:
                    result = HttpVerbs.Get;
                    break;
            }
            return result;
        }
    }
}
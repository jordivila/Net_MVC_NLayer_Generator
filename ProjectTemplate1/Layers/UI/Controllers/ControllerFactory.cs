using System;
using System.Web.Mvc;
using System.Web.Routing;


namespace $safeprojectname$.Controllers
{
    public class CustomControllerFactory : DefaultControllerFactory
    {
        public Type GetControllerTypeByRequest(RequestContext requestContext, string controllerName)
        {
            return base.GetControllerType(requestContext, controllerName);
        }
    }
}

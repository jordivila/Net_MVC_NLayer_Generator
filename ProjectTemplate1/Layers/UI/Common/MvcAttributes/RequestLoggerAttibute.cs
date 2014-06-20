using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Logging;
using $customNamespace$.UI.Web.Controllers;

namespace $customNamespace$.UI.Web.Common.Mvc.Attributes
{
    public class RequestLoggerActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {
                if (ControllerHelper.IsUserIntendedController(filterContext.Controller.GetType()))
                {
                    //TODO: add MSMQ logging so these lines do not affect performance
                    string LogTitle = string.Format("{0} {1}", MvcApplication.Name, LoggerCategories.UIBeginRequest);
                    NameValueCollection serverVars = System.Web.HttpContext.Current.Request.Params;
                    Dictionary<string, object> param = (from key in serverVars.AllKeys select new KeyValuePair<string, object>(key, serverVars[key])).ToDictionary(k => k.Key, k => k.Value);
                    LoggingHelper.Write(new LogEntry(LogTitle, LoggerCategories.UIBeginRequest, 1, 1, TraceEventType.Information, LogTitle, param));
                }
            }
            catch (Exception)
            {
                //WARNING !!!! DO NOT RE THROUGH Exception as it can be set as RegisterGlobalFilters
            }
            base.OnActionExecuting(filterContext);
        }
    }
}

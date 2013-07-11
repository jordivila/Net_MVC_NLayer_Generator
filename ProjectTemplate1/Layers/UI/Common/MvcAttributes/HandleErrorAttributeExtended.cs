using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Logging;
using $safeprojectname$.Areas.Error;

namespace $safeprojectname$.Common.Mvc.Attributes
{
    public class HandleErrorAttributeExtended : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);


            string LogTitle = string.Format("{0} {1}", MvcApplication.Name, LoggerCategories.UIServerSideUnhandledException);
            NameValueCollection serverVars = System.Web.HttpContext.Current.Request.ServerVariables;
            Dictionary<string, object> param = (from key in serverVars.AllKeys select new KeyValuePair<string, object>(key, serverVars[key])).ToDictionary(k => k.Key, k => k.Value);
            LoggingHelper.Write(new LogEntry(filterContext.Exception, LoggerCategories.UIServerSideUnhandledException, 1, 1, TraceEventType.Error, LogTitle, param));

            if (filterContext.RequestContext.HttpContext.Request.IsAjaxRequest())
            {
                JsonResult jr = new JsonResult()
                {
                    Data = new DataResultString()
                                    {
                                        Message = $customNamespace$.Resources.General.GeneralTexts.UnexpectedError,
                                        IsValid = false,
                                        Data = $customNamespace$.Resources.General.GeneralTexts.UnexpectedError,
                                        MessageType = DataResultMessageType.Error
                                    }
                };

                filterContext.Result = jr;
                filterContext.ExceptionHandled = true;
            }
            else
            {
                UrlHelper url = new UrlHelper(filterContext.RequestContext);
                RedirectResult r = null;
                Type exceptionType = filterContext.Exception.GetType();
                if (exceptionType == typeof(FaultException))
                {
                    r = new RedirectResult(string.Format("{0}?id={1}", ErrorUrlHelper.FaultExceptionUnExpected(url), filterContext.Exception.Message));
                }
                else
                {
                    if (exceptionType.Namespace == typeof(Endpoint).Namespace)
                    {
                        r = new RedirectResult(ErrorUrlHelper.CommunicationError(url));
                    }
                    else
                    {
                        r = new RedirectResult(ErrorUrlHelper.UnExpected(url));
                    }
                }
                
                filterContext.Result = r;
                filterContext.ExceptionHandled = true;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.Unity;
using $customNamespace$.Models.Authentication;
using $customNamespace$.Models.Enumerations;
using $customNamespace$.Models.Logging;
using $customNamespace$.Models.Unity;
using $safeprojectname$.Areas.Error.Models;
using $safeprojectname$.Areas.UserAccount;
using $customNamespace$.Resources.General;


namespace $safeprojectname$.Areas.Error.Controllers
{
    public class ErrorController : AsyncController   //--> Must NOT implement IControllerWithClientResources. Otherwise Take a look at ~/Views/Shared/_LogOnPartial.cshtml
    {
        public ActionResult UnAuthorized()
        {
            ErrorInfoModel model = new ErrorInfoModel();
            model.BaseViewModelInfo.Title = GeneralTexts.UnAuthorizedError;
            return View(model);
        }

        public ActionResult Index()
        {
            ErrorInfoModel model = new ErrorInfoModel();
            model.BaseViewModelInfo.Title = GeneralTexts.UnexpectedError;
            return View(model);
        }

        public ActionResult FaultExceptionUnExpected()
        {
            ErrorInfoModel model = new ErrorInfoModel();
            model.BaseViewModelInfo.Title = GeneralTexts.UnexpectedError;
            return View(model);
        }

        public ActionResult SessionExpired()
        {
            using (IAuthenticationProxy authServices = DependencyFactory.Resolve<IAuthenticationProxy>())
            {
                authServices.LogOut();
            }

            Session.Abandon();

            SessionExpiredModel m = new SessionExpiredModel();
            m.BaseViewModelInfo.Title = GeneralTexts.SessionExpired;
            m.LoginUrl = UserAccountUrlHelper.Account_LogOn(Url);
            return View(m);
        }

        public ActionResult NotFound404()
        {
            NotFound404Model model = new NotFound404Model();
            model.BaseViewModelInfo.Title = GeneralTexts.UnexpectedError;
            return View(model);
        }

        public ActionResult Communication()
        {
            ErrorInfoModel model = new ErrorInfoModel();
            model.BaseViewModelInfo.Title = GeneralTexts.UnexpectedError;
            return View(model);
        }

        #region Log Client Side Error
        public void LogClientSideErrorAsync(
                string clientSideError_Description,
                string clientSideError_Url,                     //--> where did the error happend Ex.: someFileName.js
                string clientSideError_Line,                 //--> line where the error happend 
                string clientSideError_ParentUrl,       //--> url view route where did the error happend Ex.: http://www.domain.com/someUrl
                string clientSideError_UserAgent)
        {
            AsyncManager.OutstandingOperations.Increment();

            string LogTitle = string.Format("{0} {1}", MvcApplication.Name, LoggerCategories.UIClientSideJavascriptError);
            Dictionary<string, object> paramVariables = (from key in System.Web.HttpContext.Current.Request.ServerVariables.AllKeys
                                                         where System.Web.HttpContext.Current.Request.ServerVariables[key] != string.Empty
                                                         select new KeyValuePair<string, object>(key, System.Web.HttpContext.Current.Request.ServerVariables[key])).ToDictionary(k => k.Key, k => k.Value);
            Dictionary<string, object> paramLogger = new Dictionary<string, object>();

            Task.Factory.StartNew(() =>
            {
                try
                {
                    paramLogger.Add("clientSideError_Description", clientSideError_Description);
                    paramLogger.Add("clientSideError_Url", clientSideError_Url);
                    paramLogger.Add("clientSideError_Line", clientSideError_Line);
                    paramLogger.Add("clientSideError_ParentUrl", clientSideError_ParentUrl);
                    paramLogger.Add("clientSideError_UserAgent", clientSideError_UserAgent);
                    Dictionary<string, object> parameters = paramLogger.Concat(paramVariables).ToDictionary(x => x.Key, x => x.Value);
                    LogEntry lEntry = new LogEntry(LogTitle, LoggerCategories.UIClientSideJavascriptError, 1, 1, TraceEventType.Error, LogTitle, parameters);
                    LoggingHelper.Write(lEntry);
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                AsyncManager.OutstandingOperations.Decrement();
            });
        }

        public ActionResult LogClientSideErrorCompleted()
        {
            return base.File(Server.MapPath("~/Areas/Error/Content/ErrorLogged.jpg"), "image/jpeg");
        }
        #endregion
    }
}

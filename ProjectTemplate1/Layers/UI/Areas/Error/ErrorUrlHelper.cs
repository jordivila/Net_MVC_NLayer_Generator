using System.Web.Mvc;

namespace $customNamespace$.UI.Web.Areas.Error
{
    public static class ErrorUrlHelper
    {
        public static string SessionExpired(this UrlHelper helper)
        {
            return helper.Action("SessionExpired", "Error", new { Area = ErrorAreaRegistration.ErrorAreaName });
        }
        public static string UnAuthorized(this UrlHelper helper)
        {
            return helper.Action("UnAuthorized", "Error", new { Area = ErrorAreaRegistration.ErrorAreaName });
        }
        public static string UnExpected(this UrlHelper helper)
        {
            return helper.Action("Index", "Error", new { Area = ErrorAreaRegistration.ErrorAreaName });
        }
        public static string FaultExceptionUnExpected(this UrlHelper helper)
        {
            return helper.Action("FaultExceptionUnExpected", "Error", new { Area = ErrorAreaRegistration.ErrorAreaName });
        }
        public static string CommunicationError(this UrlHelper helper)
        {
            return helper.Action("Communication", "Error", new { Area = ErrorAreaRegistration.ErrorAreaName });
        }
        public static string LogClientSideError(this UrlHelper helper)
        {
            return helper.Action("LogClientSideError", "Error", new { Area = ErrorAreaRegistration.ErrorAreaName });
        }
        public static string NotFound404(this UrlHelper helper)
        {
            return helper.Action("NotFound404", "Error", new { Area = ErrorAreaRegistration.ErrorAreaName });
        }
    }
}
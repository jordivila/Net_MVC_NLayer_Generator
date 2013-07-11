using System.Web.Mvc;
using $safeprojectname$.Models;

namespace $safeprojectname$.Areas.Error.Models
{
    public class SessionExpiredModel: baseViewModel
    {
        public string LoginUrl { get; set; }
    }

    public class ErrorInfoModel : baseViewModel
    {
        public HandleErrorInfo ErrorInfo { get; set; }
    }

    public class NotFound404Model : baseViewModel
    { 

    }
}
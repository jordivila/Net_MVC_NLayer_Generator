using System.Web.Mvc;
using $customNamespace$.Models.Enumerations;

namespace $safeprojectname$.TestCases.AuthorizeAttribute
{
    [$customNamespace$.UI.Web.Common.Mvc.Attributes.Authorize(Roles = SiteRoles.Administrator)]
    public class AdminOnlyControllerController : Controller
    {
        public static JsonResult IndexResult = new JsonResult() { Data = new { ResultMethod = "Controller method executed" } };

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult Index()
        {
            return IndexResult;
        }
    }

    public class AdminOnlyControllerAreaRegistration : AreaRegistration
    {
        public static string AdminOnlyControllerAttributeAreaName = "AdminOnlyControllerAreaName";

        public override string AreaName
        {
            get
            {
                return AdminOnlyControllerAreaRegistration.AdminOnlyControllerAttributeAreaName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                string.Format("{0}_default", AdminOnlyControllerAreaRegistration.AdminOnlyControllerAttributeAreaName),
                string.Format("{0}/{{action}}/{{id}}", AdminOnlyControllerAreaRegistration.AdminOnlyControllerAttributeAreaName),
                new { controller = AdminOnlyControllerAreaRegistration.AdminOnlyControllerAttributeAreaName, action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}

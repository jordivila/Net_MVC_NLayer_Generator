using System.Web.Mvc;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Globalization;

namespace $customNamespace$.UI.Web.Controllers
{
    public class CultureController : Controller
    {
        [HttpPost]
        public JsonResult Set(string culture)
        {
            //CulturesAvailable cultureSelected = (CulturesAvailable)Enum.Parse(typeof(CulturesAvailable), culture.Replace("-", "_"));

            MvcApplication.UserRequest.UserProfile.Culture = GlobalizationHelper.CultureInfoGetOrDefault(culture);
            MvcApplication.UserRequest.UserProfile.ApplyClientProperties();

            DataResultBoolean result = new DataResultBoolean();
            result.IsValid = true;
            result.Data = true;
            return Json(result);
        }
    }
}

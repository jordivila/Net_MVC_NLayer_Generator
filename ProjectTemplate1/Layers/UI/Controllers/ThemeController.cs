using System;
using System.Web.Mvc;
using $customNamespace$.Models.Common;
using $customNamespace$.Models.Enumerations;

namespace $safeprojectname$.Controllers
{
    public class ThemeController : Controller
    {
        [HttpPost]
        public JsonResult Set(string theme)
        {

            try
            {
                ThemesAvailable themeSelected = (ThemesAvailable)Enum.Parse(typeof(ThemesAvailable), theme);

                MvcApplication.UserRequest.UserProfile.Theme = themeSelected;
                MvcApplication.UserRequest.UserProfile.ApplyClientProperties();


                //if (!MvcApplication.UserRequest.ContextBag.AllKeys.Contains(UserRequestModel_Keys.WcfClientThemeSelectedCookieName))
                //{
                    //MvcApplication.UserRequest.Context.Response.Cookies.Add(new HttpCookie(UserRequestModel_Keys.WcfClientThemeSelectedCookieName, theme.ToString()));
                //}
                //else
                //{
                    //MvcApplication.UserRequest.Context.Response.Cookies[UserRequestModel_Keys.WcfClientThemeSelectedCookieName].Value = theme.ToString();
                //}

                DataResultString result = new DataResultString();
                result.IsValid = true;
                result.Data = themeSelected.ToUri();
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
    }
}

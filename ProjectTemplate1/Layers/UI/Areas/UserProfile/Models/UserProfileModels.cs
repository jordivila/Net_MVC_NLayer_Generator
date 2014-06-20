using $customNamespace$.Models.Profile;
using $customNamespace$.UI.Web.Models;
using $customNamespace$.UI.Web.Common.Mvc.Attributes;

namespace $customNamespace$.UI.Web.Areas.UserProfile.Models
{
    [NonValidateModelOnHttpGet]
    public class UserProfileIndexModel : baseViewModel
    {
        public DataResultUserProfile UserProfileResult { get; set; }
        public DataResultUserProfile UserProfileResultUpdated { get; set; }
    }
}
using $customNamespace$.Models.Profile;
using $safeprojectname$.Models;
using $safeprojectname$.Common.Mvc.Attributes;

namespace $safeprojectname$.Areas.UserProfile.Models
{
    [NonValidateModelOnHttpGet]
    public class UserProfileIndexModel : baseViewModel
    {
        public DataResultUserProfile UserProfileResult { get; set; }
        public DataResultUserProfile UserProfileResultUpdated { get; set; }
    }
}
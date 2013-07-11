using System.Collections.Generic;
using $customNamespace$.Models.Membership;
using $safeprojectname$.Models;
using $safeprojectname$.Common.Mvc.Attributes;


namespace $safeprojectname$.Areas.UserAdministration.Models
{
    public enum Actions : int
    {
        Approve,
        UnApprove,
        Delete,
        Update,
        Search,
        //Paginate,
        //PageSizeChange,
        //Sort,
        ViewDetail,
        UnLock
    }

    [NonValidateModelOnHttpGet]
    public class IndexViewModel : baseViewModel
    {
        public Actions Action { get; set; }
        public DataFilterUserList Filter { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public DataResultUserList UserListResult { get; set; }
    }
}
using System.Collections.Generic;
using $customNamespace$.Models.Membership;
using $customNamespace$.UI.Web.Models;
using $customNamespace$.UI.Web.Common.Mvc.Attributes;


namespace $customNamespace$.UI.Web.Areas.UserAdministration.Models
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
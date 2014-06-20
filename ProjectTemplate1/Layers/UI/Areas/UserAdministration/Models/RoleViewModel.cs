using System.Collections.Generic;
using $customNamespace$.Models.Membership;
using $customNamespace$.UI.Web.Models;

namespace $customNamespace$.UI.Web.Areas.UserAdministration.Models
{
    public class RoleViewModel : baseViewModel
	{
		public string Role { get; set; }
		public IEnumerable<MembershipUserWrapper> Users { get; set; }
	}
}
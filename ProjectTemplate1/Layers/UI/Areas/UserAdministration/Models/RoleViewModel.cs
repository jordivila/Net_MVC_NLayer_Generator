using System.Collections.Generic;
using $customNamespace$.Models.Membership;
using $safeprojectname$.Models;

namespace $safeprojectname$.Areas.UserAdministration.Models
{
    public class RoleViewModel : baseViewModel
	{
		public string Role { get; set; }
		public IEnumerable<MembershipUserWrapper> Users { get; set; }
	}
}
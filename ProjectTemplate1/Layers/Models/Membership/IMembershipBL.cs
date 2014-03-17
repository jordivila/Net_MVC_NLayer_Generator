using System;

namespace $customNamespace$.Models.Membership
{
    public interface IMembershipBL : IMembershipProxy
    {
        bool ValidatePasswordStrength(string password);
    }
}

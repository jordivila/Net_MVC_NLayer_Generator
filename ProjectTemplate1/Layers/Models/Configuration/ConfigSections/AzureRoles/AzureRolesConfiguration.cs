using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Configuration;
using System.ServiceModel;

namespace $customNamespace$.Models.Configuration.ConfigSections.AzureRoles
{
    public interface IAzureRolesConfiguration
    {
        string WCF_RoleName { get; set; }
        string WCF_InternalEndPointName { get; set; }

        RoleInstanceEndpoint RoleInstanceEndpointGet(string roleName, string endpointName);
        EndpointAddress ReplaceEndpointAddressAuthorityByRoleEndpoint(EndpointAddress endpoint, string roleName, string roleEndpointName);
    }

    public class AzureRolesConfiguration : IAzureRolesConfiguration
    {
        public AzureRolesConfiguration()
        {
            this.WCF_RoleName = "$customNamespace$.WCF.ServicesHostWorkerRole";
            this.WCF_InternalEndPointName = "Internal";
        }

        public string WCF_RoleName { get; set; }
        public string WCF_InternalEndPointName { get; set; }


        public RoleInstanceEndpoint RoleInstanceEndpointGet(string roleName, string endpointName)
        {
            return RoleEnvironment.Roles[roleName].Instances[0].InstanceEndpoints[endpointName];
        }


        public EndpointAddress ReplaceEndpointAddressAuthorityByRoleEndpoint(EndpointAddress endpointToReplace, string roleName, string roleEndpointName)
        {
            RoleInstanceEndpoint roleInstanceEnpoint = this.RoleInstanceEndpointGet(roleName, roleEndpointName);

            return
                new EndpointAddress(
                    new Uri(endpointToReplace.Uri.ToString().Replace(endpointToReplace.Uri.Authority,
                            string.Format("{0}:{1}",
                                            roleInstanceEnpoint.IPEndpoint.Address.ToString(),
                                            roleInstanceEnpoint.IPEndpoint.Port))));
        }
    }
}

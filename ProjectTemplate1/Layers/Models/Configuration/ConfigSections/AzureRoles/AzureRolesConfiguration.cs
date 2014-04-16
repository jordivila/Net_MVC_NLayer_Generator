using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Configuration;
using System.ServiceModel;
using System.Net;

namespace $customNamespace$.Models.Configuration.ConfigSections.AzureRoles
{
    public interface IAzureRolesConfiguration
    {
        string WCF_RoleName { get; set; }
        string WCF_InternalEndPointName { get; set; }
        int WCF_InstanceNumber { get; set; }

        IPEndPoint BackEndGetIpEndpoint();
    }

    public class AzureRolesConfiguration : IAzureRolesConfiguration
    {
        public AzureRolesConfiguration()
        {
            this.WCF_RoleName = "$customNamespace$.WCF.ServicesHostWorkerRole";
            this.WCF_InternalEndPointName = "Internal";
            this.WCF_InstanceNumber = 0;
        }

        public string WCF_RoleName { get; set; }
        public string WCF_InternalEndPointName { get; set; }
        public int WCF_InstanceNumber { get; set; }


        private RoleInstanceEndpoint BackEndRoleInstanceEndpointGet()
        {
            return RoleEnvironment.Roles[this.WCF_RoleName].Instances[this.WCF_InstanceNumber].InstanceEndpoints[this.WCF_InternalEndPointName];
        }

        public IPEndPoint BackEndGetIpEndpoint()
        {
            return this.BackEndRoleInstanceEndpointGet().IPEndpoint;
        }
    }
}
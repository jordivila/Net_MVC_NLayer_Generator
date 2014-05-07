using Microsoft.WindowsAzure.ServiceRuntime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace $customNamespace$.Models.Configuration.ConfigSections.AzureRoles
{
    public interface IAzureRolesConfiguration
    {
        string WCF_RoleName { get; set; }
        string WCF_InternalEndPointName { get; set; }
        int WCF_InstanceNumber { get; set; }

        IPEndPoint BackEndGetIpEndpoint();
        string DatabaseCnnStringGetByName(string databaseCnnName);
    }

    public class AzureRolesConfiguration : IAzureRolesConfiguration
    {
        public AzureRolesConfiguration()
        {
            this.WCF_RoleName = "$customNamespace$.WCF.ServicesHostWorkerRole";
            this.WCF_InternalEndPointName = "Internal";
            this.WCF_InstanceNumber = RoleEnvironment.CurrentRoleInstance.UpdateDomain;
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

        public string DatabaseCnnStringGetByName(string databaseCnnName)
        {
            return RoleEnvironment.GetConfigurationSettingValue(databaseCnnName);
        }
    }
}

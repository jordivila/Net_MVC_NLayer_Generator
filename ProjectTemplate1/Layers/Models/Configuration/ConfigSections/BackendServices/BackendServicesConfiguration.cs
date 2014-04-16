using $customNamespace$.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace $customNamespace$.Models.Configuration.ConfigSections.BackendServices
{
    public interface IBackendServicesConfiguration
    {
        Binding GetBinding();
        Uri GetEndpoint(HostingPlatform HostingPlatform, Binding binding, ContractDescription contractDescription);
        IPEndPoint GetIPEndPoint(HostingPlatform HostingPlatform);
    }

    public class BackendServicesConfiguration : IBackendServicesConfiguration
    {
        public Binding GetBinding()
        {
            NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);

            return binding;
        }

        private string GetBindingProtocol(Binding binding)
        {
            if (binding is NetTcpBinding)
            {
                return "net.tcp";
            }

            if (binding is BasicHttpBinding)
            {
                return "http";
            }

            throw new ArgumentException("Binding not supported. Add protocol yourself if you want to");
        }

        public Uri GetEndpoint(HostingPlatform HostingPlatform, Binding binding, ContractDescription contractDescription)
        {
            return new Uri(String.Format("{0}://{1}/{2}",
                                            this.GetBindingProtocol(binding),
                                            this.GetIPEndPoint(HostingPlatform),
                                            contractDescription.ContractType.FullName));
        }

        public IPEndPoint GetIPEndPoint(HostingPlatform HostingPlatform)
        {
            IPEndPoint backendEndpointAddress = null;

            switch (HostingPlatform)
            {
                case HostingPlatform.Azure:
                    backendEndpointAddress = ApplicationConfiguration.AzureRolesConfigurationSection.BackEndGetIpEndpoint();
                    break;
                default:    // Custom Host assumed
                    Func<string> LocalIpAddress = delegate()
                    {
                        IPHostEntry host;
                        string localIP = "";
                        host = Dns.GetHostEntry(Dns.GetHostName());
                        foreach (IPAddress ip in host.AddressList)
                        {
                            if (ip.AddressFamily == AddressFamily.InterNetwork)
                            {
                                localIP = ip.ToString();
                                break;
                            }
                        }
                        return localIP;
                    };

                    //backendEndpointAddress = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8523);
                    backendEndpointAddress = new IPEndPoint(IPAddress.Parse(LocalIpAddress()), 8523);
                    break;
            }

            return backendEndpointAddress;
        }
    }
}

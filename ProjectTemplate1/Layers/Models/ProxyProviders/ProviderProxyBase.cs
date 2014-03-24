using Microsoft.WindowsAzure.ServiceRuntime;
using $customNamespace$.Models.Unity;
using System;
using System.ServiceModel;

namespace $customNamespace$.Models.ProxyProviders
{
    public abstract class ProviderBaseChannel<TChannel> : IDisposable
        where TChannel : class
    {
        IClientChannelInitializer<TChannel> initiator = null;

        public ProviderBaseChannel()
        {
            initiator = DependencyFactory.Resolve<IClientChannelInitializer<TChannel>>();
        }

        protected TChannel proxy
        {
            get
            {
                return (TChannel)this.initiator.Proxy;
            }
        }

        public CommunicationState State
        {
            get
            {
                return this.initiator.Proxy.State;
            }
        }

        public virtual void Dispose()
        {
            this.initiator.Dispose();
        }
    }

    public interface IClientChannelInitializer<TChannel> : IDisposable
    {
        IClientChannel Proxy { get; }
    }

    public abstract class ClientChannelInitializer<TChannel> : IClientChannelInitializer<TChannel>
    {
        private static ChannelFactory<TChannel> channelFactoryInstance = null;
        private IClientChannel channelInstance = null;

        public ClientChannelInitializer()
        {
            if (ClientChannelInitializer<TChannel>.channelFactoryInstance == null)
            {
                ClientChannelInitializer<TChannel>.channelFactoryInstance = this.ChannelFactoryInit(new ChannelFactory<TChannel>(typeof(TChannel).Name));
            }
        }

        protected abstract ChannelFactory<TChannel> ChannelFactoryInit(ChannelFactory<TChannel> channelFactory);

        public IClientChannel Proxy 
        {
            get
            {
                if (this.channelInstance == null)
                {
                    this.channelInstance = this.ClientChannelCreate(ClientChannelInitializer<TChannel>.channelFactoryInstance);
                }
                return this.channelInstance;
            }
        }

        protected virtual IClientChannel ClientChannelCreate(ChannelFactory<TChannel> factory)
        {
            IClientChannel channel = (IClientChannel)factory.CreateChannel();
            channel.Open();
            return channel;
        }

        public void Dispose()
        {
            if (this.channelInstance != null)
            {
                this.channelInstance.Close();
                this.channelInstance.Dispose();
            }
        }
    }

    public class ClientChannelCustomHostInitializer<TChannel> : ClientChannelInitializer<TChannel>
    {
        public ClientChannelCustomHostInitializer() : base() { }

        protected override ChannelFactory<TChannel> ChannelFactoryInit(ChannelFactory<TChannel> channelFactory)
        {
            return channelFactory;
        }
    }

    public class ClientChannelAzureInternalRoleInitializer<TChannel> : ClientChannelInitializer<TChannel>
    {
        private string roleName = "$customNamespace$.WCF.ServicesHostWorkerRole";
        private string roleInternalEndpointName = "Internal";
        private RoleInstanceEndpoint roleInternalInstanceEndPoint = null;

        public ClientChannelAzureInternalRoleInitializer() : base() { }

        private RoleInstanceEndpoint RoleInternalInstanceEndpointInit()
        {
            if (this.roleInternalInstanceEndPoint == null)
            {
                this.roleInternalInstanceEndPoint = RoleEnvironment.Roles[roleName].Instances[0].InstanceEndpoints[roleInternalEndpointName];
            }

            return this.roleInternalInstanceEndPoint;
        }

        protected override ChannelFactory<TChannel> ChannelFactoryInit(ChannelFactory<TChannel> channelFactory)
        {
            this.RoleInternalInstanceEndpointInit();

            EndpointAddress channelFactoryEndpointAddress = channelFactory.Endpoint.Address;
            
            EndpointAddress endpointWorkerRoleAddress =
                new EndpointAddress(
                    new Uri(channelFactoryEndpointAddress.Uri.ToString().Replace(channelFactoryEndpointAddress.Uri.Authority,
                            string.Format("{0}:{1}",
                                            roleInternalInstanceEndPoint.IPEndpoint.Address.ToString(),
                                            roleInternalInstanceEndPoint.IPEndpoint.Port))));

            channelFactory.Endpoint.Address = endpointWorkerRoleAddress;

            return channelFactory;
        }
    }
}
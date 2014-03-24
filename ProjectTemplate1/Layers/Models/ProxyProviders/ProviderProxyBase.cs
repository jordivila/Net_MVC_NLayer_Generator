using Microsoft.WindowsAzure.ServiceRuntime;
using $customNamespace$.Models.Configuration;
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
        private string roleName = ApplicationConfiguration.AzureRolesConfigurationSection.WCF_RoleName;
        private string roleInternalEndpointName = ApplicationConfiguration.AzureRolesConfigurationSection.WCF_InternalEndPointName;
        private RoleInstanceEndpoint roleInternalInstanceEndPoint = null;

        public ClientChannelAzureInternalRoleInitializer() : base() { }

        private RoleInstanceEndpoint RoleInternalInstanceEndpointInit()
        {
            if (this.roleInternalInstanceEndPoint == null)
            {
                this.roleInternalInstanceEndPoint = ApplicationConfiguration.AzureRolesConfigurationSection.RoleInstanceEndpointGet(roleName, roleInternalEndpointName);
            }

            return this.roleInternalInstanceEndPoint;
        }

        protected override ChannelFactory<TChannel> ChannelFactoryInit(ChannelFactory<TChannel> channelFactory)
        {
            this.RoleInternalInstanceEndpointInit();

            channelFactory.Endpoint.Address = ApplicationConfiguration.AzureRolesConfigurationSection.ReplaceEndpointAddressAuthorityByRoleEndpoint(
                channelFactory.Endpoint.Address, roleName, roleInternalEndpointName);

            return channelFactory;
        }
    }
}
using Microsoft.WindowsAzure.ServiceRuntime;
using $customNamespace$.Models.Configuration;
using $customNamespace$.Models.Unity;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using $customNamespace$.Models.Enumerations;

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
                ClientChannelInitializer<TChannel>.channelFactoryInstance =
                    this.ChannelFactoryInit(
                        new ChannelFactory<TChannel>(typeof(TChannel).Name));
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
            Uri backendEndpointAddress = ApplicationConfiguration.BackendServicesConfiguration.GetEndpoint(
                                                    HostingPlatform.Custom,
                                                    channelFactory.Endpoint.Binding,
                                                    ContractDescription.GetContract(typeof(TChannel)));

            channelFactory.Endpoint.Address = new EndpointAddress(backendEndpointAddress);

            return channelFactory;
        }
    }

    public class ClientChannelAzureInternalRoleInitializer<TChannel> : ClientChannelInitializer<TChannel>
    {
        public ClientChannelAzureInternalRoleInitializer() : base() { }

        protected override ChannelFactory<TChannel> ChannelFactoryInit(ChannelFactory<TChannel> channelFactory)
        {
            Uri backendEndpointAddress = ApplicationConfiguration.BackendServicesConfiguration.GetEndpoint(
                                                    HostingPlatform.Azure,
                                                    channelFactory.Endpoint.Binding,
                                                    ContractDescription.GetContract(typeof(TChannel)));

            channelFactory.Endpoint.Address = new EndpointAddress(backendEndpointAddress);

            return channelFactory;
        }
    }
}
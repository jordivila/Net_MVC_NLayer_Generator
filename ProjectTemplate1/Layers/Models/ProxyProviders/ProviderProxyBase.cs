using Microsoft.WindowsAzure.ServiceRuntime;
using $customNamespace$.Models.Unity;
using System;
using System.ServiceModel;

namespace $customNamespace$.Models.ProxyProviders
{
    public abstract class ProviderBaseChannel<TChannel> : IDisposable
        where TChannel : class
    {
        IProviderBaseChannelInitiator<TChannel> initiator = null;

        public ProviderBaseChannel()
        {
            initiator = DependencyFactory.Resolve<IProviderBaseChannelInitiator<TChannel>>();
        }

        private IClientChannel ProxyGet()
        {
            IClientChannel result = this.initiator.ProxyInit();

            //if (((IClientChannel)result).State == CommunicationState.Faulted)
            //{
            //    result.Abort();
            //    result.Close();
            //    result.Dispose();

            //    //cache.Remove(cacheChannelClientKey);
            //    result = this.ProxyInit();
            //}

            return result;
        }

        protected TChannel proxy
        {
            get
            {
                return (TChannel)ProxyGet();
            }
        }

        public CommunicationState State
        {
            get
            {
                return ((IClientChannel)this.proxy).State;
            }
        }

        public virtual void Dispose()
        {
            this.initiator.Dispose();
        }
    }

    public interface IProviderBaseChannelInitiator<TChannel> : IDisposable
    {
        IClientChannel ProxyInit();
    }

    public abstract class ProviderChannelInitiatorBase<TChannel> : IProviderBaseChannelInitiator<TChannel>
    {
        protected ChannelFactory<TChannel> channelFactoryInstance = null;
        protected IClientChannel channelInstance = null;

        public ProviderChannelInitiatorBase()
        {
            this.channelFactoryInstance = new ChannelFactory<TChannel>(typeof(TChannel).Name);
        }

        public abstract IClientChannel ProxyInit();

        public void Dispose()
        {
            if (this.channelInstance != null)
            {
                this.channelInstance.Close();
                this.channelInstance.Dispose();
            }

            if (this.channelFactoryInstance != null)
            {
                this.channelFactoryInstance.Close();
            }
        }
    }

    public class ProviderChannelInitiatorAzureRole<TChannel> : ProviderChannelInitiatorBase<TChannel>
    {
        public ProviderChannelInitiatorAzureRole() : base() { }

        public override IClientChannel ProxyInit()
        {
            if (this.channelInstance == null)
            {
                RoleInstanceEndpoint internalEndPoint = RoleEnvironment.Roles["$customNamespace$.WCF.ServicesHostWorkerRole"].Instances[0].InstanceEndpoints["Internal"];
                EndpointAddress channelFactoryEndpointAddress = this.channelFactoryInstance.Endpoint.Address;
                EndpointAddress endpointWorkerRoleAddress = new EndpointAddress(new Uri(channelFactoryEndpointAddress.Uri.ToString().Replace(channelFactoryEndpointAddress.Uri.Authority, string.Format("{0}:{1}", internalEndPoint.IPEndpoint.Address.ToString(), internalEndPoint.IPEndpoint.Port))));
                this.channelInstance = (IClientChannel)this.channelFactoryInstance.CreateChannel(endpointWorkerRoleAddress);
                this.channelInstance.Open();
            }
            return this.channelInstance;
        }
    }

    public class ProviderChannelInitiatorCustomHost<TChannel> : ProviderChannelInitiatorBase<TChannel>
    {
        public ProviderChannelInitiatorCustomHost() : base() { }

        public override IClientChannel ProxyInit()
        {
            if (this.channelInstance == null)
            {
                this.channelInstance = (IClientChannel)this.channelFactoryInstance.CreateChannel();
                this.channelInstance.Open();
            }
            return this.channelInstance;
        }
    }

}
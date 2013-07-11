using System;
using System.ServiceModel;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace $safeprojectname$.ProxyProviders
{
    public abstract class ProviderBaseChannel<TChannel> : IDisposable
        where TChannel : class
    {
        static readonly object locker = new object();

        private ICacheManager cache = CacheFactory.GetCacheManager("CacheManagerForProxyProviders");
        private string cacheType;
        private string cacheChannelFactoryKey;
        private string cacheChannelClientKey;

        public ProviderBaseChannel()
        {
            this.cacheType = typeof(TChannel).Name;
            this.cacheChannelFactoryKey = string.Format("channel_{0}", cacheType);
            this.cacheChannelClientKey = string.Format("channelClient_{0}", cacheType);
        }

        private ChannelFactory<TChannel> ChannelFactoryGet()
        {
            ChannelFactory<TChannel> result;

            lock (locker)
            {
                if (cache[cacheChannelFactoryKey] == null)
                {
                    cache.Add(cacheChannelFactoryKey, new ChannelFactory<TChannel>(typeof(TChannel).Name));
                }
                result = (ChannelFactory<TChannel>)cache[cacheChannelFactoryKey];
            }

            return result;
        }

        private IClientChannel ProxyInit()
        {
            IClientChannel result;

            lock (locker)
            {
                if (cache[cacheChannelClientKey] == null)
                {
                    cache.Add(cacheChannelClientKey, ChannelFactoryGet().CreateChannel());
                    ((IClientChannel)cache[cacheChannelClientKey]).Open();
                }
                result = ((IClientChannel)cache[cacheChannelClientKey]);
            }
            return result;
        }

        private IClientChannel ProxyGet()
        {
            IClientChannel result;

            lock (locker)
            {
                result = this.ProxyInit();
                if (((IClientChannel)result).State == CommunicationState.Faulted)
                {
                    result.Abort();
                    result.Close();
                    result.Dispose();

                    cache.Remove(cacheChannelClientKey);
                    result = this.ProxyInit();
                }
            }

            return result;
        }

        internal TChannel proxy
        {
            get
            {
                return (TChannel)ProxyGet();
            }
        }

        public virtual void Dispose()
        {
            // Do NOT dispose TChannel, otherwise runtime needs to recreate and reopen it
        }

        /*
        static readonly object locker = new object();
        static ChannelFactory<TChannel> _channelFactory = null;
        static TChannel _clientChannel = null;

        private ChannelFactory<TChannel> ChannelFactoryGet()
        {
            if (_channelFactory == null)
            {
                _channelFactory = new ChannelFactory<TChannel>(typeof(TChannel).Name);
            }
            return _channelFactory;
        }

        private IClientChannel ProxyInit()
        {
            _clientChannel = ChannelFactoryGet().CreateChannel();
            ((IClientChannel)_clientChannel).Open();
            return ((IClientChannel)_clientChannel);
        }

        private IClientChannel ProxyGet()
        {
            if (_clientChannel == null)
            {
                ProxyInit();
            }

            if (((IClientChannel)_clientChannel).State == CommunicationState.Faulted)
            {
                ((IClientChannel)_clientChannel).Abort();
                ((IClientChannel)_clientChannel).Close();
                ProxyInit();
            }

            return ((IClientChannel)_clientChannel);
        }

        internal TChannel proxy
        {
            get
            {
                lock (locker)
                {
                    return (TChannel)ProxyGet(); ;
                }
            }
        }

        public virtual void Dispose()
        {
            // Do NOT dispose TChannel, otherwise runtime needs to recreate and reopen it
        }
         
         */

    }
}
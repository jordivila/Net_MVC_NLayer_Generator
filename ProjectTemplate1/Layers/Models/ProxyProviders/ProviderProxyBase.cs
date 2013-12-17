using System;
using System.ServiceModel;

namespace $safeprojectname$.ProxyProviders
{
    public abstract class ProviderBaseChannel<TChannel> : IDisposable
        where TChannel : class
    {
        static readonly object locker = new object();

        //private ICacheManager cache = CacheFactory.GetCacheManager("CacheManagerForProxyProviders");
        //private string cacheType;
        //private string cacheChannelFactoryKey;
        //private string cacheChannelClientKey;
        private ChannelFactory<TChannel> channelFactoryInstance = null;

        public ProviderBaseChannel()
        {
            //this.cacheType = typeof(TChannel).Name;
            //this.cacheChannelFactoryKey = string.Format("channel_{0}", cacheType);
            //this.cacheChannelClientKey = string.Format("channelClient_{0}", cacheType);
        }

        private ChannelFactory<TChannel> ChannelFactoryGet()
        {
            if (this.channelFactoryInstance == null)
            {
                this.channelFactoryInstance = new ChannelFactory<TChannel>(typeof(TChannel).Name);
            }

            return this.channelFactoryInstance;

            //ChannelFactory<TChannel> result;

            //lock (locker)
            //{
            //    if (cache[cacheChannelFactoryKey] == null)
            //    {
            //        cache.Add(cacheChannelFactoryKey, new ChannelFactory<TChannel>(typeof(TChannel).Name));
            //    }
            //    result = (ChannelFactory<TChannel>)cache[cacheChannelFactoryKey];
            //}

            //return result;
        }

        private IClientChannel ProxyInit()
        {
            IClientChannel channel = (IClientChannel)ChannelFactoryGet().CreateChannel();
            channel.Open();
            return channel;

            //IClientChannel result;

            //lock (locker)
            //{
            //    if (cache[cacheChannelClientKey] == null)
            //    {
            //        cache.Add(cacheChannelClientKey, ChannelFactoryGet().CreateChannel());
            //        ((IClientChannel)cache[cacheChannelClientKey]).Open();
            //    }
            //    result = ((IClientChannel)cache[cacheChannelClientKey]);
            //}
            //return result;
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

                    //cache.Remove(cacheChannelClientKey);
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
    }
}
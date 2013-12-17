using System.Runtime.Caching;
using System.ServiceModel.Syndication;
using $safeprojectname$.ProxyProviders;

namespace $safeprojectname$.Syndication
{
    public class ProviderProxySyndication : ProviderBaseChannel<IProviderSyndication>, IProviderSyndication
    {
        private const string CacheManagerName = "CacheManagerForBlogFrontEnd";
        private ObjectCache Cache = new MemoryCache(CacheManagerName);
        private CacheItemPolicy CachePolicy = new CacheItemPolicy();

        public SyndicationFeedFormatter GetInfoWithNoItems()
        {
            string cacheKey = "GetInfoWithNoItems";

            if (Cache.Get(cacheKey) == null)
            {
                Cache.Add(cacheKey, this.proxy.GetInfoWithNoItems(), CachePolicy);
            }
            return (SyndicationFeedFormatter)Cache.Get(cacheKey);
        }
        public DataResultSyndicationItems GetByCategory(DataFilterSyndication filter)
        {
            string cacheKey = string.Format("{0}_{1}_{2}_{3}", "GetByCategory", filter.CategoryName, filter.Page, filter.PageSize);

            if (Cache.Get(cacheKey) == null)
            {
                Cache.Add(cacheKey, this.proxy.GetByCategory(filter), CachePolicy);
            }
            return (DataResultSyndicationItems)Cache.Get(cacheKey);
        }
        public DataResultSyndicationItems GetByTitle(DataFilterSyndication filter)
        {
            string cacheKey = string.Format("{0}_{1}_{2}_{3}", "GetByTitle", filter.Uri, filter.Page, filter.PageSize);

            if (Cache.Get(cacheKey) == null)
            {
                Cache.Add(cacheKey, this.proxy.GetByTitle(filter), CachePolicy);
            }
            return (DataResultSyndicationItems)Cache.Get(cacheKey);
        }
        public DataResultSyndicationItems GetLast(DataFilterSyndication filter)
        {
            string cacheKey = string.Format("{0}_{1}_{2}_{3}", "GetLast", filter.PageSize, filter.Page, filter.PageSize);

            if (Cache.Get(cacheKey) == null)
            {
                Cache.Add(cacheKey, this.proxy.GetLast(filter), CachePolicy);
            }
            return (DataResultSyndicationItems)Cache.Get(cacheKey);
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
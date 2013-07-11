using System.ServiceModel.Syndication;
using Microsoft.Practices.EnterpriseLibrary.Caching;
using $safeprojectname$.ProxyProviders;

namespace $safeprojectname$.Syndication
{
    public class ProviderProxySyndication : ProviderBaseChannel<IProviderSyndication>, IProviderSyndication
    {
        private static string CacheManagerName = "CacheManagerForBlogFrontEnd";
        private static ICacheManager Cache = CacheFactory.GetCacheManager(CacheManagerName);

        public SyndicationFeedFormatter GetInfoWithNoItems()
        {
            string cacheKey = "GetInfoWithNoItems";

            if (Cache.GetData(cacheKey) == null)
            {
                Cache.Add(cacheKey, this.proxy.GetInfoWithNoItems());
            }
            return (SyndicationFeedFormatter)Cache.GetData(cacheKey);
        }
        public DataResultSyndicationItems GetByCategory(DataFilterSyndication filter)
        {
            string cacheKey = string.Format("{0}_{1}_{2}_{3}", "GetByCategory", filter.CategoryName, filter.Page, filter.PageSize);

            if (Cache.GetData(cacheKey) == null)
            {
                Cache.Add(cacheKey, this.proxy.GetByCategory(filter));
            }
            return (DataResultSyndicationItems)Cache.GetData(cacheKey);
        }
        public DataResultSyndicationItems GetByTitle(DataFilterSyndication filter)
        {
            string cacheKey = string.Format("{0}_{1}_{2}_{3}", "GetByTitle", filter.Uri, filter.Page, filter.PageSize);

            if (Cache.GetData(cacheKey) == null)
            {
                Cache.Add(cacheKey, this.proxy.GetByTitle(filter));
            }
            return (DataResultSyndicationItems)Cache.GetData(cacheKey);
        }
        public DataResultSyndicationItems GetLast(DataFilterSyndication filter)
        {
            string cacheKey = string.Format("{0}_{1}_{2}_{3}", "GetLast", filter.PageSize, filter.Page, filter.PageSize);

            if (Cache.GetData(cacheKey) == null)
            {
                Cache.Add(cacheKey, this.proxy.GetLast(filter));
            }
            return (DataResultSyndicationItems)Cache.GetData(cacheKey);
        }
        public override void Dispose()
        {
            base.Dispose();
        }
    }
}
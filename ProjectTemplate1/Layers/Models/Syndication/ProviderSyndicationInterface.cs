using System;
using System.ServiceModel;
using System.ServiceModel.Syndication;

namespace $safeprojectname$.Syndication
{
    [ServiceContract]
    [ServiceKnownType(typeof(Atom10ItemFormatter))]
    [ServiceKnownType(typeof(Atom10FeedFormatter))]
    public interface IProviderSyndication : IDisposable
    {

        [OperationContract]
        /// <summary>
        /// Get Blog Info With out SindycationItemsContent. Blog Info = SyndicationFeed with categories, titles etc BUT no item content
        /// </summary>
        /// <returns></returns>
        SyndicationFeedFormatter GetInfoWithNoItems();

        [OperationContract]
        /// <summary>
        /// Get Items by category/tag name
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        DataResultSyndicationItems GetByCategory(DataFilterSyndication filter);

        [OperationContract]
        /// <summary>
        /// Get items by title
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        DataResultSyndicationItems GetByTitle(DataFilterSyndication filter);

        [OperationContract]
        /// <summary>
        /// Get last items
        /// </summary>
        /// <param name="howMany">the number of items to get</param>
        /// <returns></returns>
        DataResultSyndicationItems GetLast(DataFilterSyndication filter);
    }
}

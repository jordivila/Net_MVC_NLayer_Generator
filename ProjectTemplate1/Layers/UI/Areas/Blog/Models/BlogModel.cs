using System.ServiceModel.Syndication;
using $customNamespace$.Models.Syndication;
using $safeprojectname$.Models;

namespace $safeprojectname$.Areas.Blog.Models
{
    //[NonValidateModelOnHttpGet]
    public class BlogModel : baseViewModel
    {
        public string FeedId { get; set; }
        public SyndicationFeedFormatter FeedFormatter { get; set; }

        public DataFilterSyndication FeedFilter { get; set; }
        public DataResultSyndicationItems FeedItems { get; set; }
    }
}
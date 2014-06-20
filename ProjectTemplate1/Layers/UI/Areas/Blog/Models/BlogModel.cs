using System.ServiceModel.Syndication;
using $customNamespace$.Models.Syndication;
using $customNamespace$.UI.Web.Models;

namespace $customNamespace$.UI.Web.Areas.Blog.Models
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
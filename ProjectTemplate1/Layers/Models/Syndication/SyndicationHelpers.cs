using System.Linq;
using System.ServiceModel.Syndication;
using $safeprojectname$.Syndication;

namespace $safeprojectname$.ExtensionMethods
{
    public static class SyndicationFeedHelper
    {
        /// <summary>
        /// Selects all categories inside SyndicationItems and push them into an array
        /// </summary>
        /// <param name="feedFormatter"></param>
        /// <returns></returns>
        public static SyndicationCategory[] CategoriesGetFromItems(this SyndicationFeedFormatter feedFormatter)
        {
            SyndicationCategoryComparer categoryComparer = new SyndicationCategoryComparer();
            SyndicationCategory[] categories = feedFormatter.Feed.Items
                                        .Select(p => { return p.Categories; })
                                        .SelectMany(p => p)
                                        .Distinct(categoryComparer)
                                        .ToArray();
            return categories;
        }

        /// <summary>
        /// Remove SyndicationContent from SindycationItems decreasing DataOut passed between servers 
        /// </summary>
        /// <param name="feedFormatter"></param>
        /// <returns></returns>
        public static SyndicationFeedFormatter RemoveContent(this SyndicationFeedFormatter feedFormatter)
        {
            feedFormatter.Feed.Items = feedFormatter
                                        .Feed
                                        .Items
                                        .Select(p =>
                                        {
                                            p.Summary = new TextSyndicationContent(string.Empty, TextSyndicationContentKind.Plaintext);
                                            p.Content = new TextSyndicationContent(string.Empty, TextSyndicationContentKind.Plaintext);
                                            return p;
                                        });
            return feedFormatter;
        }

    }


}

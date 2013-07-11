using System.Collections.Generic;
using System.ServiceModel.Syndication;

namespace $safeprojectname$.Syndication
{
    public class SyndicationCategoryComparer : IEqualityComparer<SyndicationCategory>
    {
        public bool Equals(SyndicationCategory x, SyndicationCategory y)
        {
            return x.Name == y.Name;
        }

        public int GetHashCode(SyndicationCategory obj)
        {
            return string.CompareOrdinal(obj.Name, string.Empty);
        }
    }
}

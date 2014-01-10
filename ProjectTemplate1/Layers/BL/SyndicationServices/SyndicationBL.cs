using System.ServiceModel.Syndication;
using Microsoft.Practices.Unity;
using $customNamespace$.DAL.SyndicationServices;
using $customNamespace$.Models.Syndication;
using $customNamespace$.Models.Unity;

namespace $safeprojectname$.SyndicationServices
{
    public class SyndicationBL : BaseBL, IProviderSyndication
    {
        private ISyndicationDAL _dal;
        public SyndicationBL()
        {
            _dal = DependencyFactory.Resolve<ISyndicationDAL>();
        }
        public override void Dispose()
        {
            base.Dispose();

            if (this._dal != null)
            {
                this._dal.Dispose();
            }
        }

        public SyndicationFeedFormatter GetInfoWithNoItems()
        {
            return this._dal.GetInfoWithNoItems();
        }

        public DataResultSyndicationItems GetByCategory(DataFilterSyndication filter)
        {
            return this._dal.GetByCategory(filter);
        }

        public DataResultSyndicationItems GetByTitle(DataFilterSyndication filter)
        {
            return this._dal.GetByTitle(filter);
        }

        public DataResultSyndicationItems GetLast(DataFilterSyndication filter)
        {
            return this._dal.GetLast(filter);
        }
    }
}

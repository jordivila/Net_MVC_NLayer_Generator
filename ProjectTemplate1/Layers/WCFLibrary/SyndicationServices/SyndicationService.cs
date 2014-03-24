using System.ServiceModel.Syndication;
using $customNamespace$.BL.SyndicationServices;
using $customNamespace$.Models.Syndication;

namespace $customNamespace$.WCF.ServicesLibrary.SyndicationServices
{
    public class SyndicationService : BaseService, ISyndicationProxy
    {
        private ISyndicationProxy _bl;

        public SyndicationService()
        {
            _bl = new SyndicationBL();
        }

        public override void Dispose()
        {
            if (this._bl != null)
            {
                this._bl.Dispose();
            }

            base.Dispose();
        }

        public SyndicationFeedFormatter GetInfoWithNoItems()
        {
            return this._bl.GetInfoWithNoItems();
        }
        public DataResultSyndicationItems GetByCategory(DataFilterSyndication filter)
        {
            DataResultSyndicationItems result = this._bl.GetByCategory(filter);
            return result;
        }
        public DataResultSyndicationItems GetByTitle(DataFilterSyndication filter)
        {
            return this._bl.GetByTitle(filter);
        }
        public DataResultSyndicationItems GetLast(DataFilterSyndication filter)
        {
            return this._bl.GetLast(filter);
        }
    }
}
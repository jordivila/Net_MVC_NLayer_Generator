using System.ServiceModel.Syndication;
using $customNamespace$.BL.SyndicationServices;
using $customNamespace$.Models.Syndication;

namespace $safeprojectname$.SyndicationServices
{
    public class SyndicationService : ISyndicationProxy
    {
        private ISyndicationProxy _bl;

        public SyndicationService()
        {
            _bl = new SyndicationBL();
        }
        
        public void Dispose()
        {
            //base.Dispose();

            if (this._bl != null)
            {
                this._bl.Dispose();
            }
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

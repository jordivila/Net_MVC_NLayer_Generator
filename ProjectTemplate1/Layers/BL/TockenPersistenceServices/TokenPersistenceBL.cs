using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using $customNamespace$.DAL.TokenTemporaryPersistenceServices;
using $customNamespace$.Models.TokenPersistence;
using $customNamespace$.Models.Unity;

namespace $safeprojectname$.TokenTemporaryPersistenceServices
{
    public interface ITokenTemporaryPersistenceBL : ITokenTemporaryPersistenceServices { }

    public class TokenTemporaryPersistenceBL : BaseBL, ITokenTemporaryPersistenceBL
    {
        private ITokenTemporaryPersistenceDAL _dal;

        public TokenTemporaryPersistenceBL()
        {
            _dal = DependencyFactory.Resolve<ITokenTemporaryPersistenceDAL>();
        }

        public override void Dispose()
        {
            base.Dispose();

            if (this._dal != null)
            {
                this._dal.Dispose();
            }
        }

        public List<TokenTemporaryPersistenceServiceItem> GetAll()
        {
            return this._dal.GetAll();
        }

        public List<TokenTemporaryPersistenceServiceItem> FindBy(System.Linq.Expressions.Expression<Func<TokenTemporaryPersistenceServiceItem, bool>> predicate)
        {
            return this._dal.FindBy(predicate);
        }

        public void Insert(TokenTemporaryPersistenceServiceItem entity)
        {
            this._dal.Insert(entity);
        }

        public void Delete(TokenTemporaryPersistenceServiceItem entity)
        {
            this._dal.Delete(entity);
        }

        public void Update(TokenTemporaryPersistenceServiceItem entity)
        {
            this._dal.Update(entity);
        }

        public TokenTemporaryPersistenceServiceItem GetById(object id)
        {
            return this._dal.GetById(id);
        }
    }
}

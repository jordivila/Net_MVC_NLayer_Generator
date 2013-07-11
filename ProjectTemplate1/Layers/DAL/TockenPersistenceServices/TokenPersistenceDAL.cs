using System;
using System.Collections.Generic;
using $customNamespace$.Models.TokenPersistence;

namespace $safeprojectname$.TokenTemporaryPersistenceServices
{
    public class TokenTemporaryPersistenceDAL : BaseDAL, ITokenTemporaryPersistenceDAL
    {
        public override void Dispose()
        {
            base.Dispose();
        }

        public virtual List<TokenTemporaryPersistenceServiceItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public virtual List<TokenTemporaryPersistenceServiceItem> FindBy(System.Linq.Expressions.Expression<Func<TokenTemporaryPersistenceServiceItem, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual void Insert(TokenTemporaryPersistenceServiceItem entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(TokenTemporaryPersistenceServiceItem entity)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(TokenTemporaryPersistenceServiceItem entity)
        {
            throw new NotImplementedException();
        }

        public virtual TokenTemporaryPersistenceServiceItem GetById(object id)
        {
            throw new NotImplementedException();
        }
    }
}

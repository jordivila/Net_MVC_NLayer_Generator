using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using $customNamespace$.Models.TokenPersistence;
using $customNamespace$.DAL;
using $customNamespace$.DAL.TokenTemporaryPersistenceServices;
using $customNamespace$.Models.TokenPersistence;

namespace $customNamespace$.DAL.TokenTemporaryPersistenceServices
{
    public class TokenTemporaryPersistenceDAL : BaseDAL, ITokenTemporaryPersistenceDAL
    {
        public override void Dispose()
        {
            base.Dispose();
        }

        private static List<TokenTemporaryPersistenceServiceItem> _tokensList = new List<TokenTemporaryPersistenceServiceItem>();

        public List<TokenTemporaryPersistenceServiceItem> GetAll()
        {
            lock (_tokensList)
            {
                return TokenTemporaryPersistenceDAL._tokensList;
            }
        }

        public List<TokenTemporaryPersistenceServiceItem> FindBy(Expression<Func<TokenTemporaryPersistenceServiceItem, bool>> predicate)
        {
            lock (_tokensList)
            {

                return this.GetAll().Where(predicate.Compile()).ToList();
            }
        }

        public void Insert(TokenTemporaryPersistenceServiceItem entity)
        {
            lock (_tokensList)
            {

                TokenTemporaryPersistenceDAL._tokensList.Add(entity);
            }
        }

        public void Delete(TokenTemporaryPersistenceServiceItem entity)
        {
            lock (_tokensList)
            {

                var item = TokenTemporaryPersistenceDAL._tokensList.Where(x => x.Token == entity.Token);
                if (item.Count() > 0)
                {
                    TokenTemporaryPersistenceDAL._tokensList.Remove(item.First());
                }
            }
        }

        public void Update(TokenTemporaryPersistenceServiceItem entity)
        {
            throw new NotImplementedException();
        }

        public TokenTemporaryPersistenceServiceItem GetById(object id)
        {
            lock (_tokensList)
            {

                IEnumerable<TokenTemporaryPersistenceServiceItem> result = this.GetAll().Where(x => x.Token.ToString() == ((Guid)id).ToString());
                if (result.Count() > 0)
                {
                    return result.First();
                }
                else
                {
                    return null;
                }
            }
        }
    }
}

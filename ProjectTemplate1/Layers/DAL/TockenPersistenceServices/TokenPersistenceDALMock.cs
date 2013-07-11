using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using $customNamespace$.Models.TokenPersistence;

namespace $safeprojectname$.TokenTemporaryPersistenceServices
{
    public class TokenTemporaryPersistenceDALMock : TokenTemporaryPersistenceDAL, ITokenTemporaryPersistenceDAL
    {
        private static List<TokenTemporaryPersistenceServiceItem> _tokensList = new List<TokenTemporaryPersistenceServiceItem>();

        public override List<TokenTemporaryPersistenceServiceItem> GetAll()
        {
            lock (_tokensList)
            {
                return TokenTemporaryPersistenceDALMock._tokensList;    
            }
        }

        public  override List<TokenTemporaryPersistenceServiceItem> FindBy(Expression<Func<TokenTemporaryPersistenceServiceItem, bool>> predicate)
        {
            lock (_tokensList)
            {

                return this.GetAll().Where(predicate.Compile()).ToList();
            }
        }

        public  override void Insert(TokenTemporaryPersistenceServiceItem entity)
        {
            lock (_tokensList)
            {

                TokenTemporaryPersistenceDALMock._tokensList.Add(entity);
            }
        }

        public  override void Delete(TokenTemporaryPersistenceServiceItem entity)
        {
            lock (_tokensList)
            {

                var item = TokenTemporaryPersistenceDALMock._tokensList.Where(x => x.Token == entity.Token);
                if (item.Count() > 0)
                {
                    TokenTemporaryPersistenceDALMock._tokensList.Remove(item.First());
                }
            }
        }

        public  override void Update(TokenTemporaryPersistenceServiceItem entity)
        {
            throw new NotImplementedException();
        }

        public  override void Dispose()
        {
            
        }

        public  override TokenTemporaryPersistenceServiceItem GetById(object id)
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

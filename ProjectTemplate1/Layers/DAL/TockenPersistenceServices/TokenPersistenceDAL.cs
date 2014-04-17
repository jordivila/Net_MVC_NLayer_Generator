using Microsoft.Practices.EnterpriseLibrary.Data;
using $customNamespace$.Models;
using $customNamespace$.Models.Configuration;
using $customNamespace$.Models.Configuration.ConnectionProviders;
using $customNamespace$.Models.TokenPersistence;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace $customNamespace$.DAL.TokenTemporaryPersistenceServices
{
    public class TokenTemporaryDatabasePersistenceDAL<T> : BaseDAL, ITokenTemporaryPersistenceDAL<T>
    {
        public override void Dispose()
        {
            base.Dispose();
        }

        public TokenTemporaryPersistenceServiceItem<T> Insert(TokenTemporaryPersistenceServiceItem<T> entity)
        {
            Database db = DatabaseFactory.CreateDatabase(Info.GetDatabaseName(ApplicationConfiguration.DatabaseNames.TokenPersistence));
            DbCommand cmd = db.GetStoredProcCommand("TokenTemporaryPersistenceInsert");
            db.AddInParameter(cmd, "@id", DbType.String, entity.Token.ToString());
            db.AddInParameter(cmd, "@tokenCreated", DbType.DateTime, entity.TokenCreated);
            db.AddInParameter(cmd, "@tokenObjectSerialized", DbType.String, baseModel.SerializeObjectToJson(entity.TokenObject));
            db.ExecuteNonQuery(cmd);
            return entity;
        }

        public object Delete(TokenTemporaryPersistenceServiceItem<T> entity)
        {
            Database db = DatabaseFactory.CreateDatabase(Info.GetDatabaseName(ApplicationConfiguration.DatabaseNames.TokenPersistence));
            DbCommand cmd = db.GetStoredProcCommand("TokenTemporaryPersistenceDelete");
            db.AddInParameter(cmd, "@id", DbType.String, entity.Token.ToString());
            return db.ExecuteScalar(cmd);
        }

        public TokenTemporaryPersistenceServiceItem<T> Update(TokenTemporaryPersistenceServiceItem<T> entity)
        {
            throw new NotImplementedException();
        }

        public TokenTemporaryPersistenceServiceItem<T> GetById(object id)
        {
            TokenTemporaryPersistenceServiceItem<T> result = null;
            Database db = DatabaseFactory.CreateDatabase(Info.GetDatabaseName(ApplicationConfiguration.DatabaseNames.TokenPersistence));
            DbCommand cmd = db.GetStoredProcCommand("TokenTemporaryPersistenceGetById");
            db.AddInParameter(cmd, "@id", DbType.String, ((Guid)id).ToString());
            int i = this.ExecuteReader<TokenTemporaryPersistenceServiceItem<T>>(db, null, cmd, ref result);
            return result;
        }
    }

    public class TokenTemporaryInMemoryPersistenceDAL<T> : BaseDAL, ITokenTemporaryPersistenceDAL<T>
    {
        public override void Dispose()
        {
            base.Dispose();
        }

        private static Dictionary<Guid, TokenTemporaryPersistenceServiceItem<T>> _tokensList = new Dictionary<Guid, TokenTemporaryPersistenceServiceItem<T>>();

        public TokenTemporaryPersistenceServiceItem<T> Insert(TokenTemporaryPersistenceServiceItem<T> entity)
        {
            lock (_tokensList)
            {
                TokenTemporaryInMemoryPersistenceDAL<T>._tokensList.Add(entity.Token, entity);
            }

            return entity;
        }

        public object Delete(TokenTemporaryPersistenceServiceItem<T> entity)
        {
            int rowCount = 0;

            lock (_tokensList)
            {
                rowCount = TokenTemporaryInMemoryPersistenceDAL<T>._tokensList.Where(x => x.Key == entity.Token).Count();

                if (rowCount > 0)
                {
                    TokenTemporaryInMemoryPersistenceDAL<T>._tokensList.Remove(entity.Token);
                }
            }

            return rowCount;
        }

        public TokenTemporaryPersistenceServiceItem<T> Update(TokenTemporaryPersistenceServiceItem<T> entity)
        {
            throw new NotImplementedException();
        }

        public TokenTemporaryPersistenceServiceItem<T> GetById(object id)
        {
            lock (_tokensList)
            {
                if (TokenTemporaryInMemoryPersistenceDAL<T>._tokensList.Any(x => x.Key == ((Guid)id)))
                {
                    return TokenTemporaryInMemoryPersistenceDAL<T>._tokensList[((Guid)id)];
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
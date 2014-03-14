using $customNamespace$.Models.Common;
using $customNamespace$.Models.RepositoryPattern;

namespace $customNamespace$.Models.TokenPersistence
{
    public interface ITokenTemporaryPersistenceBL<T> : IRepository<TokenTemporaryPersistenceServiceItem<T>>
    {

    }

    public interface ITokenTemporaryPersistenceDAL<T> : IRepository<TokenTemporaryPersistenceServiceItem<T>>
    {

    }
}

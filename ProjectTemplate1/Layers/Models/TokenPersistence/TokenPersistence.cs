using System;
using $customNamespace$.Models.RepositoryPattern;
using System.Data;
using $customNamespace$.Models.Common;

namespace $customNamespace$.Models.TokenPersistence
{
    public interface ITokenTemporaryPersistenceServiceItem<T> : IDataReaderBindable
    {
        Guid Token { get; set; }
        DateTime TokenCreated { get; }
        T TokenObject { get; set; }
    }

    public class TokenTemporaryPersistenceServiceItem<T> : ITokenTemporaryPersistenceServiceItem<T>
    {
        public TokenTemporaryPersistenceServiceItem()
            : base()
        {
            this.Token = Guid.NewGuid();
            this.TokenCreated = DateTime.Now;
        }

        public TokenTemporaryPersistenceServiceItem(T obj)
            : this()
        {
            this.TokenObject = obj;
        }

        public Guid Token { get; set; }
        public DateTime TokenCreated { get; set; }
        public T TokenObject { get; set; }


        public void DataBind(IDataReader dr)
        {
            this.Token = Guid.Parse((string)dr["tokenId"]);
            this.TokenCreated = (DateTime)dr["tokenCreated"];
            this.TokenObject = (T)baseModel.DeserializeFromJson<T>((string)dr["tokenObjectSerialized"]);
        }
    }
}
using System;
using $safeprojectname$.RepositoryPattern;

namespace $safeprojectname$.TokenPersistence
{
    public interface ITokenTemporaryPersistenceServices : IRepository<TokenTemporaryPersistenceServiceItem>{}

    interface ITokenTemporaryPersistenceServiceItem
    {
        DateTime TokenCreated { get; }
        object TokenObject { get; set; }
        Guid Token { get; set; }
    }

    public class TokenTemporaryPersistenceServiceItem : ITokenTemporaryPersistenceServiceItem
    {
        private Guid _Token = Guid.NewGuid();
        private DateTime _TokenCreated = DateTime.Now;
        
        public Guid Token
        {
            get
            {
                return this._Token;
            }
            set
            {
                this._Token = value;
            }
        }
        public DateTime TokenCreated
        {
            get
            {
                return this._TokenCreated;
            }
        }
        public object TokenObject { get; set; }

        public TokenTemporaryPersistenceServiceItem() { }
        public TokenTemporaryPersistenceServiceItem(object obj)
        {
            this.TokenObject = obj;
        }

    }
}

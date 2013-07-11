using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace $safeprojectname$.RepositoryPattern
{
    public interface IRepository<T>: IDisposable where T : class
    {
        List<T> GetAll();
        List<T> FindBy(Expression<Func<T, bool>> predicate);
        T GetById(object id);
        void Insert(T entity);
        void Delete(T entity);
        void Update(T entity);
    }

}

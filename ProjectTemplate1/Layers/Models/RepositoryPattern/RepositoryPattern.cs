using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace $customNamespace$.Models.RepositoryPattern
{
    public interface IRepository<T> : IDisposable where T : class
    {
        //List<T> GetAll();
        
        // Find By Predicated
        //List<T> FindBy(Expression<Func<T, bool>> predicate);

        // Find By Predicate usage sample
        //public List<T> FindBy(Expression<Func<T, bool>> predicate)
        //{
        //    return this.listOfSomething.Where(predicate.Compile()).ToList();
        //}


        T GetById(object id);
        T Insert(T entity);
        T Update(T entity);
        object Delete(T entity);
    }

}

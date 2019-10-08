using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Roulette.DataAccess.Interfaces
{
    public interface IRepository<T> 
    {
        T Create();
        IQueryable<T> Find();
        IQueryable<T> FindReadOnly();
        IQueryable<T> Find(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindWithInclude(string[] includedNavigationEntities);
        T Find(object id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(int id);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);

        void DeleteAndSave(int id);
        void DeleteAndSave(T entity);
        void DeleteRangeAndSave(IEnumerable<T> entities);
        T EnsureFindSingle(System.Collections.Generic.ICollection<T> entities);
        T EnsureFindSingle(int id);
        T EnsureFindSingle(Expression<Func<T, bool>> predicate);
        T FindSingleOrNull(Expression<Func<T, bool>> predicate);
        V FindSingleValue<V>(Expression<Func<T, bool>> predicate, System.Linq.Expressions.Expression<Func<T, V>> selector);
        T InsertAndSave(T entity);
        T UpdateAndSave(T entity);

        void RejectChanges();
    }


}

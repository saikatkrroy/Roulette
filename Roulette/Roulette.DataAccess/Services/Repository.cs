using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Roulette.DataAccess.Interfaces;

namespace Roulette.DataAccess.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly RouletteDbContext _context;
        private readonly DbSet<T> _set;

        public Repository(DbContext context)
        {
            _context = context as RouletteDbContext;
            _set = context.Set<T>();
        }

        public T Create() => _set.Create();

        public IQueryable<T> Find()
        {
            var result=_set.AsQueryable();
            _context.SaveChanges();
            return result;
        }

        public IQueryable<T> FindReadOnly()
        {
            return _set.AsNoTracking();
        }

        public IQueryable<T> FindWithInclude(string[] includedNavigationEntities)
        {
            var query = _set.AsQueryable();
            if (includedNavigationEntities != null && includedNavigationEntities.Length > 0)
            {
                foreach (var i in includedNavigationEntities)
                {
                    query = query.Include(i);
                }

            }
            return query;
        }
        public T Find(object id)
        {
            return _set.Find(id);
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _set.Where(predicate);
        }

        public void Insert(T entity)
        {
            if (entity != null)
            {
                _set.Add(entity);
            }
        }

        public void Update(T entity)
        {
        }

        public void Delete(int id)
        {
            T entity = Find(id);
            if (entity != null)
            {
                _set.Remove(entity);
            }
        }

        public void Delete(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
            {
                _set.Attach(entity);
            }
            _set.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _set.RemoveRange(entities);
        }

        public V FindSingleValue<V>(Expression<Func<T, bool>> predicate,
            Expression<Func<T, V>> selector)
        {
            return Find(predicate).Select(selector).FirstOrDefault();
        }

        public T FindSingleOrNull(Expression<Func<T, bool>> predicate)
        {
            return Find(predicate).FirstOrDefault();
        }

        public T EnsureFindSingle(int id)
        {
            var entity = Find(id);
            ThrowNotFoundIfNull(entity, id);
            return entity;
        }

        public T EnsureFindSingle(Expression<Func<T, bool>> predicate)
        {
            var entity = FindSingleOrNull(predicate);
            ThrowNotFoundIfNull(entity);
            return entity;
        }

        public T EnsureFindSingle(ICollection<T> entities)
        {
            var entity = entities.FirstOrDefault();
            ThrowUnexpectedIfNull(entity);
            return entity;
        }

        public T InsertAndSave(T entity)
        {
            Insert(entity);
            _context.SaveChanges();
            return entity;
        }

        public T UpdateAndSave(T entity)
        {
            Update(entity);
            _context.SaveChanges();

            return entity;
        }

        public void DeleteAndSave(int id)
        {
            Delete(id);
            _context.SaveChanges();
        }

        public void DeleteAndSave(T entity)
        {
            Delete(entity);
            _context.SaveChanges();
        }

        public void DeleteRangeAndSave(IEnumerable<T> entities)
        {
            DeleteRange(entities);
            _context.SaveChanges();
        }

        public void RejectChanges()
        {
            _context.RejectChanges();
        }

        protected void ThrowUnexpected(string message = null, Exception innerException = null)
        {
            throw new Exception(message, innerException);
        }

        protected void ThrowUnexpectedIfNull(object obj,
            string message = null, Exception innerException = null)
        {
            if (obj == null)
                throw new Exception(message, innerException);
        }

        protected void ThrowBadDataIf(bool shouldThrow,
            string message = null, Exception innerException = null)
        {
            if (shouldThrow)
                throw new Exception(message, innerException);
        }

        protected void ThrowNotFoundIfNull(T entity)
        {
            if (entity == null)
            {
                var message = "Can't find item of type " + typeof(T).Name + ".";
                throw new Exception(message);
            }
        }

        protected void ThrowNotFoundIfNull(T entity, int id)
        {
            if (entity == null)
            {
                var message = "Can't find item of type " + typeof(T).Name + " for id " + id + ".";
                throw new Exception(message);
            }
        }

    }
}

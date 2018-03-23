using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace BNE.Web.Services.Solr.Database.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {

        private DbContext _dataContext;
        private readonly IDbSet<T> _dbset;

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected DbContext DataContext
        {
            get { return _dataContext ?? (_dataContext = DatabaseFactory.Get()); }
        }

        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _dbset = DataContext.Set<T>();
        }

        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
            _dataContext.SaveChanges();
        }
        public virtual void Update(T entity)
        {
            _dbset.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
            _dataContext.SaveChanges();
        }
        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
            _dataContext.SaveChanges();
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                _dbset.Remove(obj);
        }
        public virtual T GetById(long id)
        {
            return _dbset.Find(id);
        }
        public virtual T GetById(string id)
        {
            return _dbset.Find(id);
        }
        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.ToList();
        }
        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbset.Where(where).AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }
            return query;
        }
        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).FirstOrDefault<T>();
        }
        public virtual IQueryable<T> GetManyQueryable(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbset.Where(where).AsQueryable();
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }
            return query;
        }

    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace BNE.Dashboard.Data.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class, new()
    {
        private DashboardEntities _dataContext;
        private readonly IDbSet<T> _dbset;
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _dbset = DataContext.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected DashboardEntities DataContext
        {
            get { return _dataContext ?? (_dataContext = DatabaseFactory.Get()); }
        }
        public virtual T Add(T entity)
        {
            _dbset.Add(entity);
            return entity;
        }
        public virtual void Update(T entity)
        {
            _dbset.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = _dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                _dbset.Remove(obj);
        }
        public virtual T GetById(int id)
        {
            return _dbset.Find(id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.ToList();
        }
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).ToList();
        }

        public virtual IQueryable<T> Query(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where);
        }

        public virtual int ExecuteStoredProcedure(string storedProcedureName, Dictionary<string, string> parms)
        {
            var parameters = new List<ObjectParameter>();

            if (parms.Count > 0)
                parameters.AddRange(parms.Select(parm => new ObjectParameter(parm.Key, parm.Value)));

            return ((IObjectContextAdapter)DataContext).ObjectContext.ExecuteFunction(storedProcedureName, parameters.ToArray());
        }

    }
}

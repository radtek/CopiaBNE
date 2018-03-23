using BNE.Comum.Domain.Localizable;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace BNE.Data.Infrastructure
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
            get
            {
                if (_dataContext == null)
                {

                    _dataContext = DatabaseFactory.Get();
                    ((IObjectContextAdapter)_dataContext).ObjectContext.ObjectMaterialized += TranslateEntity_ObjectMaterialized;
                }
                return _dataContext;
            }
        }
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _dbset = DataContext.Set<T>();
        }
        public virtual void Add(T entity)
        {
            _dbset.Add(entity);
        }
        public virtual void Update(T entity)
        {
            _dbset.Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Update(T entity, T property)
        {
            _dbset.Attach(entity);
            _dataContext.Entry(entity).Property(n => property).IsModified = true;
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
        public T Get(Expression<Func<T, bool>> where)
        {
            return _dbset.Where(where).FirstOrDefault<T>();
        }
        public virtual IEnumerable<T> GetAll()
        {
            return _dbset.ToList();
        }
        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            var query = _dbset.Where(where).AsQueryable();
            if (includes != null)
                query = includes.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }

        /// <summary>
        /// Método chamado a quando uma entidade é contruída para tentar efetuar a tradução
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void TranslateEntity_ObjectMaterialized(object sender, ObjectMaterializedEventArgs e)
        {
            if (e.Entity == null)
                return;

            if (e.Entity is Comum.Model.Localizable.ILocalizableEntity)
                Translation.Translate(e.Entity, _dataContext);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BNE.ExceptionLog.Model;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BNE.ExceptionLog.Data.Infrastructure
{
    public abstract class RepositoryBase<T> where T : MessageBase
    {

        private IMongoDatabase _dataContext;
        private readonly IMongoCollection<T> _dbset;
        protected IDatabaseFactory DatabaseFactory { get; private set; }

        protected IMongoDatabase DataContext
        {
            get { return _dataContext ?? (_dataContext = DatabaseFactory.Get()); }
        }

        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            _dbset = DataContext.GetCollection<T>(typeof(T).Name);
        }

        public virtual void Add(T entity)
        {
            _dbset.InsertOneAsync(entity);
        }

        public virtual void Update(T entity)
        {
            var filter = Builders<T>.Filter.Eq("Id", entity.Id);
            
            _dbset.ReplaceOneAsync(filter, entity);

            //bset.Attach(entity);
            //_dataContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Update(T entity, T property)
        {
            //_dbset.Attach(entity);
            //_dataContext.Entry(entity).Property(n => property).IsModified = true;
        }

        public virtual void Delete(T entity)
        {
            //_dbset.Remove(entity);
            //_dataContext.SaveChanges();
        }

        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            //IEnumerable<T> objects = _dbset.Where<T>(where).AsEnumerable();
            //foreach (T obj in objects)
            //    _dbset.Remove(obj);
        }

        public virtual T GetById(long id)
        {
            //return _dbset.Find(id);
            return null;
        }

        public virtual T GetById(string id)
        {
            //return _dbset.Find(id);
            return null;
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            var retorno = _dbset.Find(where).FirstOrDefaultAsync();

            return retorno.Result;
        }

        public virtual IEnumerable<T> GetAll()
        {
            var retorno = _dbset.Find(new BsonDocument()).ToListAsync();

            return retorno.Result.AsEnumerable();
        }

        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            //var query = _dbset.Where(where).AsQueryable();
            //if (includes != null)
            //    query = includes.Aggregate(query, (current, include) => current.Include(include));

            //return query;
            return null;
        }
    }
}

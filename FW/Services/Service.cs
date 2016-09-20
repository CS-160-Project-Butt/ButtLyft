using AASC.FW.Infrastructure;
using AASC.FW.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.OData.Query;
using System.Linq.Expressions;

namespace AASC.FW.Services
{
    public abstract class Service<T> : IService<T> where T : IObjectState
    {
        private readonly IRepositoryAsync<T> _repository;

        protected Service(IRepositoryAsync<T> repository)
        {
            _repository = repository;
        }

        public virtual T Find(params object[] keyValues)
        {
            return _repository.Find(keyValues);
        }

        public virtual IQueryable<T> SelectQuery(string query, params object[] parameters)
        {
            return _repository.SelectQuery(query, parameters).AsQueryable();
        }

        public virtual void Insert(T entity)
        {
            _repository.Insert(entity);
        }

        public virtual void InsertRange(IEnumerable<T> entities)
        {
            _repository.InsertRange(entities);
        }

        public virtual void InsertGraph(T entity)
        {
            _repository.Insert(entity);
        }

        public virtual void InsertGraphRange(IEnumerable<T> entities)
        {
            _repository.InsertGraphRange(entities);
        }

        public virtual void Update(T entity)
        {
            _repository.Update(entity);
        }

        public virtual void Delete(object id)
        {
            _repository.Delete(id);
        }

        public virtual void Delete(T entity)
        {
            _repository.Delete(entity);
        }

        public IQueryFluent<T> Query()
        {
            return _repository.Query();
        }

        public virtual IQueryFluent<T> Query(IQueryObject<T> queryObject)
        {
            return _repository.Query(queryObject);
        }

        public virtual IQueryFluent<T> Query(Expression<Func<T, bool>> query)
        {
            return _repository.Query(query);
        }

        public virtual async Task<T> FindAsync(params object[] keyValues)
        {
            return await _repository.FindAsync(keyValues);
        }

        public virtual async Task<T> FindAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await _repository.FindAsync(cancellationToken, keyValues);
        }

        public virtual async Task<bool> DeleteAsync(params object[] keyValues)
        {
            return await DeleteAsync(CancellationToken.None, keyValues);
        }

        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            return await DeleteAsync(cancellationToken, keyValues);
        }

        public IQueryable ODataQueryable(ODataQueryOptions<T> oDataQueryOptions)
        {
            return _repository.Queryable(oDataQueryOptions);
        }

        public IQueryable<T> ODataQueryable()
        {
            return _repository.Queryable();
        }
    }
}

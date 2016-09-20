using AASC.FW.Infrastructure;
using AASC.FW.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.OData.Query;

namespace AASC.FW.Services
{
    public interface IService<T> where T : IObjectState
    {
        T Find(params object[] keyValues);
        IQueryable<T> SelectQuery(string query, params object[] parameters);
        void Insert(T entity);
        void InsertRange(IEnumerable<T> entities);
        void InsertGraph(T entity);
        void InsertGraphRange(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(object id);
        void Delete(T entity);
        IQueryFluent<T> Query();
        IQueryFluent<T> Query(IQueryObject<T> queryObject);
        IQueryFluent<T> Query(Expression<Func<T, bool>> query);
        Task<T> FindAsync(params object[] keyValues);
        Task<T> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        Task<bool> DeleteAsync(params object[] keyValues);
        Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);
        IQueryable ODataQueryable(ODataQueryOptions<T> oDataQueryOptions);
        IQueryable<T> ODataQueryable();


    }
}

using AASC.FW.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.OData.Query;

namespace AASC.FW.Repositories
{
    public interface IRepository<T> where T : IObjectState
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
        IQueryFluent<T> Query(IQueryObject<T> queryObject);
        IQueryFluent<T> Query(Expression<Func<T, bool>> query);
        IQueryFluent<T> Query();
        IQueryable Queryable(ODataQueryOptions<T> oDataQueryOptions);
        IQueryable<T> Queryable();
        IRepository<T> GetRepository<T>() where T : IObjectState;
    }
}

using AASC.FW.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AASC.FW.Repositories
{
    public interface IQueryFluent<T> where T : IObjectState
    {
        IQueryFluent<T> OrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy);
        IQueryFluent<T> Include(Expression<Func<T, object>> expression);
        IEnumerable<T> SelectPage(int page, int pageSize, out int totalCount);
        IEnumerable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector = null);
        IEnumerable<T> Select();
        Task<IEnumerable<T>> SelectAsync();
        IQueryable<T> SqlQuery(string query, params object[] parameters);
    }
}
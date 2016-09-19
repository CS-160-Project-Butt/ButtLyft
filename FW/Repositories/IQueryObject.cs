using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AASC.FW.Repositories
{
    public interface IQueryObject<T>
    {
        Expression<Func<T, bool>> Query();
        Expression<Func<T, bool>> And(Expression<Func<T, bool>> query);
        Expression<Func<T, bool>> Or(Expression<Func<T, bool>> query);
        Expression<Func<T, bool>> And(IQueryObject<T> queryObject);
        Expression<Func<T, bool>> Or(IQueryObject<T> queryObject);
    }
}

using AASC.FW.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqKit;

namespace AASC.FW.EF6
{
    public abstract class QueryObject<T> : IQueryObject<T>
    {
        private Expression<Func<T, bool>> _query;
        public virtual Expression<Func<T, bool>> Query()
        {
            return _query;
        }

        public Expression<Func<T, bool>> And(Expression<Func<T, bool>> query)
        {
            return _query == null ? query : _query.And(query.Expand());
        }

        public Expression<Func<T, bool>> Or(Expression<Func<T, bool>> query)
        {
            return _query == null ? query : _query.Or(query.Expand());
        }

        public Expression<Func<T, bool>> And(IQueryObject<T> queryObject)
        {
            return And(queryObject.Query());
        }

        public Expression<Func<T, bool>> Or(IQueryObject<T> queryObject)
        {
            return Or(queryObject.Query());
        }

        protected void Add(Expression<Func<T, bool>> predicate)
        {
            _query = (_query == null) ? predicate : _query.And(predicate.Expand());
        }
    }
}

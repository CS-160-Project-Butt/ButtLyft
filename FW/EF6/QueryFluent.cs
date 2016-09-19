using AASC.FW.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AASC.FW.EF6
{
    public sealed class QueryFluent<T> : IQueryFluent<T> where T : Entity
    {
        private readonly Expression<Func<T, bool>> _expression;
        private readonly List<Expression<Func<T, object>>> _includes;
        private readonly Repository<T> _repository;
        private Func<IQueryable<T>, IOrderedQueryable<T>> _orderBy;

        public QueryFluent(Repository<T> repository)
        {
            _repository = repository;
            _includes = new List<Expression<Func<T, object>>>();
        }

        public QueryFluent(Repository<T> repository, IQueryObject<T> queryObject)
            : this(repository)
        {
            _expression = queryObject.Query();
        }

        public QueryFluent(Repository<T> repository, Expression<Func<T, bool>> expression)
            : this(repository)
        {
            _expression = expression;
        }

        public IQueryFluent<T> OrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy)
        {
            _orderBy = orderBy;
            return this;
        }

        public IQueryFluent<T> Include(Expression<Func<T, object>> expression)
        {
            _includes.Add(expression);
            return this;
        }

        public IEnumerable<T> SelectPage(int page, int pageSize, out int totalCount)
        {
            totalCount = _repository.Select(_expression).Count();
            return _repository.Select(_expression, _orderBy, _includes, page, pageSize);
        }
        public IEnumerable<T> Select()
        {
            return _repository.Select(_expression, _orderBy, _includes);
        }

        public IEnumerable<TResult> Select<TResult>(Expression<Func<T, TResult>> selector)
        {
            return _repository.Select(_expression, _orderBy, _includes).Select(selector);
        }

        public async Task<IEnumerable<T>> SelectAsync()
        {
            return await _repository.SelectAsync(_expression, _orderBy, _includes);
        }

        public IQueryable<T> SqlQuery(string query, params object[] parameters)
        {
            return _repository.SelectQuery(query, parameters).AsQueryable();
        }
    }
}

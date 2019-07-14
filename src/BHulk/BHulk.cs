using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using BHulk.Util;
using Microsoft.EntityFrameworkCore;

namespace BHulk
{
    public class BHulk<TEntity> : ISetterActions<TEntity>, IBulkAction<TEntity> 
        where TEntity : class
    {
        private readonly List<string> _setters = new List<string>();
        private readonly List<object> _values = new List<object>();
        private readonly List<object> _indexes = new List<object>();
        private readonly Func<DbContext> _contextFactory;
        private int _stepOf;
        private int _step;
        private SqlBuilder<TEntity> _sqlBuilder;

        private BHulk(Func<DbContext> contextFactory) => _contextFactory = contextFactory;

        public static ISetterActions<TEntity> UseContext(Func<DbContext> contextFactory) =>
            new BHulk<TEntity>(contextFactory);

        public ISetterActions<TEntity> Set(Expression<Func<TEntity, object>> predicate, object value)
        {
            var member = predicate.Body.GetMember();
            member.CheckValueType(value);

            AddSetter(value, member.Name);
            return this;
        }

        public ISetterActions<TEntity> Set(string columnName, object value)
        {
            AddSetter(value,columnName);
            return this;
        }

        public IBulkAction<TEntity> InStepOf(int stepOf)
        {
            _stepOf = stepOf;
            _step = (_indexes.Count() + _stepOf - 1) / stepOf;
            return this;
        }

        public IBulkAction<TEntity> For<T>(IEnumerable<T> indexes)
        {
            var enumerable = indexes as T[] ?? indexes.ToArray();
            if (!enumerable.Any()) return this;
            foreach (var index in enumerable) _indexes.Add(index);

            return this;
        }

        public IBulkAction<TEntity> For(Expression<Func<TEntity, bool>> predicate)
        {
            using (var context = _contextFactory())
            {
                var field = context.GetEntityType<TEntity>().GetPrimaryKey().Name;

                _indexes.AddRange(context.Set<TEntity>().AsNoTracking().Where(predicate)
                    .Select( CreateNewStatement(field) ).ToList());
            }
            return this;
        }

        public int Execute()
        {
            var myTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None,
                    TaskContinuationOptions.None,
                    TaskScheduler.Default);

            return myTaskFactory
                .StartNew<Task<int>>(ExecuteAsync)
                .Unwrap<int>()
                .GetAwaiter()
                .GetResult();

        }

        public async Task<int> ExecuteAsync()
        {
            var affectedRows = 0;
            for (var i = 0; i < _step; i++)
            {
                var indexParams = _indexes.Skip(i * _stepOf).Take(_stepOf).ToList();
                using (var context = _contextFactory())
                {
                    var sql = GetSql(context, indexParams);
                    affectedRows += await context.Database.ExecuteSqlCommandAsync(sql, _values.Concat(indexParams));
                }
            }
            return affectedRows;
        }

        private void AddSetter(object value, string memberName)
        {
            var parameterIndex = _setters.Count == 0 ? 0 : (_setters.Count + 1) - 1;

            _setters.Add($"{memberName} = {{{parameterIndex}}}");
            _values.Add(value);
        }

        private string GetSql(DbContext context, IEnumerable<object> indexParams)
        {
            _sqlBuilder = _sqlBuilder ?? SqlBuilder<TEntity>.Build(context, _setters);
                return _sqlBuilder.AppendInStatement(indexParams);
        }

        private static Expression<Func<TEntity, object>> CreateNewStatement(string fields)
        {
            var param = Expression.Parameter(typeof(TEntity), "item");

            return Expression.Lambda<Func<TEntity, object>>(Expression.Convert(Expression.Property(param, fields), typeof(object)), param);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using BHulk.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
        private string _baseSql;

        private BHulk(Func<DbContext> contextFactory) => _contextFactory = contextFactory;

        public static ISetterActions<TEntity> UseContext(Func<DbContext> contextFactory) =>
            new BHulk<TEntity>(contextFactory);

        public ISetterActions<TEntity> Set(Expression<Func<TEntity, object>> predicate, object value)
        {
            var member = GetMember(predicate.Body);
            CheckType(member, value);

            var parameterIndex = _setters.Count == 0 ? 0 : (_setters.Count + 1) - 1;

            _setters.Add($"{member.Name} = {{{parameterIndex}}}");
            _values.Add(value);
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

        public async Task<int> ExecuteAsync()
        {
            var affectedRows = 0;
            for (var i = 0; i < _step; i++)
            {                
                var indexParams = _indexes.Skip(i * _stepOf).Take(_stepOf).ToList();
                using (var context = _contextFactory())
                {
                    var inParams = Enumerable.Range(_setters.Count, indexParams.Count)
                        .Select(j => $"{{{j}}}")
                        .ToArray();

                    _baseSql = _baseSql ?? BuildBaseSql<TEntity>(context, _setters);

                    var rSql = $"{_baseSql} ({string.Join(',', inParams)})";
                    affectedRows += await context.Database.ExecuteSqlCommandAsync(rSql, _values.Concat(indexParams));
                }
            }
            return affectedRows;
        }

        private static MemberInfo GetMember(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.MemberAccess:
                    return ((MemberExpression)expression).Member;
                case ExpressionType.Convert:
                    return GetMember(((UnaryExpression)expression).Operand);
                default:
                    throw new NotSupportedException(expression.NodeType.ToString());
            }
        }

        private static void CheckType(MemberInfo member, object value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            if(((PropertyInfo)member).PropertyType != value.GetType() )
                throw new ArgumentException($"Invalid value for {member.Name}");
        }

        private static string BuildBaseSql<T>(IDbContextDependencies context, IEnumerable<string> setters) where T : class
        {
            var entityType = context.GetEntityType<T>();

            return $"UPDATE {entityType.GetTableName()} SET {string.Join(", ", setters)} WHERE {entityType.GetPrimaryKey().Name} IN";
        }


        

        private static Expression<Func<TEntity, object>> CreateNewStatement(string fields)
        {
            var param = Expression.Parameter(typeof(TEntity), "item");

            return Expression.Lambda<Func<TEntity, object>>(Expression.Convert(Expression.Property(param, fields), typeof(object)), param);
        }

    }
}

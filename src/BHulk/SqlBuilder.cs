using System.Collections.Generic;
using System.Linq;
using BHulk.Util;
using Microsoft.EntityFrameworkCore;

namespace BHulk
{
    internal class SqlBuilder<TEntity> where TEntity : class
    {
        private readonly int _settersCount;
        private readonly string _baseSql;

        private SqlBuilder(DbContext context, IEnumerable<string> setters)
        {
            var entityType = context.GetEntityType<TEntity>();

            var enumerable = setters as string[] ?? setters.ToArray();
            _settersCount = enumerable.Count();

            _baseSql = $"UPDATE {entityType.GetTableName()} " +
                   $"SET {string.Join(", ", enumerable)} WHERE {entityType.GetPrimaryKey().Name} IN";
        }
        internal static SqlBuilder<TEntity> Build(DbContext context, IEnumerable<string> setters) =>
            new SqlBuilder<TEntity>(context, setters);

        internal string AppendInStatement(IEnumerable<object> indexParams)
        {
            var inParams = Enumerable.Range(_settersCount, indexParams.Count())
                .Select(j => $"{{{j}}}")
                .ToArray();
            return $"{_baseSql} ({string.Join(',', inParams)})";
        }
    }
}
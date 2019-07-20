using System.Collections.Generic;
using System.Linq;
using BHulk.Util;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BHulk
{
    internal class SqlBuilder<TEntity> where TEntity : class
    {
        private readonly int _settersCount;
        private readonly string _baseSql;
        private readonly IEntityType _entityType;

        private SqlBuilder(DbContext context, IEnumerable<string> setters)
        {
            _entityType = context.GetEntityType<TEntity>();

            var enumerable = setters as string[] ?? setters.ToArray();
            _settersCount = enumerable.Count();

            _baseSql = $"UPDATE {_entityType.GetTableName()} " +
                   $"SET {string.Join(", ", enumerable)} WHERE";
        }
        internal static SqlBuilder<TEntity> Build(DbContext context, IEnumerable<string> setters) =>
            new SqlBuilder<TEntity>(context, setters);

        internal string AppendInStatement(IEnumerable<object> indexParams)
        {
            var inParams = Enumerable.Range(_settersCount, indexParams.Count())
                .Select(j => $"{{{j}}}")
                .ToArray();
            return $"{_baseSql} {_entityType.GetPrimaryKey().Name} IN ({string.Join(',', inParams)})";
        }
    }
}
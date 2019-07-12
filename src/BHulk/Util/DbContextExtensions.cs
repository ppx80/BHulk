using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BHulk.Util
{
    internal static class DbContextExtensions
    {
        internal static IEntityType GetEntityType<T>(this IDbContextDependencies context)
        {
            var entityType = context.Model.FindEntityType(typeof(T));

            if (entityType == null)
                throw new ArgumentException($"The entity {typeof(T).Name} was not found in the model of the current context");
            return entityType;
        }
    }
}

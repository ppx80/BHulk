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
            return context.Model.FindEntityType(typeof(T)) ??
                   throw new ArgumentException($"The entity {typeof(T).Name} was not found in the model of the current context");
        }
    }
}

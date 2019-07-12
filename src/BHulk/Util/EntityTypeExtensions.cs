using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BHulk.Util
{
    internal static class EntityTypeExtensions
    {
        internal static IProperty GetPrimaryKey(this IEntityType entityType) =>
            entityType.FindPrimaryKey().Properties.First();
    }
}

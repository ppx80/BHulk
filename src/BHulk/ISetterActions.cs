using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BHulk
{
    public interface ISetterActions<T>
    {
        ISetterActions<T> Set(Expression<Func<T, object>> expression, object value);
        IBulkAction<T> For<T1>(IEnumerable<T1> indexes);

        IBulkAction<T> For(Expression<Func<T, bool>> predicate);
    }
}

using System.Linq.Expressions;
using Usp_Project.Utils;

namespace USP_Project.Data.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, IEnumerable<Expression<Func<T, bool>>> filters)
    {
        if (queryable is null) throw new ArgumentNullException(nameof(queryable));

        foreach (var filter in filters.OrEmptyIfNull().IgnoreNullValues())
        {
            queryable = queryable.Where(filter);
        }

        return queryable;
    }

    public static IQueryable<T> Transform<T>(this IQueryable<T> queryable,
        IEnumerable<Func<IQueryable<T>, IQueryable<T>>> transformations)
    {
        if (queryable is null) throw new ArgumentNullException(nameof(queryable));

        foreach (var transformation in transformations.OrEmptyIfNull().IgnoreNullValues())
        {
            queryable = transformation(queryable);
        }

        return queryable;
    }
}
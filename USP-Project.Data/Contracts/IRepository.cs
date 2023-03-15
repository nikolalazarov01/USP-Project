using System.Linq.Expressions;
using Usp_Project.Utils;

namespace USP_Project.Data.Contracts;

public interface IRepository<T> where T : class
{
    Task<OperationResult> CreateAsync(T entity, CancellationToken token);
    Task<OperationResult<bool>> AnyAsync(IEnumerable<Expression<Func<T, bool>>> filters, CancellationToken token);
    Task<OperationResult<T>> GetAsync(IEnumerable<Expression<Func<T, bool>>> filters,
        IEnumerable<Func<IQueryable<T>, IQueryable<T>>> transformations, CancellationToken token);
}
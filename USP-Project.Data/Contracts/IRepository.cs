using System.Linq.Expressions;
using Usp_Project.Utils;

namespace USP_Project.Data.Contracts;

public interface IRepository<T> where T : class
{
    Task<OperationResult> CreateAsync(T entity, CancellationToken token);
    Task<OperationResult<bool>> AnyAsync(IEnumerable<Expression<Func<T, bool>>> filters, CancellationToken token);
}
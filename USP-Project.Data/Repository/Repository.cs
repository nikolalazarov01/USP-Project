using Microsoft.EntityFrameworkCore;
using USP_Project.Data.Contracts;
using Usp_Project.Utils;

namespace USP_Project.Data.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly UspDbContext _db;
    protected readonly DbSet<T> _dbSet;

    public Repository(UspDbContext db)
    {
        _db = db;
        _dbSet = _db.Set<T>();
    }

    public async Task<OperationResult> CreateAsync(T entity, CancellationToken token)
    {
        var operationResult = new OperationResult();
        if (!operationResult.ValidateNotNull(entity)) return operationResult;
        
        try
        {
            await this._db.AddAsync(entity, token);
            await this._db.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            operationResult.AppendError(ex);
        }

        return operationResult;
    }
}
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using USP_Project.Data.Contracts;
using USP_Project.Data.Extensions;
using Usp_Project.Utils;

namespace USP_Project.Data.Repository;

public class Repository<T> : IRepository<T> where T : class, IEntity
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

    public async Task<OperationResult<bool>> AnyAsync(IEnumerable<Expression<Func<T, bool>>> filters, CancellationToken token)
    {
        var operationResult = new OperationResult<bool>();

        try
        {
            var result = await this._db.Set<T>().Filter(filters).AnyAsync(token);
            operationResult.Data = result;
        }
        catch (Exception ex)
        {
            operationResult.AppendError(ex);
        }

        return operationResult;
    }

    public async Task<OperationResult<T>> GetAsync(IEnumerable<Expression<Func<T, bool>>> filters, IEnumerable<Func<IQueryable<T>, IQueryable<T>>> transformations, CancellationToken token)
    {
        var operationResult = new OperationResult<T>();

        try
        {
            var result = await this._db.Set<T>().Filter(filters).Transform(transformations).FirstOrDefaultAsync(token);
            operationResult.Data = result;
        }
        catch (Exception ex)
        {
            operationResult.AppendError(ex);
        }

        return operationResult;
    }

    public async Task<OperationResult<IEnumerable<T>>> GetManyAsync(IEnumerable<Expression<Func<T, bool>>> filters, IEnumerable<Func<IQueryable<T>, IQueryable<T>>> transformations, CancellationToken token)
    {
        var operationResult = new OperationResult<IEnumerable<T>>();

        try
        {
            var result = await this._db.Set<T>().Filter(filters).Transform(transformations).ToListAsync(token);
            operationResult.Data = result;
        }
        catch (Exception ex)
        {
            operationResult.AppendError(ex);
        }

        return operationResult;
    }

    public async Task<OperationResult> UpdateAsync(T entity, CancellationToken token)
    {
        var operationResult = new OperationResult();
        if (!operationResult.ValidateNotNull(entity)) return operationResult;

        try
        {
            var trackedEntity = await this._db.Set<T>().FirstOrDefaultAsync(e => e.Id == entity.Id, token);
            if (trackedEntity is not null) this._db.Entry(trackedEntity).State = EntityState.Detached;
            this._db.Entry(entity).State = EntityState.Modified;

            this._db.Set<T>().Update(entity);
            await this._db.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            operationResult.AppendError(ex);
        }

        return operationResult;
    }
}
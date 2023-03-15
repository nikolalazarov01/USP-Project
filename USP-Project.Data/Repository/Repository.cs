using Microsoft.EntityFrameworkCore;
using USP_Project.Data.Contracts;

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
}
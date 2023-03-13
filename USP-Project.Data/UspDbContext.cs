using Microsoft.EntityFrameworkCore;

namespace USP_Project.Data;

public class UspDbContext : DbContext
{
    public UspDbContext(DbContextOptions options) : base(options)
    {
    }
}
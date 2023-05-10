using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using USP_Project.Data.Models;

namespace USP_Project.Data.Seeding;

internal sealed class BrandSeeder : ISeeder
{
    public int Priority => 2;
    
    private readonly IServiceScopeFactory _scopeFactory;

    public BrandSeeder(IServiceScopeFactory scopeFactory)
        => _scopeFactory = scopeFactory;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<UspDbContext>();

        if(await context.Brands.AnyAsync(cancellationToken)) return;

        var brands = new Brand[]
        {
            new()
            {
                Name = "BMW",
                Description = "Cool German Sports Cars",
                Id = Guid.NewGuid()
            }
        };
        
        context.Brands.AddRange(brands);
        await context.SaveChangesAsync(cancellationToken);
    }
}
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

        var brands = GetBrands().ToList();
        
        context.Brands.AddRange(brands);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static IEnumerable<Brand> GetBrands()
        => new Brand[]
        {
            new()
            {
                Name = "BMW",
                Description = "Cool German Sports Cars",
                Id = Guid.NewGuid()
            },
            new()
            {
                Name = "Audi",
                Description = "Cool German Sports Cars #2",
                Id = Guid.NewGuid()
            },
            new()
            {
                Name = "Toyota",
                Description = "Cool Japanese Sports Cars",
                Id = Guid.NewGuid()
            },
            new()
            {
                Name = "Honda",
                Description = "Cool Japanese Sports Cars #2",
                Id = Guid.NewGuid()
            },
            new()
            {
                Name = "Ferrari",
                Description = "Cool Italian Sports Cars",
                Id = Guid.NewGuid()
            },
            new()
            {
                Name = "Lamborghini",
                Description = "Cool Italian Sports Cars #2",
                Id = Guid.NewGuid()
            },
            new()
            {
                Name = "Aston Martin",
                Description = "Cool British Sports Cars",
                Id = Guid.NewGuid()
            },
        };
}
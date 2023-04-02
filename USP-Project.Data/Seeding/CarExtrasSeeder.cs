using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using USP_Project.Data.Models;

namespace USP_Project.Data.Seeding;

internal sealed class CarExtrasSeeder : ISeeder
{
    public int Priority => 100;
    
    private readonly IServiceScopeFactory _scopeFactory;

    public CarExtrasSeeder(IServiceScopeFactory scopeFactory)
        => _scopeFactory = scopeFactory;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<UspDbContext>();

        if(await context.CarExtras.AnyAsync(cancellationToken)) return;
        
        var carsExtras= Array.Empty<CarsExtras>();
        
        context.CarExtras.AddRange(carsExtras);
        await context.SaveChangesAsync(cancellationToken);
    }
}
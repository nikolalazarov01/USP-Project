using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using USP_Project.Data.Models;

namespace USP_Project.Data.Seeding;

internal sealed class CarSeeder : ISeeder
{
    public int Priority => 10;
    
    private readonly IServiceScopeFactory _scopeFactory;

    public CarSeeder(IServiceScopeFactory scopeFactory)
        => _scopeFactory = scopeFactory;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<UspDbContext>();

        if(await context.Cars.AnyAsync(cancellationToken)) return;
        
        var cars = Array.Empty<Car>();
        
        context.Cars.AddRange(cars);
        await context.SaveChangesAsync(cancellationToken);
    }
}
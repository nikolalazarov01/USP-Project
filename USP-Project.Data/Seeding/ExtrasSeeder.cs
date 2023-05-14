using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using USP_Project.Data.Models;

namespace USP_Project.Data.Seeding;

internal sealed class ExtrasSeeder : ISeeder
{
    public int Priority => 1;
    
    private readonly IServiceScopeFactory _scopeFactory;

    public ExtrasSeeder(IServiceScopeFactory scopeFactory)
        => _scopeFactory = scopeFactory;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<UspDbContext>();

        if(await context.Extras.AnyAsync(cancellationToken)) return;

        var extras = GetExtras();
        
        context.Extras.AddRange(extras);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static IEnumerable<Extra> GetExtras()
        => new[]
        {
            "Navigation system",
            "Heated seats",
            "Advanced safety features",
            "Premium sound system",
            "Sunroof / moonroof",
            "Navigation system",
            "Air conditioning",
            "Keyless entry",
            "Built-in Wi-Fi",
            "Adaptive Cruise Control",
            "Rearview Camera",
        }
            .Select(s => new Extra { Id = Guid.NewGuid(), Name = s })
            .ToList();
}
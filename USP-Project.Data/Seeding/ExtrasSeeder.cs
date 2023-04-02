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
        
        var extras = Array.Empty<Extra>();
        
        context.Extras.AddRange(extras);
        await context.SaveChangesAsync(cancellationToken);
    }
}
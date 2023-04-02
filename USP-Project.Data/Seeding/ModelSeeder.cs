using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using USP_Project.Data.Models;

namespace USP_Project.Data.Seeding;

internal sealed class ModelSeeder : ISeeder
{
    public int Priority => 2;
    
    private readonly IServiceScopeFactory _scopeFactory;

    public ModelSeeder(IServiceScopeFactory scopeFactory)
        => _scopeFactory = scopeFactory;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<UspDbContext>();

        if(await context.Models.AnyAsync(cancellationToken)) return;
        
        var models = Array.Empty<Model>();
        
        context.Models.AddRange(models);
        await context.SaveChangesAsync(cancellationToken);
    }
}
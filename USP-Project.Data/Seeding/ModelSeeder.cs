using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using USP_Project.Data.Models;

namespace USP_Project.Data.Seeding;

internal sealed class ModelSeeder : ISeeder
{
    public int Priority => 3;

    private readonly IServiceScopeFactory _scopeFactory;

    public ModelSeeder(IServiceScopeFactory scopeFactory)
        => _scopeFactory = scopeFactory;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<UspDbContext>();

        if (await context.Models.AnyAsync(cancellationToken)) return;
        var models = await GetModels(context);

        context.Models.AddRange(models);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task<IEnumerable<Model>> GetModels(UspDbContext context)
    {
        var brands = await context.Brands.ToListAsync();

        var bmw = brands.FirstOrDefault(b => b.Name == "BMW")!;
        var audi = brands.FirstOrDefault(b => b.Name == "Audi")!;
        var toyota = brands.FirstOrDefault(b => b.Name == "Toyota");
        var honda = brands.FirstOrDefault(b => b.Name == "Honda");
        var ferrari = brands.FirstOrDefault(b => b.Name == "Ferrari");
        var lamborghini= brands.FirstOrDefault(b => b.Name == "Lamborghini");
        var astonMartin = brands.FirstOrDefault(b => b.Name == "Aston Martin");

        var bmws = new[] { "i7", "i7", "X5", "X3", "530d" }
            .Select(m => new Model { Brand = bmw, Name = m });
        
        var audis = new[] { "A3", "Q3", "Q5", "Q7", "TT", "R8" }
            .Select(m => new Model { Brand = audi, Name = m });
        
        var toyotas = new[] { "Corolla", "Camry", "RAV4", "Highlander", "Tacoma", "Tundra" }
            .Select(m => new Model { Brand = toyota, Name = m });
        
        var hondas = new[] { "Civic", "Accord", "CR-V", "Pilot", "Odyssey", "RidgeLine", "Insight" }
            .Select(m => new Model { Brand = honda, Name = m });
        
        var ferraris = new[] { "488 GTB", "Portofino", "Roma", "SF90 Stradale", "GTC4Luso" }
            .Select(m => new Model { Brand = ferrari, Name = m });
        
        var lamborghinis = new[] { "Aventador", "Huracan", "Urus", "Diablo", "Gallardo" }
            .Select(m => new Model { Brand = lamborghini, Name = m });
        
        var astonMartins = new[] { "DB11", "Vantage", "Rapide", "DB9", "Vanquish", "Valkyrie" }
            .Select(m => new Model { Brand = astonMartin, Name = m });

        var allModels = bmws
            .Concat(audis)
            .Concat(toyotas)
            .Concat(hondas)
            .Concat(ferraris)
            .Concat(lamborghinis)
            .Concat(astonMartins);
        
        return allModels;
    }

}
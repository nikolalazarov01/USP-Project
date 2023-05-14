using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using USP_Project.Data.Models;
using USP_Project.Data.Models.Enums;

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

        var cars = await GetCars(context);
        var extras = await context.Extras
            .ToListAsync(cancellationToken);
        
        foreach (var car in cars)
        {
            car.CarsExtras = extras
                .Skip(Random.Shared.Next(5))
                .Take(Random.Shared.Next(5))
                .Select(e => new CarsExtras
                {
                    Car = car,
                    Extra = e
                }).ToList();
        }
        
        context.Cars.AddRange(cars);
        await context.SaveChangesAsync(cancellationToken);
    }

    private static async Task<List<Car>> GetCars(UspDbContext context)
    {
        string carImageFilePath = Path.Combine("Images", "sample-car.jpg");
        var models = await context.Models.ToListAsync();
        
        return models.Select(m => new Car
        {
            Model = m,
            BrandId = m.BrandId,
            EngineSize = (decimal?)(Random.Shared.Next(0, 100) * 0.10),
            Engine = Enum.GetValues<EngineType>()[Random.Shared.Next(2)],
            ProductionYear = Random.Shared.Next(1970, 2023),
            CarsExtras = Array.Empty<CarsExtras>().ToList(),
            ImagePaths = new[] { carImageFilePath }
        }).ToList();
    }
}
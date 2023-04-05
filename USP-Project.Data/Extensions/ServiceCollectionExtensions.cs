using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using USP_Project.Data.Contracts;
using USP_Project.Data.Repository;
using USP_Project.Data.Seeding;

namespace USP_Project.Data.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddData(
        this IServiceCollection services,
        string connectionString)
    {
        services
            // .AddScoped<UspDbContext>()
            .AddDbContext<UspDbContext>((sp, options) =>
                options
                    .UseMemoryCache(sp.GetRequiredService<IMemoryCache>())
                    .UseNpgsql(connectionString))
            .AddSeeding()
            .AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services
            .AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = false;
                options.SignIn.RequireConfirmedEmail = false;
            })
            .AddEntityFrameworkStores<UspDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddSeeding(this IServiceCollection services)
    {
        var seederTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(t => t is { IsInterface: false, IsAbstract: false }
                        && typeof(ISeeder).IsAssignableFrom(t))
            .ToList();

        services.AddScoped<CompositeSeeder>();
        foreach (var seederType in seederTypes)
        {
            services.AddScoped(typeof(ISeeder), seederType);
        }

        return services;
    }

    public static async Task SeedAsync(
        this IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        
        var seeder = scope.ServiceProvider.GetRequiredService<CompositeSeeder>();
        var context = scope.ServiceProvider.GetRequiredService<UspDbContext>();
        
        // Adding fuzzy search functionality ... :)
        await context.Database.ExecuteSqlRawAsync(
            @"CREATE EXTENSION IF NOT EXISTS fuzzystrmatch;",
            cancellationToken);
        
        await seeder.SeedAsync(cancellationToken);
    }
}
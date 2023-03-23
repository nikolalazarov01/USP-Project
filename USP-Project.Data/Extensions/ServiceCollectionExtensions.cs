using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using USP_Project.Data.Contracts;
using USP_Project.Data.Repository;

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
    
    // TODO: Potentially add Seeding and Repository components next ...
}
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace USP_Project.Data.Extensions;

public static class DatabaseExtensions
{
    public static void SetupDatabase([NotNull] this IServiceCollection services, string connectionString)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        services.AddDbContext<DbContext, UspDbContext>(options => options.UseNpgsql(connectionString));
    }
}
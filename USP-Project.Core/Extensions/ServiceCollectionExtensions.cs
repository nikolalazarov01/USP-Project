using Microsoft.Extensions.DependencyInjection;
using USP_Project.Core.Contracts;
using USP_Project.Core.Services;

namespace USP_Project.Core.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
        => services.AddScoped<ICarsService, CarsService>();
}
using USP_Project.Data.Contracts;

namespace USP_Project.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        services
            .AddAuthentication()
            .AddGoogle(options =>
            {
                var clientId = configuration.GetSection("Google")[nameof(options.ClientId)];
                var clientSecret = configuration.GetSection("Google")[nameof(options.ClientSecret)];

                options.ClientId = clientId;
                options.ClientSecret = clientSecret;
            })
            .AddFacebook(options =>
            {
                options.AppId = configuration.GetSection("Facebook")[nameof(options.AppId)];
                options.AppSecret = configuration.GetSection("Facebook")[nameof(options.AppSecret)];
            });
        
        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        return services;
    }
    
}
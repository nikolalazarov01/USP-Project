using USP_Project.Data.Contracts;
using USP_Project.Data.Repository;
using USP_Project.Web.Settings;

namespace USP_Project.Web.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var googleOptions = configuration.Get<GoogleOptions>(GoogleOptions.ConfigSection);
        var facebookOptions = configuration.Get<FacebookOptions>(FacebookOptions.ConfigSection);

        services
            .AddAuthentication()
            .AddGoogle(options =>
            {
                options.ClientId = googleOptions.ClientId;
                options.ClientSecret = googleOptions.ClientSecret;
            })
            .AddFacebook(options =>
            {
                options.AppId = facebookOptions.AppId;
                options.AppSecret = facebookOptions.AppSecret;
            });
        
        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        if (services is null) throw new ArgumentNullException(nameof(services));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
        return services;
    }

    private static TSettings Get<TSettings>(
        this IConfiguration configuration,
        string? sectionName = default)
        where TSettings : new()
    {
        var settings = new TSettings();
        configuration.Bind(sectionName ?? typeof(TSettings).Name, settings);

        return settings;
    }
    
}
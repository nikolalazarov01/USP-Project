using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;

namespace USP_Project.Web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCachedStaticFiles(
        this IApplicationBuilder app,
        IWebHostEnvironment environment,
        string path = "")
        => app.UseStaticFiles(new StaticFileOptions
        {
            HttpsCompression = HttpsCompressionMode.Compress,
            FileProvider = new CompositeFileProvider(environment.WebRootFileProvider),
            RequestPath = path,
            ContentTypeProvider = new FileExtensionContentTypeProvider(),
            OnPrepareResponse = ctx =>
            {
                var logger = ctx.Context.RequestServices.GetRequiredService<ILogger<StaticFileOptions>>();
                logger.LogInformation("[{Path}] Requesting a {Type} resource",
                    ctx.Context.Request.Path.Value,
                    ctx.Context.Request.ContentType);
                
                var headers = ctx.Context.Response.GetTypedHeaders();
                headers.CacheControl = new CacheControlHeaderValue
                {
                    MaxAge = TimeSpan.FromDays(30),
                    Public = true
                };
                headers.Expires = DateTimeOffset.Now.AddDays(30);
            }
        });

}
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;

namespace USP_Project.Web.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseCachedStaticFiles(
        this IApplicationBuilder app,
        IWebHostEnvironment environment)
        => app.UseStaticFiles(new StaticFileOptions
        {
            HttpsCompression = HttpsCompressionMode.Compress,
            FileProvider = new CompositeFileProvider(environment.WebRootFileProvider),
            RequestPath = "/static",
            ContentTypeProvider = new FileExtensionContentTypeProvider(),
            OnPrepareResponse = ctx =>
            {
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
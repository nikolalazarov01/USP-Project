using Microsoft.AspNetCore.Identity.UI.Services;
using USP_Project.Core.Extensions;
using USP_Project.Data.Extensions;
using USP_Project.Web;
using USP_Project.Web.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetSection("Database")["ConnectionString"];

builder.Services
    .AddData(connectionString)
    .AddCore()
    .AddApplicationAuthentication(builder.Configuration)
    //.ConfigureServices()
    .AddScoped<IEmailSender, NullMailSender>()
    .AddControllersWithViews();

builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error")
        .UseHsts();
}

app.UseHttpsRedirection()
    .UseStaticFiles()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
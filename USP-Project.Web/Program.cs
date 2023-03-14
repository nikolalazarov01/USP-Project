using Microsoft.AspNetCore.Identity.UI.Services;
using USP_Project.Data.Extensions;
using USP_Project.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetSection("Database")["ConnectionString"];

builder.Services
    .AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration.GetSection("Google")[nameof(options.ClientId)];
        options.ClientSecret = builder.Configuration.GetSection("Google")[nameof(options.ClientSecret)];
    })
    .AddFacebook(options =>
    {
        options.AppId = builder.Configuration.GetSection("Facebook")[nameof(options.AppId)];
        options.AppSecret = builder.Configuration.GetSection("Facebook")[nameof(options.AppSecret)];
    });
    
builder.Services
    .AddData(connectionString)
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
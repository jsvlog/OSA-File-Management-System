using OSAWebAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication("Cookies")
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddScoped<RegionComService>();
builder.Services.AddScoped<InventoryService>();
builder.Services.AddScoped<MonitoringService>();
builder.Services.AddScoped<AuthService>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5000);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var authService = scope.ServiceProvider.GetRequiredService<AuthService>();
    try
    {
        authService.EnsureUsersTableExists();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Could not ensure users table exists: {ex.Message}");
    }

    var monitoringService = scope.ServiceProvider.GetRequiredService<MonitoringService>();
    try
    {
        monitoringService.EnsureTableExists();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Warning: Could not ensure monitoring_submissions table exists: {ex.Message}");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
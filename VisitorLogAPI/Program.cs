using VisitorLogAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<VisitorLogService>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5002);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Visitors}/{action=Authenticate}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var visitorService = scope.ServiceProvider.GetRequiredService<VisitorLogService>();
    visitorService.EnsureTableExists();
}

app.Run();

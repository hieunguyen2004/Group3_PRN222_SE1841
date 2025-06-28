using DAO.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var log4NetConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "log4net.config");

builder.Logging.ClearProviders();

builder.Logging.AddLog4Net(log4NetConfigPath);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

var startupLogger = app.Services.GetRequiredService<ILogger<Program>>();

startupLogger.LogInformation($"TopCVWeb application is starting in environment: {app.Environment.EnvironmentName}");
startupLogger.LogDebug("Check debug message");
startupLogger.LogCritical("Check critical message");
startupLogger.LogWarning("Check Warning message");
startupLogger.LogError("Check error message");


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
    startupLogger.LogInformation("Exception Handler and HSTS enabled for Production environment.");
}
else
{
    startupLogger.LogInformation("Application is running in Development environment.");
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

startupLogger.LogInformation("TopCVWeb application has shut down.");
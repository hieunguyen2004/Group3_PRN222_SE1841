using DAO.Models;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Interface;
using Service;
using Service.Interface;

var builder = WebApplication.CreateBuilder(args);

var log4NetConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "log4net.config");

builder.Logging.ClearProviders();

builder.Logging.AddLog4Net(log4NetConfigPath);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICVRepository, CVRepository>();
builder.Services.AddScoped<ICVService, CVService>();




var app = builder.Build();

var startupLogger = app.Services.GetRequiredService<ILogger<Program>>();

startupLogger.LogInformation($"TopCVWeb application is starting in environment: {app.Environment.EnvironmentName}");
startupLogger.LogDebug("Check debug message");
startupLogger.LogCritical("Check critical message");
startupLogger.LogWarning("Check Warning message");
startupLogger.LogError("Check error message");

try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();
        if (dbContext.Database.CanConnect())
        {
            startupLogger.LogInformation("Database connection successful. Database ready.");
        }
        else
        {
            startupLogger.LogCritical("CANNOT CONNECT TO DATABASE! Please check the connection string and database status.");
        }
    }
}
catch (Exception ex)
{
    startupLogger.LogCritical(ex, "An error occurred while checking database connection: {Message}", ex.Message);
}

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
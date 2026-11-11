using NLog;
using NLog.Web;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

// Add services to the container.
try
{
    builder.Services.AddControllers();

    // Перенесли регистрацию контекста СЮДА (до builder.Build())
    builder.Services.AddDbContext<WebApplication1.Database.StudentDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

    // Learn more about configuring OpenAPI at https://aka.ms
    builder.Services.AddOpenApi();

    var app = builder.Build(); // Сборка приложения теперь строго после сервисов

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}

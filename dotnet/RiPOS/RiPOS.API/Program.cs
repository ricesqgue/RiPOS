using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using RiPOS.API.Utilities.ActionFilters;
using RiPOS.API.Utilities.Extensions;
using RiPOS.API.Utilities.Middleware;
using RiPOS.Database;

var logger = LogManager.Setup().LoadConfigurationFromFile("NLog.config").GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseNLog();

    builder.Services.ConfigureJwtAuthentication(builder.Configuration);

    builder.Services.ConfigureCors();

    builder.Services.ConfigureSwagger();

    builder.Services.AddScoped<ModelValidationAttribute>();

    builder.Services.AddControllers();

    builder.Services.ConfigureCoreServices();

    builder.Services.ConfigureRepositories();

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddDbContext<RiPOSDbContext>(d => d.UseSqlServer("name=ConnectionStrings:RiPOS"));

    builder.Services.AddHttpContextAccessor();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors("CorsPolicy");

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.UseMiddleware<ExceptionMiddleware>();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}



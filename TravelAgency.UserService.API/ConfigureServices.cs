using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

namespace TravelAgency.UserService.API;

public static class ConfigureServices
{
    private const string TimePattern = @"^([0-1][0-9]|2[0-3]):[0-5][0-9]$";

    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File("logs/logs-.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

        return services;
    }

    public static IServiceCollection AddAndConfigureSwagger(this IServiceCollection services, string serviceName, string openIdConnectUrl)
    {
        return services.AddSwaggerGen(
            c =>
            {
                c.CustomSchemaIds(t => t.FullName!.Replace('+', '_'));
                c.SwaggerDoc("v1", new OpenApiInfo { Title = serviceName, Version = "v1" });

                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        BearerFormat = "jwt",
                        Description = "Please enter your access token.",
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer"
                    });

                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>("Bearer");
                c.EnableAnnotations();

                c.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
                c.MapType<TimeOnly>(() => new OpenApiSchema { Type = "string", Pattern = TimePattern });
                c.MapType<TimeSpan>(() => new OpenApiSchema { Type = "string", Pattern = TimePattern });
            });
    }
}

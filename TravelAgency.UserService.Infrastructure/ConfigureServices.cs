using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Models;
using TravelAgency.UserService.Infrastructure.Persistance;
using TravelAgency.UserService.Infrastructure.Persistance.Interceptors;
using TravelAgency.UserService.Infrastructure.Repositories;
using TravelAgency.UserService.Infrastructure.Services;

namespace TravelAgency.UserService.Infrastructure;
public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionStringDto = configuration.GetRequiredSection("Database").Get<DatabaseDto>();
        var connectionString = BuildConnectionString(connectionStringDto);

        services.AddDbContext<UserServiceDbContext>(options =>
                options.UseSqlServer(connectionString, builder => builder.MigrationsAssembly(typeof(UserServiceDbContext).Assembly.FullName)));

        services.AddScoped<UserServiceDbContextInitialiser>();
        services.AddScoped<IUserServiceDbContext, UserServiceDbContext>();
        services.AddScoped<BaseAuditableEntitySaveChangesInterceptor>();

        services.RegisterRepositories();
        services.RegisterServices();

        return services;
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();

        return services;
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeService, DateTimeService>();

        return services;
    }


    private static string BuildConnectionString(DatabaseDto? databaseDto)
    {
        if (databaseDto is null)
        {
            throw new InvalidOperationException("Database section cannot be found. Please check your secret manager.");
        }

        var sqlStringBuilder = new SqlConnectionStringBuilder();

        sqlStringBuilder.UserID = databaseDto.UserId;
        sqlStringBuilder.Password = databaseDto.Password;
        sqlStringBuilder.TrustServerCertificate = databaseDto.TrustServerCertificate;
        sqlStringBuilder.DataSource = databaseDto.DataSource;
        sqlStringBuilder.InitialCatalog = databaseDto.InitialCatalog;

        return sqlStringBuilder.ConnectionString;
    }
}

using Amazon;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelAgency.CommonLibrary.AWS;
using TravelAgency.CommonLibrary.Models;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Infrastructure.Persistance;
using TravelAgency.UserService.Infrastructure.Persistance.Interceptors;
using TravelAgency.UserService.Infrastructure.Repositories;
using TravelAgency.UserService.Infrastructure.Services;

namespace TravelAgency.UserService.Infrastructure;
public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var awsEnv = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
        string dockerNamingConvention = string.Empty;

        if (awsEnv is not null)
        {
            dockerNamingConvention = "Docker_";
        }

        builder.Configuration.AddAndConfigureSecretManager(builder.Environment, RegionEndpoint.EUNorth1, dockerNamingConvention);

        var connectionString = builder.Configuration.GetConnectionString("UserServiceDatabase");

        services.AddDbContext<UserServiceDbContext>(options =>
                options.UseSqlServer(connectionString, builder => builder.MigrationsAssembly(typeof(UserServiceDbContext).Assembly.FullName)));

        services.AddScoped<UserServiceDbContextInitialiser>();
        services.AddScoped<IUserServiceDbContext, UserServiceDbContext>();
        services.AddScoped<BaseAuditableEntitySaveChangesInterceptor>();

        services.RegisterRepositories();
        services.RegisterServices();

        services.Configure<AwsCognitoSettingsDto>(builder.Configuration.GetRequiredSection("AWS:Cognito"));

        var cognitoConfiguration = builder.Configuration.GetRequiredSection("AWS:Cognito").Get<AwsCognitoSettingsDto>()!;

        services.AddAuthenticationAndJwtConfiguration(cognitoConfiguration);
        services.AddAuthorizationWithPolicies();

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
        services.AddScoped<IAmazonCognitoService, AmazonCognitoService>();  

        return services;
    }
}

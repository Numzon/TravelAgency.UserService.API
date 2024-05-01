using Amazon;
using Amazon.CognitoIdentityProvider;
using LinqKit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Data.Common;
using TravelAgency.SharedLibrary.AWS;
using TravelAgency.SharedLibrary.Models;
using TravelAgency.SharedLibrary.RabbitMQ;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Models;
using TravelAgency.UserService.Infrastructure.Persistance;
using TravelAgency.UserService.Infrastructure.Persistance.Interceptors;
using TravelAgency.UserService.Infrastructure.Publishers;
using TravelAgency.UserService.Infrastructure.Repositories;
using TravelAgency.UserService.Infrastructure.Services;

namespace TravelAgency.UserService.Infrastructure;
public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, WebApplicationBuilder builder)
    {
        builder.Configuration.AddAndConfigureSecretManager(builder.Environment, RegionEndpoint.EUNorth1);

        var connectionString = builder.Configuration.GetConnectionString("UserServiceDatabase");
        if (string.IsNullOrEmpty(connectionString))
        {
            connectionString = builder.BuildConnectionStringFromUserSecrets();
        }

        services.AddDbContext<UserServiceDbContext>(options =>
                options.UseSqlServer(connectionString, builder => builder.MigrationsAssembly(typeof(UserServiceDbContext).Assembly.FullName)));

        services.AddScoped<UserServiceDbContextInitialiser>();
        services.AddScoped<IUserServiceDbContext, UserServiceDbContext>();
        services.AddScoped<BaseAuditableEntitySaveChangesInterceptor>();
        services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        services.RegisterRepositories();
        services.RegisterServices();
        services.RegisterPublishers();

        services.Configure<AwsCognitoSettingsDto>(builder.Configuration.GetRequiredSection("AWS:Cognito"));
        services.Configure<AmazonEmailServiceSettingsDto>(builder.Configuration.GetRequiredSection("AWS:SimpleEmailService"));
        services.Configure<AmazonNotificationServiceSettingsDto>(builder.Configuration.GetRequiredSection("AWS:SimpleNotificationService"));
        services.Configure<RabbitMqSettingsDto>(builder.Configuration.GetRequiredSection("RabbitMQ"));

        var cognitoConfiguration = builder.Configuration.GetRequiredSection("AWS:Cognito").Get<AwsCognitoSettingsDto>()!;

        var cognitoClient = new AmazonCognitoIdentityProviderClient(RegionEndpoint.GetBySystemName(cognitoConfiguration.Region));
        services.AddSingleton<IAmazonCognitoIdentityProvider>(cognitoClient);
            
        try
        {
            services.AddAuthenticationAndJwtConfiguration(cognitoConfiguration);
        }
        catch (Exception ex)
        {
            if (!builder.Environment.IsDevelopment())
            {
                Log.Error(ex.Message);
            }
        }

        var rabbitMqSettings = builder.Configuration.GetRequiredSection("RabbitMQ").Get<RabbitMqSettingsDto>()!;

        services.AddRabbitMqConfiguration(rabbitMqSettings);

        services.AddAuthorizationWithPolicies();

        return services;
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services.AddScoped<INotificationTypeRepository, NotificationTypeRepository>();
        services.AddScoped<IClientAccountRepository, ClientAccountRepository>();

        return services;
    }

    private static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeService, DateTimeService>();
        services.AddScoped<IAmazonCognitoService, AmazonCognitoService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IAmazonEmailService, AmazonEmailService>();
        services.AddScoped<IAmazonNotificationService, AmazonNotificationService>();

        return services;
    }

    private static IServiceCollection RegisterPublishers(this IServiceCollection services)
    {
        services.AddScoped<ITravelAgencyPublisher, TravelAgencyPublisher>();
        services.AddScoped<IManagerPublisher, ManagerPublisher>();
        services.AddScoped<IEmployeePublisher, EmployeePublisher>();

        return services;
    }

    private static string BuildConnectionStringFromUserSecrets(this WebApplicationBuilder builder)
    {
        var connectionStringBuilder = new DbConnectionStringBuilder();

        builder.Configuration
             .GetRequiredSection("Database")
             .GetChildren()
             .Where(x => !string.IsNullOrWhiteSpace(x.Value))
             .ForEach(x => connectionStringBuilder.Add(x.Key, x.Value!));

        return connectionStringBuilder.ToString();
    }
}

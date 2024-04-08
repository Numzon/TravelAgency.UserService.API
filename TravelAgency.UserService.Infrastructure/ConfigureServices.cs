using Amazon;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.Common.Models;
using TravelAgency.UserService.Domain.Enums;
using TravelAgency.UserService.Infrastructure.Persistance;
using TravelAgency.UserService.Infrastructure.Persistance.Interceptors;
using TravelAgency.UserService.Infrastructure.Repositories;
using TravelAgency.UserService.Infrastructure.Services;

namespace TravelAgency.UserService.Infrastructure;
public static class ConfigureServices
{
    public async static Task<IServiceCollection> AddInfrastructureServices(this IServiceCollection services, WebApplicationBuilder builder)
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

        await services.AddAuthenticationAndJwtTokenConfiguration(cognitoConfiguration);

        services.AddAuthenticationAndJwtTokenConfiguration();

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

    private static IConfigurationBuilder AddAndConfigureSecretManager(this IConfigurationBuilder configuration, IWebHostEnvironment environment, RegionEndpoint region, string? prefix = null)
    {
        configuration.AddSecretsManager(
           region: region,
           configurator: o =>
           {
               string secretPrefix = $"{prefix ?? string.Empty}{environment.EnvironmentName}_{environment.ApplicationName.Replace(".", "_")}_";
               o.SecretFilter = s => s.Name
                   .StartsWith(secretPrefix, StringComparison.OrdinalIgnoreCase);

               o.KeyGenerator = (_, s) => s
                   .Replace(secretPrefix, string.Empty, StringComparison.OrdinalIgnoreCase)
                   .Replace("__", ":", StringComparison.Ordinal);
           });

        return configuration;
    }

    private static async Task<IServiceCollection> AddAuthenticationAndJwtTokenConfiguration(this IServiceCollection services, AwsCognitoSettingsDto settings)
    {
        using var httpClient = new HttpClient();

        var cognitoSigningKeys = new JsonWebKeySet(await httpClient.GetStringAsync(settings.JwtKeysUrl))
            .GetSigningKeys();

        services
            .AddAuthentication(
                o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(
                o =>
                {
                    o.Authority = settings.AuthorityUrl;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        AudienceValidator = (_, token, parameters) => parameters.ValidAudience
                            == (token as JwtSecurityToken)?.Claims.SingleOrDefault(c => c.Type == AwsTokenNames.ClientId)?.Value,
                        IssuerSigningKeys = cognitoSigningKeys,
                        ValidIssuer = settings.AuthorityUrl,
                        ValidAudience = settings.ClientId,
                        ValidAlgorithms = new[] { "RS256" },
                        ValidateIssuerSigningKey = true
                    };
                });

        return services;
    }


    private static IServiceCollection AddAuthenticationAndJwtTokenConfiguration(this IServiceCollection services)
    {
        services
            .AddAuthorization(
                o =>
                {
                    o.AddPolicy(
                        PolicyNames.ClientPolicy,
                        p => p.RequireClaim(AwsTokenNames.Groups, CognitoGroups.ClientAccount));

                    o.AddPolicy(
                        PolicyNames.TravelAgencyPolicy,
                        p => p.RequireClaim(AwsTokenNames.Groups, CognitoGroups.TravelAgencyAccount));

                    o.AddPolicy(
                        PolicyNames.EmployeePolicy,
                        p => p.RequireClaim(AwsTokenNames.Groups, CognitoGroups.Employee));

                    o.AddPolicy(
                       PolicyNames.TravelManagerPolicy,
                       p => p.RequireClaim(AwsTokenNames.Groups, CognitoGroups.Employee, CognitoGroups.TravelManager));

                    o.AddPolicy(
                       PolicyNames.FleetManagerPolicy,
                       p => p.RequireClaim(AwsTokenNames.Groups, CognitoGroups.Employee, CognitoGroups.FleetManager));

                    o.AddPolicy(
                       PolicyNames.HumanResourceManagerPolicy,
                       p => p.RequireClaim(AwsTokenNames.Groups, CognitoGroups.Employee, CognitoGroups.HumanResourcesManager));

                    o.AddPolicy(
                       PolicyNames.FinancialResourceManagerPolicy,
                       p => p.RequireClaim(AwsTokenNames.Groups, CognitoGroups.Employee, CognitoGroups.FinancialManager));
                });

        return services;
    }
}

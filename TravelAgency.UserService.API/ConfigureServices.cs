﻿using Serilog;

namespace TravelAgency.UserService.API;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Information()
        .WriteTo.File("logs/logs-.txt", rollingInterval: RollingInterval.Day)
        .CreateLogger();

        return services;
    }
}

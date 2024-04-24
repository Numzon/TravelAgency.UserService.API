using Serilog;
using System.Reflection;
using TravelAgency.SharedLibrary.Swagger;
using TravelAgency.UserService.Application;
using TravelAgency.UserService.Infrastructure;
using TravelAgency.UserService.Infrastructure.Persistance;

namespace TravelAgency.UserService.API;

public class Program
{
    protected Program()
    {

    }

    protected static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder);
        builder.Services.AddApiServices();

        builder.Host.UseSerilog();

        Assembly assembly = typeof(Program).Assembly;

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddAndConfigureSwagger(assembly.GetName().Name!);

            builder.Configuration.AddUserSecrets(assembly);
        }

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger()
               .UseSwaggerUI();

            using (var scope = app.Services.CreateScope())
            {
                var initialiser = scope.ServiceProvider.GetRequiredService<UserServiceDbContextInitialiser>();
                await initialiser.InitialiseAsync();
                await initialiser.SeedAsync();
            }
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
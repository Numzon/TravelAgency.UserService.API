using Serilog;
using System.Reflection;
using TravelAgency.SharedLibrary.Swagger;
using TravelAgency.UserService.API;
using TravelAgency.UserService.Application;
using TravelAgency.UserService.Infrastructure;
using TravelAgency.UserService.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

if (builder.Environment.IsProduction())
{
    builder.Configuration.AddJsonFile("secrets/appsettings.Production.json", optional: true);
}

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


#pragma warning disable S1118 // Utility classes should not have public constructors
public partial class Program { }
#pragma warning restore S1118 // Utility classes should not have public constructors
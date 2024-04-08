using Serilog;
using System.Reflection;
using TravelAgency.UserService.API;
using TravelAgency.UserService.Application;
using TravelAgency.UserService.Application.Common.Models;
using TravelAgency.UserService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApplicationServices();
await builder.Services.AddInfrastructureServices(builder);
builder.Services.AddApiServices();

builder.Host.UseSerilog();

Assembly assembly = typeof(Program).Assembly;

var cognitoConfiguration = builder.Configuration
    .GetRequiredSection("AWS:Cognito")
    .Get<AwsCognitoSettingsDto>()!;

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddAndConfigureSwagger(assembly.GetName().Name!,
            cognitoConfiguration.AuthorityDiscoveryUrl);
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger()
       .UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TravelAgency.UserService.Infrastructure.Persistance;

namespace TravelAgency.UserService.Tests.Shared.Configurations;

public abstract class BaseIntegrationTest : IDisposable
{
    protected TestServer TestServer { get; set; } = null!;
    protected Fixture _fixture;

    private Respawner _respawner = null!;


    protected BaseIntegrationTest()
    {
        _fixture = new Fixture();

        _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
         .ToList().ForEach(behavior => _fixture.Behaviors.Remove(behavior));
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    protected async Task InitializeDatabaseAsync()
    {
        IServiceProvider serviceProvider = TestServer.Services;

        using IServiceScope scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetService<UserServiceDbContext>()!;

        await context.Database.MigrateAsync();

        var dbConnection = context.Database.GetDbConnection();

        await dbConnection.OpenAsync();

        _respawner = await Respawner.CreateAsync(context.Database.GetDbConnection(), new RespawnerOptions
        {
            TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" },
            DbAdapter = DbAdapter.SqlServer
        });

        await dbConnection.CloseAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        using IServiceScope scope = TestServer.Services.CreateScope();
        var context = scope.ServiceProvider.GetService<UserServiceDbContext>()!;

        var dbConnection = context.Database.GetDbConnection();

        await dbConnection.OpenAsync();
        await _respawner.ResetAsync(dbConnection);
        await dbConnection.CloseAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);

    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            TestServer.Dispose();
        }
    }
}
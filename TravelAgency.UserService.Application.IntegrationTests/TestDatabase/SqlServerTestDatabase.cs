using MediatR;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Respawn;
using System.Data.Common;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.IntegrationTests.TestDatabase.Interface;
using TravelAgency.UserService.Infrastructure.Persistance;
using TravelAgency.UserService.Infrastructure.Persistance.Interceptors;
using Xunit.Sdk;

namespace TravelAgency.UserService.Application.IntegrationTests.TestDatabase;
public sealed class SqlServerTestDatabase : ITestDatabase, IAsyncDisposable
{
    private readonly string AppsettingsFileName = "appsettings.Tests.json";
    private readonly string DatabaseName = "UserServiceTestDatabase";

    private readonly string _connectionString;
    private SqlConnection _connection = null!;
    private Respawner _respawner = null!;

    public SqlServerTestDatabase()
    {
        var configuration = new ConfigurationBuilder()
          .SetBasePath(GetAppsettingsTestsPath())
          .AddJsonFile(AppsettingsFileName)
          .AddEnvironmentVariables()
          .Build();

        var connectionString = configuration.GetConnectionString(DatabaseName);

        _connectionString = connectionString ?? throw new NullException("ConnectionString is null");
    }

    public async ValueTask DisposeAsync()
    {
        await _connection.DisposeAsync();
    }

    public DbConnection GetConnection()
    {
        return _connection;
    }

    public async Task InitialiseAsync()
    {
        _connection = new SqlConnection(_connectionString);

        var options = new DbContextOptionsBuilder<UserServiceDbContext>().UseSqlServer(_connectionString).Options;

        var dateTimeService = Mock.Of<IDateTimeService>();
        var mediatr = Mock.Of<IMediator>();
        var currentUserService = Mock.Of<ICurrentUserService>();

        var interceptor = new BaseAuditableEntitySaveChangesInterceptor(currentUserService, dateTimeService);

        var context = new UserServiceDbContext(options, mediatr, interceptor);

        context.Database.Migrate();

        _respawner = await Respawner.CreateAsync(_connectionString, new RespawnerOptions
        {
            TablesToIgnore = new Respawn.Graph.Table[] { "__EFMigrationsHistory" }
        });
    }

    public async Task ResetAsync()
    {
        await _respawner.ResetAsync(_connectionString);
    }


    private string GetAppsettingsTestsPath()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory is not null && !directory.GetFiles(AppsettingsFileName).Any())
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? throw new NullException("Full name is null");
    }
}

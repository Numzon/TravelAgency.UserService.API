using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using TravelAgency.UserService.Application.IntegrationTests.TestDatabase;
using TravelAgency.UserService.Application.IntegrationTests.TestDatabase.Interface;
using TravelAgency.UserService.Domain.Common;
using TravelAgency.UserService.Infrastructure.Persistance;

namespace TravelAgency.UserService.API.IntegrationTests;

public class BaseSqlServerDbTest<TEntity> //: IAsyncLifetime
    where TEntity : BaseEntity
{
    private static ITestDatabase _database = default!;
    private static CustomWebApplicationFactory _factory = default!;
    private static IServiceScopeFactory _scopeFactory = default!;

    protected readonly Fixture _fixture;

    protected BaseSqlServerDbTest()
    {
        _fixture = new Fixture();
    }

    public async static Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        return await mediator.Send(request);
    }

    public async static Task SendAsync(IBaseRequest request)
    {
        using var scope = _scopeFactory.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<ISender>();

        await mediator.Send(request);
    }

    public async static Task ResetState()
    {
        await _database.ResetAsync();
    }

    public async static Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();

        return await context.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    public async static Task<IEnumerable<TEntity>> ToListAsync(Expression<Func<TEntity, bool>>? predicate = null)
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();

        if (predicate is not null)
        {
            return await context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        return await context.Set<TEntity>().ToListAsync();
    }

    public async static Task<TEntity> CreateAsync(TEntity entity)
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();

        await context.Set<TEntity>().AddAsync(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public async static Task<IEnumerable<TEntity>> CreateRangeAsync(IEnumerable<TEntity> entities)
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();

        await context.Set<TEntity>().AddRangeAsync(entities);
        await context.SaveChangesAsync();

        return entities;
    }

    public async static Task<int> CountAsync()
    {
        using var scope = _scopeFactory.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<UserServiceDbContext>();

        return await context.Set<TEntity>().CountAsync();
    }

    public async static Task InitializeAsync()
    {
        _database = await TestDatabaseFactory.CreateAsync();

        _factory = new CustomWebApplicationFactory(_database.GetConnection());

        _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();

        await _database.ResetAsync();
    }

    public static async Task DisposeAsync()
    {
        await _database.DisposeAsync();
        await _factory.DisposeAsync();
    }
}
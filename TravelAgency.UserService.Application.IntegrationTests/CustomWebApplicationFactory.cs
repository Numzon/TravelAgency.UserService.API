using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Data.Common;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Infrastructure.Persistance;

namespace TravelAgency.UserService.API.IntegrationTests;
public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly DbConnection _connection;
    private readonly Fixture _fixture;

    public CustomWebApplicationFactory(DbConnection connection)
    {
        _connection = connection;
        _fixture = new Fixture();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services
                .RemoveAll<ICurrentUserService>()
                .AddScoped(provider => Mock.Of<ICurrentUserService>(s => s.Id == _fixture.Create<string>()));

            services
                .RemoveAll<DbContextOptions<UserServiceDbContext>>()
                .AddDbContext<UserServiceDbContext>((sp, options) =>
                {
                    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                    options.UseSqlServer(_connection);
                });
        });
    }
}

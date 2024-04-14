using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using TravelAgency.UserService.Infrastructure.Persistance;

namespace TravelAgency.UserService.SharedTestLibrary.TestServers;
public sealed class UserServiceTestServer : TestServer
{
    public UserServiceTestServer(IWebHostBuilder builder) : base(builder)
    {
        DbContext = Host.Services.GetRequiredService<UserServiceDbContext>();
    }

    public UserServiceDbContext DbContext { get; set; }
}

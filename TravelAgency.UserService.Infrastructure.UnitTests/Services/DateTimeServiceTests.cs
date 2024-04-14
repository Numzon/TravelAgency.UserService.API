using TravelAgency.UserService.Infrastructure.Services;

namespace TravelAgency.UserService.Infrastructure.IntegrationTests.Services;
public sealed class DateTimeServiceTests
{
    [Fact]
    public void Now_ValidDate()
    {
        var dateTimeService = new DateTimeService();

        var date = DateTime.Now;
        var dateTimeNow = dateTimeService.Now;


        dateTimeNow.Should().BeSameDateAs(date);
    }
}

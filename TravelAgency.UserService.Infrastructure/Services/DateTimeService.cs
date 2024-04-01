using TravelAgency.UserService.Application.Common.Interfaces;

namespace TravelAgency.UserService.Infrastructure.Services;
public sealed class DateTimeService : IDateTimeService
{
    public DateTime Now  => DateTime.Now;
}

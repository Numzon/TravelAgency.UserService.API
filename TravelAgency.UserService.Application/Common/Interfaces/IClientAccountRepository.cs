using TravelAgency.UserService.Domain.Entities;
using TravelAgency.UserService.Domain.Events;

namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface IClientAccountRepository
{
    Task CreateAsync(ClientUserCreatedEvent notification, CancellationToken cancellationToken);
    Task<ClientAccount?> GetByUserId(string userId, CancellationToken cancellationToken);
    Task<bool> DoesClientAccountForGivenUserExist(string userId, CancellationToken cancellationToken);
}

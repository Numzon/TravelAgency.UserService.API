using TravelAgency.UserService.Application.Common.Models;
using TravelAgency.UserService.Application.NotificationTypes.Commands.CreateNotificationType;
using TravelAgency.UserService.Application.NotificationTypes.Commands.UpdateNotificationType;
using TravelAgency.UserService.Application.NotificationTypes.Queries.GetNotificationTypesQuery;
using TravelAgency.UserService.Domain.Entities;

namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface INotificationTypeRepository
{
    Task<ICollection<NotificationType>> GetByFitlersAsync(GetNotificationTypesQuery query, CancellationToken cancellationToken);
    Task<NotificationType?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<NotificationType> CreateAsync(CreateNotificationTypeCommand command, CancellationToken cancellationToken);
    Task UpdateAsync(NotificationType type, CancellationToken cancellationToken);
    Task DeleteAsync(NotificationType type, CancellationToken cancellationToken);
}

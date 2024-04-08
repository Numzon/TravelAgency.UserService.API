using Microsoft.EntityFrameworkCore;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Application.NotificationTypes.Commands.CreateNotificationType;
using TravelAgency.UserService.Application.NotificationTypes.Queries.GetNotificationTypesQuery;
using TravelAgency.UserService.Domain.Entities;

namespace TravelAgency.UserService.Infrastructure.Repositories;
public class NotificationTypeRepository : INotificationTypeRepository
{
    private readonly IUserServiceDbContext _context;

    public NotificationTypeRepository(IUserServiceDbContext context)
    {
        _context = context;
    }

    public async Task<NotificationType> CreateAsync(CreateNotificationTypeCommand command, CancellationToken cancellationToken)
    {
        var type = new NotificationType { Name = command.Name };

        await _context.NotificationTypes.AddAsync(type, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return type;
    }

    public async Task<ICollection<NotificationType>> GetByFitlersAsync(GetNotificationTypesQuery query, CancellationToken cancellationToken)
    {
        var result = await _context.NotificationTypes
            .Where(x => query.SearchString == null || x.Name.Contains(query.SearchString))
            .ToListAsync(cancellationToken);

        return result;
    }

    public async Task<NotificationType?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var result = await _context.NotificationTypes
            .Where(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

        return result;
    }

    public async Task UpdateAsync(NotificationType type, CancellationToken cancellationToken)
    {
        _context.NotificationTypes.Entry(type);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(NotificationType type, CancellationToken cancellationToken)
    {
        _context.NotificationTypes.Remove(type);
        await _context.SaveChangesAsync(cancellationToken);
    }
}

using Microsoft.EntityFrameworkCore;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Domain.Entities;
using TravelAgency.UserService.Domain.Events;

namespace TravelAgency.UserService.Infrastructure.Repositories;
public sealed class ClientAccountRepository : IClientAccountRepository
{
    private readonly IUserServiceDbContext _context;

    public ClientAccountRepository(IUserServiceDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(ClientUserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var clientAccount = new ClientAccount { UserId = notification.UserId };

        await _context.ClientAccount.AddAsync(clientAccount, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);  
    }

    public async Task<ClientAccount?> GetByUserId(string userId, CancellationToken cancellationToken)
    {
        var clientAccount = await _context.ClientAccount.FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);

        return clientAccount;
    }

    public async Task<bool> DoesClientAccountForGivenUserExist(string userId, CancellationToken cancellationToken)
    {
        return await _context.ClientAccount.AnyAsync(x => x.UserId == userId, cancellationToken);
    }
}

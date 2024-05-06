using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelAgency.UserService.Application.Common.Interfaces;
using TravelAgency.UserService.Domain.Entities;
using TravelAgency.UserService.Domain.Events;

namespace TravelAgency.UserService.Infrastructure.Repositories;
public sealed class ClientAccountRepository : IClientAccountRepository
{
    private readonly IUserServiceDbContext _context;
    private readonly IMapper _mapper;

    public ClientAccountRepository(IUserServiceDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task CreateAsync(ClientUserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var clientAccount = _mapper.Map<ClientAccount>(notification);   

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

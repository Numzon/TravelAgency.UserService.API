﻿using Microsoft.EntityFrameworkCore;
using TravelAgency.UserService.Domain.Entities;


namespace TravelAgency.UserService.Application.Common.Interfaces;
public interface IUserServiceDbContext
{
    public DbSet<NotificationTemplate> NotificationTemplate { get; set; }
    public DbSet<NotificationType> NotificationTypes { get; set; }
    public DbSet<ClientAccount> ClientAccount { get; set; }
    public DbSet<CreditCard> CreditCard { get; set; }


    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

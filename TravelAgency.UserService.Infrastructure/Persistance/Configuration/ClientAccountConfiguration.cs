using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelAgency.UserService.Domain.Entities;

namespace TravelAgency.UserService.Infrastructure.Persistance.Configuration;
public sealed class ClientAccountConfiguration : IEntityTypeConfiguration<ClientAccount>
{
    public void Configure(EntityTypeBuilder<ClientAccount> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.UserId)
            .IsRequired();

        builder
        .HasOne(x => x.CreditCard)
        .WithOne(x => x.ClientAccount)
        .HasForeignKey<ClientAccount>(x => x.CreditCardId);
    }
}

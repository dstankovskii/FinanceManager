using FinanceManager.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceManager.Data.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<ClientEntity>
{
    public void Configure(EntityTypeBuilder<ClientEntity> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Balance)
               .HasColumnType("decimal(18,2)");

        builder.HasMany(c => c.Transactions)
               .WithOne(t => t.Client)
               .HasForeignKey(t => t.ClientId);
    }
}

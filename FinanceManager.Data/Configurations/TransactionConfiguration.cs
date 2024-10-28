using FinanceManager.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FinanceManager.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<TransactionEntity>
{
    public void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.DateTime)
               .HasColumnType("timestamptz") 
               .IsRequired();

        builder.Property(t => t.Amount)
               .HasColumnType("decimal(18,2)");

        builder.Property(t => t.RevertedAt)
               .HasColumnType("timestamptz");

        builder.Property(t => t.Type)
               .HasConversion<string>();

        builder.HasOne(t => t.Client)
               .WithMany(c => c.Transactions)
               .HasForeignKey(t => t.ClientId);
    }
}

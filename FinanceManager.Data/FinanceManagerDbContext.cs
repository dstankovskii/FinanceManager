using Microsoft.EntityFrameworkCore;
using FinanceManager.Data.Entities;

namespace FinanceManager.Data;

public class FinanceManagerDbContext : DbContext
{
    public FinanceManagerDbContext(DbContextOptions<FinanceManagerDbContext> options)
        : base(options) { }

    public DbSet<ClientEntity> Clients { get; set; }
    public DbSet<TransactionEntity> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}

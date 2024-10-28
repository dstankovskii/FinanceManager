using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FinanceManager.Data;

public class FinanceManagerDbContextFactory : IDesignTimeDbContextFactory<FinanceManagerDbContext>
{
    public FinanceManagerDbContext CreateDbContext(string[] args)
    {
        string connectionStr = "Host=localhost;Port=5432;Database=finance_manager_db;Username=ds1;Password=Temp3232";

        var optionsBuilder = new DbContextOptionsBuilder<FinanceManagerDbContext>();

        optionsBuilder
            .UseNpgsql(connectionStr)
            .UseSnakeCaseNamingConvention();

        return new FinanceManagerDbContext(optionsBuilder.Options);
    }
}

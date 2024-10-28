using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.EntityFrameworkCore;
using FinanceManager.Data;
using FinanceManager.BL.Interfaces.Services;
using FinanceManager.BL.Services;

namespace FinanceManager.IntegrationTests;

public abstract class BaseUnitTest : IDisposable
{
    protected ServiceProvider _serviceProvider;
    protected FinanceManagerDbContext _dbContext;
    protected ILogger<BaseUnitTest> _logger;

    public BaseUnitTest()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();

        var services = new ServiceCollection();

        services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(Log.Logger, dispose: true);
        });

        services.AddDbContext<FinanceManagerDbContext>(options =>
        {
            options
                .UseNpgsql("Host=localhost;Port=5432;Database=finance_manager_db;Username=ds1;Password=Temp3232")
                .UseSnakeCaseNamingConvention();
        });

        services.AddScoped<ITransactionService, TransactionService>();

        _serviceProvider = services.BuildServiceProvider();

        _dbContext = _serviceProvider.GetRequiredService<FinanceManagerDbContext>();

        _logger = _serviceProvider.GetService<ILogger<BaseUnitTest>>()
            ?? throw new InvalidOperationException("Logger not configured properly.");
    }

    public void Dispose()
    {
        _dbContext.Dispose();
        _serviceProvider?.Dispose();
        Log.CloseAndFlush();
    }
}

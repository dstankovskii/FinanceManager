using FinanceManager.BL.Interfaces.Services;
using FinanceManager.BL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceManager.BL.Extensions;

public static class BLServiceCollectionExtensions
{
    public static IServiceCollection AddBLServices(this IServiceCollection services)
    {
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<ITransactionService, TransactionService>();

        return services;
    }
}

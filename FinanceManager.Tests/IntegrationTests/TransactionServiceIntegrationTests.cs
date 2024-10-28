using Xunit;
using Microsoft.Extensions.DependencyInjection;
using FinanceManager.BL.DTOs;
using FinanceManager.BL.Interfaces.Services;
using FinanceManager.Data.Entities;
using FinanceManager.Shared.Enums;

namespace FinanceManager.IntegrationTests;

public class TransactionServiceIntegrationTests : BaseUnitTest
{
    private readonly ITransactionService _transactionService;

    public TransactionServiceIntegrationTests()
    {
        // Get the service through DI
        _transactionService = _serviceProvider.GetRequiredService<ITransactionService>();
    }

    [Fact]
    public async Task ProcessTransactionAsync_CreditAndDebit_ShouldUpdateBalanceCorrectly()
    {
        // Step 1: Create a client in the database
        var clientId = Guid.NewGuid();
        var clientEntity = new ClientEntity
        {
            Id = clientId,
            Balance = 0.0m,
        };

        _dbContext.Clients.Add(clientEntity);
        await _dbContext.SaveChangesAsync();

        // Step 2: Credit 1000 units
        var creditDto = new TransactionDTO
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            DateTime = DateTime.UtcNow,
            Amount = 1000.0m
        };

        var creditResult = await _transactionService.ProcessTransactionAsync(creditDto, TransactionType.Credit);
        Assert.Equal(1000.0m, creditResult.ClientBalance);

        // Step 3: Debit 25 units
        var debitDto = new TransactionDTO
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            DateTime = DateTime.UtcNow,
            Amount = 25.0m
        };

        var debitResult = await _transactionService.ProcessTransactionAsync(debitDto, TransactionType.Debit);
        Assert.Equal(975.0m, debitResult.ClientBalance);
    }
}

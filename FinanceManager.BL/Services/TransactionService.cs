using FinanceManager.BL.DTOs;
using FinanceManager.BL.Helpers;
using FinanceManager.BL.Interfaces;
using FinanceManager.BL.Interfaces.Services;
using FinanceManager.BL.Mapping;
using FinanceManager.Data;
using FinanceManager.Shared.Enums;
using FinanceManager.Shared.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanceManager.BL.Services;

public class TransactionService : ITransactionService
{
    private readonly FinanceManagerDbContext _context;
    private readonly ILogger<TransactionService> _logger;

    public TransactionService(
        FinanceManagerDbContext context,
        ILogger<TransactionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TransactionResponseDTO> ProcessTransactionAsync(TransactionDTO dto, TransactionType transactionType)
    {
        using var dbTransaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var transactionEntity = await _context.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == dto.Id);

            var clientEntity = await _context.Clients.FindAsync(dto.ClientId);
            if (clientEntity == null)
                throw new NotFoundException("Client not found.");

            var client = ClientMapper.FromEntity(clientEntity);

            // this method should be idempotent
            if (transactionEntity == null)
            {
                ITransaction transaction = TransactionMapper.FromDto(dto, transactionType);

                transaction.Process(client);

                transactionEntity = TransactionMapper.MapToEntity(transaction);
                _context.Transactions.Add(transactionEntity);

                ClientMapper.ApplyToEntity(client, clientEntity);

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }

            return new TransactionResponseDTO
            {
                InsertDateTime = DateTimeHelper.ConvertToUtcString(transactionEntity?.DateTime ?? DateTime.UtcNow),
                ClientBalance = client.Balance,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during transaction processing. TransactionId: {TransactionId}", dto.Id);
            await dbTransaction.RollbackAsync();
            throw;
        }
    }

    public async Task<RevertTransactionResponseDTO> RevertTransactionAsync(Guid id)
    {
        using var dbTransaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var transactionEntity = await _context.Transactions
                .Include(t => t.Client)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (transactionEntity == null)
                throw new NotFoundException("Transaction not found.");

            var clientEntity = transactionEntity.Client;
            if (clientEntity == null)
                throw new NotFoundException("Client not found.");

            var client = ClientMapper.FromEntity(clientEntity);

            // this method should be idempotent
            if (!transactionEntity.RevertedAt.HasValue)
            {
                ITransaction transaction = TransactionMapper.FromEntity(transactionEntity);

                transaction.Revert(client);

                TransactionMapper.ApplyToEntity(transaction, transactionEntity);
                ClientMapper.ApplyToEntity(client, clientEntity);

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }

            return new RevertTransactionResponseDTO
            {
                RevertDateTime = DateTimeHelper.ConvertToUtcString(transactionEntity?.DateTime ?? DateTime.UtcNow),
                ClientBalance = client.Balance,
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during transaction revert. TransactionId: {TransactionId}", id);
            await dbTransaction.RollbackAsync();
            throw;
        }
    }
}

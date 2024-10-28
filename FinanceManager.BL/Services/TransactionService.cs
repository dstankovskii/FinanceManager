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
            }

            return new TransactionResponseDTO
            {
                InsertDateTime = DateTimeHelper.ConvertToUtcString(transactionEntity?.DateTime ?? DateTime.UtcNow),
                ClientBalance = client.Balance,
            };
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency conflict occurred during transaction processing. TransactionId: {TransactionId}", dto.Id);
            throw new InvalidOperationException("A concurrency conflict occurred. Please try again.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during transaction processing. TransactionId: {TransactionId}", dto.Id);
            throw;
        }
    }

    public async Task<RevertTransactionResponseDTO> RevertTransactionAsync(Guid id)
    {
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
            }

            return new RevertTransactionResponseDTO
            {
                RevertDateTime = DateTimeHelper.ConvertToUtcString(transactionEntity?.DateTime ?? DateTime.UtcNow),
                ClientBalance = client.Balance,
            };
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogError(ex, "Concurrency conflict occurred during transaction processing. TransactionId: {TransactionId}", id);
            throw new InvalidOperationException("A concurrency conflict occurred. Please try again.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during transaction revert. TransactionId: {TransactionId}", id);
            throw;
        }
    }
}

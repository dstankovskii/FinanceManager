using FinanceManager.BL.DTOs;
using FinanceManager.BL.Interfaces;
using FinanceManager.BL.Models;
using FinanceManager.Data.Entities;
using FinanceManager.Shared.Enums;

namespace FinanceManager.BL.Mapping;

public static class TransactionMapper
{
    public static ITransaction FromDto(TransactionDTO dto, TransactionType transactionType)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        return transactionType switch
        {
            TransactionType.Credit => new CreditTransaction(dto.Id, dto.ClientId, dto.DateTime.UtcDateTime, dto.Amount),
            TransactionType.Debit => new DebitTransaction(dto.Id, dto.ClientId, dto.DateTime.UtcDateTime, dto.Amount),
            _ => throw new InvalidOperationException("Unknown transaction type.")
        };
    }

    public static ITransaction FromEntity(TransactionEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return entity.Type switch
        {
            TransactionType.Credit => new CreditTransaction(entity.Id, entity.ClientId, entity.DateTime, entity.Amount, entity.RevertedAt),
            TransactionType.Debit => new DebitTransaction(entity.Id, entity.ClientId, entity.DateTime, entity.Amount, entity.RevertedAt),
            _ => throw new InvalidOperationException("Unknown transaction type.")
        };
    }

    public static TransactionEntity MapToEntity(ITransaction transaction)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));

        return transaction switch
        {
            CreditTransaction credit => new TransactionEntity
            {
                Id = credit.Id,
                ClientId = credit.ClientId,
                DateTime = credit.DateTime,
                Amount = credit.Amount,
                RevertedAt = credit.RevertedAt,
                Type = TransactionType.Credit
            },
            DebitTransaction debit => new TransactionEntity
            {
                Id = debit.Id,
                ClientId = debit.ClientId,
                DateTime = debit.DateTime,
                Amount = debit.Amount,
                RevertedAt = debit.RevertedAt,
                Type = TransactionType.Debit
            },
            _ => throw new InvalidOperationException("Unknown transaction type.")
        };
    }

    public static void ApplyToEntity(ITransaction transaction, TransactionEntity entity)
    {
        if (transaction == null) throw new ArgumentNullException(nameof(transaction));
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        entity.RevertedAt = transaction.RevertedAt;
    }
}

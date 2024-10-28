using FinanceManager.BL.DTOs;
using FinanceManager.Shared.Enums;

namespace FinanceManager.BL.Interfaces.Services;

public interface ITransactionService
{
    Task<TransactionResponseDTO> ProcessTransactionAsync(TransactionDTO dto, TransactionType transactionType);
    Task<RevertTransactionResponseDTO> RevertTransactionAsync(Guid id);
}

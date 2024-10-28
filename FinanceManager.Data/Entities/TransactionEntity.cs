using FinanceManager.Shared.Enums;

namespace FinanceManager.Data.Entities;

public class TransactionEntity
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public DateTime DateTime { get; set; }
    public decimal Amount { get; set; }
    public DateTime? RevertedAt { get; set; }
    public TransactionType Type { get; set; }

    public ClientEntity? Client { get; set; }
}

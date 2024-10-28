using FinanceManager.BL.Interfaces;

namespace FinanceManager.BL.Models;

public class DebitTransaction : ITransaction
{
    public Guid Id { get; }
    public Guid ClientId { get; }
    public DateTime DateTime { get; }
    public decimal Amount { get; }
    public DateTime? RevertedAt { get; private set; }

    public DebitTransaction(Guid id, Guid clientId, DateTime dateTime, decimal amount, DateTime? revertedAt = null)
    {
        if (dateTime > DateTime.UtcNow)
            throw new ArgumentException("Transaction date cannot be in the future.", nameof(dateTime));

        if (amount <= 0)
            throw new ArgumentException("Amount must be greater than zero.", nameof(amount));

        Id = id;
        ClientId = clientId;
        DateTime = dateTime;
        Amount = amount;
        RevertedAt = revertedAt;
    }

    public void Process(IFinanceManager financeManager)
    {
        financeManager.DecreaseBalance(Amount);
    }

    public void Revert(IFinanceManager financeManager)
    {
        financeManager.IncreaseBalance(Amount);
        RevertedAt = DateTime.UtcNow;
    }
}

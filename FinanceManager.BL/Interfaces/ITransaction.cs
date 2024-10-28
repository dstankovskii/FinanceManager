namespace FinanceManager.BL.Interfaces;

public interface ITransaction
{
    Guid Id { get; }
    Guid ClientId { get; }
    DateTime DateTime { get; }
    decimal Amount { get; }
    DateTime? RevertedAt { get; }

    void Process(IFinanceManager financeManager);
    void Revert(IFinanceManager financeManager);
}

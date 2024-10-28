namespace FinanceManager.BL.Interfaces;

public interface IFinanceManager
{
    decimal Balance { get; }

    void IncreaseBalance(decimal amount);
    void DecreaseBalance(decimal amount);
}

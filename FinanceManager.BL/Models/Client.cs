using FinanceManager.BL.Interfaces;

namespace FinanceManager.BL.Models;

public class Client : IFinanceManager
{
    public Guid Id { get; }
    public decimal Balance { get; private set; }

    public Client(Guid id, decimal balance)
    {
        if (balance < 0)
            throw new ArgumentException("Initial balance cannot be negative.", nameof(balance));

        Id = id;
        Balance = balance;
    }

    public void IncreaseBalance(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.", nameof(amount));

        Balance += amount;
    }

    public void DecreaseBalance(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive.", nameof(amount));
        if (Balance - amount < 0)
            throw new InvalidOperationException("Insufficient balance.");

        Balance -= amount;
    }
}

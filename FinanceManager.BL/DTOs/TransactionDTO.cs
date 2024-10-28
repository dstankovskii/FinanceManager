namespace FinanceManager.BL.DTOs;

public class TransactionDTO
{
    public Guid Id { get; set; }
    public Guid ClientId { get; set; }
    public DateTimeOffset DateTime { get; set; }
    public decimal Amount { get; set; }
}

using System.ComponentModel.DataAnnotations;

namespace FinanceManager.Data.Entities;

public class ClientEntity
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
    
    [Timestamp]
    public byte[]? RowVersion { get; set; }

    public ICollection<TransactionEntity> Transactions { get; set; } = new List<TransactionEntity>();
}

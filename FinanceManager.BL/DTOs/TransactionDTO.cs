using System.ComponentModel.DataAnnotations;

namespace FinanceManager.BL.DTOs;

public class TransactionDTO
{
    [Required(ErrorMessage = "Transaction ID is required.")]
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Client ID is required.")]
    public Guid ClientId { get; set; }

    [Required(ErrorMessage = "Transaction date and time are required.")]
    public DateTimeOffset DateTime { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
    public decimal Amount { get; set; }
}

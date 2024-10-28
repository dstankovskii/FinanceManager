using System.ComponentModel.DataAnnotations;

namespace FinanceManager.BL.DTOs;

public class RevertTransactionDTO
{
    [Required(ErrorMessage = "Transaction ID is required.")]
    public Guid Id { get; set; }
}

namespace FinanceManager.BL.DTOs;

public class RevertTransactionResponseDTO
{
    public string RevertDateTime { get; set; } = "";
    public decimal ClientBalance { get; set; }
}
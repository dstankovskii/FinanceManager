namespace FinanceManager.BL.DTOs;

public class ClientBalanceResponseDTO
{
    public string BalanceDateTime { get; set; } = "";
    public decimal ClientBalance { get; set; }
}
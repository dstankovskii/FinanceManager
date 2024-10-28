using FinanceManager.BL.DTOs;

namespace FinanceManager.BL.Interfaces.Services;

public interface IClientService
{
    Task<ClientBalanceResponseDTO> CreateClientAsync(Guid id);
    Task<ClientBalanceResponseDTO> GetClientBalanceAsync(Guid id);
}

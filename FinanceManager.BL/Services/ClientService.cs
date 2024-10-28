using FinanceManager.BL.DTOs;
using FinanceManager.BL.Helpers;
using FinanceManager.BL.Interfaces.Services;
using FinanceManager.Data;
using FinanceManager.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceManager.BL.Services;

public class ClientService : IClientService
{
    private readonly FinanceManagerDbContext _context;

    public ClientService(FinanceManagerDbContext context)
    {
        _context = context;
    }

    public async Task<ClientBalanceResponseDTO> CreateClientAsync(Guid id)
    {
        var clientEntity = await _context.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);

        if (clientEntity == null)
        {
            clientEntity = new ClientEntity
            {
                Id = id,
                Balance = 0.0m,
            };

            _context.Clients.Add(clientEntity);
            await _context.SaveChangesAsync();
        }

        return new ClientBalanceResponseDTO
        {
            BalanceDateTime = DateTimeHelper.ConvertToUtcString(DateTime.UtcNow),
            ClientBalance = clientEntity.Balance,
        };
    }

    public async Task<ClientBalanceResponseDTO> GetClientBalanceAsync(Guid id)
    {
        var clientEntity = await _context.Clients
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.Id == id);

        if (clientEntity == null)
            throw new Exception("Client not found.");

        return new ClientBalanceResponseDTO
        {
            BalanceDateTime = DateTimeHelper.ConvertToUtcString(DateTime.UtcNow),
            ClientBalance = clientEntity.Balance,
        };
    }
}

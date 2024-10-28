using FinanceManager.BL.Models;
using FinanceManager.Data.Entities;

namespace FinanceManager.BL.Mapping;

public static class ClientMapper
{
    public static Client FromEntity(ClientEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return new Client(entity.Id, entity.Balance);
    }

    public static void ApplyToEntity(Client client, ClientEntity entity)
    {
        if (client == null) throw new ArgumentNullException(nameof(client));
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        entity.Balance = client.Balance;
    }
}

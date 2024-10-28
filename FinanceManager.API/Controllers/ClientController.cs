using Microsoft.AspNetCore.Mvc;
using FinanceManager.BL.Interfaces.Services;
using FinanceManager.Shared.Exceptions;

namespace FinanceManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpPost("create")]
    public async Task<IResult> CreateClient([FromQuery] Guid id)
    {
        if (id == Guid.Empty)
        {
            return Results.Problem("Invalid client ID.", statusCode: 400);
        }

        try
        {
            var result = await _clientService.CreateClientAsync(id);
            return Results.Ok(result);
        }
        catch
        {
            return Results.Problem("An unexpected error occurred.", statusCode: 500);
        }
    }

    [HttpGet("balance")]
    public async Task<IResult> GetClientBalance([FromQuery] Guid id)
    {
        if (id == Guid.Empty)
        {
            return Results.Problem("Invalid client ID.", statusCode: 400);
        }

        try
        {
            var result = await _clientService.GetClientBalanceAsync(id);
            return Results.Ok(result);
        }
        catch (NotFoundException ex)
        {
            return Results.Problem(ex.Message, statusCode: 404);
        }
        catch
        {
            return Results.Problem("An unexpected error occurred.", statusCode: 500);
        }
    }
}

using FinanceManager.BL.DTOs;
using Microsoft.AspNetCore.Mvc;
using FinanceManager.BL.Interfaces.Services;
using FinanceManager.Shared.Enums;

namespace FinanceManager.API.Controllers;

[ApiController]
public class FinanceManagerController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IClientService _clientService;

    public FinanceManagerController(ITransactionService transactionService, IClientService clientService)
    {
        _transactionService = transactionService;
        _clientService = clientService;
    }

    [HttpPost("credit")]
    public async Task<IActionResult> Credit([FromBody] TransactionDTO dto)
    {
        var result = await _transactionService.ProcessTransactionAsync(dto, TransactionType.Credit);
        return Ok(result);
    }

    [HttpPost("debit")]
    public async Task<IActionResult> Debit([FromBody] TransactionDTO dto)
    {
        var result = await _transactionService.ProcessTransactionAsync(dto, TransactionType.Debit);
        return Ok(result);
    }

    [HttpPost("revert")]
    public async Task<IActionResult> Revert([FromQuery] Guid id)
    {
        var result = await _transactionService.RevertTransactionAsync(id);
        return Ok(result);
    }

    [HttpGet("balance")]
    public async Task<IActionResult> GetClientBalance([FromQuery] Guid id)
    {
        var result = await _clientService.GetClientBalanceAsync(id);
        return Ok(result);
    }
}

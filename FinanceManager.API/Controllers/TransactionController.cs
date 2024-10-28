using FinanceManager.BL.DTOs;
using Microsoft.AspNetCore.Mvc;
using FinanceManager.BL.Interfaces.Services;
using FinanceManager.Shared.Enums;
using FinanceManager.Shared.Exceptions;

namespace FinanceManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    [HttpPost("credit")]
    public async Task<IResult> Credit([FromBody] TransactionDTO dto)
    {
        if (!ModelState.IsValid)
        {
            var validationErrors = ModelState
                .Where(kvp => kvp.Value != null && kvp.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return Results.ValidationProblem(validationErrors);
        }

        try
        {
            var result = await _transactionService.ProcessTransactionAsync(dto, TransactionType.Credit);
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

    [HttpPost("debit")]
    public async Task<IResult> Debit([FromBody] TransactionDTO dto)
    {
        if (!ModelState.IsValid)
        {
            var validationErrors = ModelState
                .Where(kvp => kvp.Value != null && kvp.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            return Results.ValidationProblem(validationErrors);
        }

        try
        {
            var result = await _transactionService.ProcessTransactionAsync(dto, TransactionType.Debit);
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

    [HttpPost("revert")]
    public async Task<IResult> Revert([FromQuery] Guid id)
    {
        if (id == Guid.Empty)
        {
            return Results.Problem("Invalid transaction ID.", statusCode: 400);
        }

        try
        {
            var result = await _transactionService.RevertTransactionAsync(id);
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

using System.Security.Claims;
using FinancialTransactionAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinancialTransactionAPI.Controllers;

/// <summary>
/// Handles all financial transactions including deposits, withdrawals and transfers.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly TransactionService _transactionService;

    public TransactionsController(TransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    /// <summary>
    /// Deposits funds into a specified account.
    /// </summary>
    /// <remarks>
    /// The account must belong to the authenticated user.
    /// </remarks>
    /// <param name="request">Account ID, amount and description</param>
    /// <returns>Transaction details</returns>
    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] TransactionRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var transaction = await _transactionService.DepositAsync(request.AccountId, userId, request.Amount, request.Description);

        if (transaction == null)
            return BadRequest(new { message = "Account not found or invalid." });

        return Ok(transaction);
    }

    /// <summary>
    /// Withdraws funds from a specified account.
    /// </summary>
    /// <remarks>
    /// The account must belong to the authenticated user and have sufficient balance.
    /// </remarks>
    /// <param name="request">Account ID, amount and description</param>
    /// <returns>Transaction details</returns>
    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] TransactionRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var transaction = await _transactionService.WithdrawAsync(request.AccountId, userId, request.Amount, request.Description);

        if (transaction == null)
            return BadRequest(new { message = "Insufficient funds or account not found." });

        return Ok(transaction);
    }

    /// <summary>
    /// Transfers funds between two accounts.
    /// </summary>
    /// <remarks>
    /// The source account must belong to the authenticated user and have sufficient balance.
    /// The destination account can belong to any user.
    /// </remarks>
    /// <param name="request">Source account, destination account, amount and description</param>
    /// <returns>Transaction details</returns>
    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer([FromBody] TransferRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var transaction = await _transactionService.TransferAsync(request.FromAccountId, request.ToAccountId, userId, request.Amount, request.Description);

        if (transaction == null)
            return BadRequest(new { message = "Insufficient funds or account not found." });

        return Ok(transaction);
    }

    /// <summary>
    /// Returns the transaction history for a specified account.
    /// </summary>
    /// <remarks>
    /// The account must belong to the authenticated user. Results are ordered by date descending.
    /// </remarks>
    /// <param name="accountId">The account ID (GUID)</param>
    /// <returns>List of transactions</returns>
    [HttpGet("{accountId}")]
    public async Task<IActionResult> GetByAccount(Guid accountId)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var transactions = await _transactionService.GetByAccountAsync(accountId, userId);
        return Ok(transactions);
    }
}

public record TransactionRequest(Guid AccountId, decimal Amount, string Description);
public record TransferRequest(Guid FromAccountId, Guid ToAccountId, decimal Amount, string Description);